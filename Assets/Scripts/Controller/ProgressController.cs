using Assets.Scripts.Fabric;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class ProgressController : MonoBehaviour
    {
        [SerializeField] private BlockFabric _blockFabric;
        [SerializeField] private GameController _gameController;
        [SerializeField] private GameObject _spawnPoint;
        
        private const float DeltaPos = 2f;

        [SerializeField] private float _stageFlippingSpeed;

        private Vector3 _startCameraPos;
        private Vector3 _startSpawnPointPos;

        private bool _isBlockFalling = false;

        private void Awake()
        {
            PrepareDelegates();
        }

        private void Start()
        {
            Prepare();
        }

        private void PrepareDelegates()
        {
            _blockFabric.onBlockGenerated += OnBlockGenerated;
            _gameController.onGameLost += ResetProgress;
        }
 
        private void OnBlockGenerated()
        {
            MoveSpawnPoint();

            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 targetPos = new Vector3(cameraPos.x, cameraPos.y + DeltaPos, cameraPos.z);
            StartCoroutine(MoveCamera(targetPos));
        }

        private void ResetProgress()
        {
            ResetSpawnPointPosition();
            ResetCameraPosition();
        }       

        private void Prepare()
        {
            _startCameraPos = Camera.main.transform.position;
            _startSpawnPointPos = _spawnPoint.transform.position;
        }

        private void MoveSpawnPoint()
        {             
            Vector3 pointPos = _spawnPoint.transform.position;
            Vector3 newPos = new Vector3(pointPos.x, pointPos.y + DeltaPos, pointPos.z);

            _spawnPoint.transform.position = newPos;
        }

        private IEnumerator MoveCamera(Vector3 targetPos)
        {            
            Vector3 startPos = Camera.main.transform.position;
            
            float startTime = Time.realtimeSinceStartup;
            float fraction = 0;

            while (fraction < 1)
            {
                fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / _stageFlippingSpeed);
                Camera.main.transform.position = Vector3.Lerp(startPos, targetPos, fraction); 
                yield return null;
            }
        }      
      
        private void ResetCameraPosition()
        {
            StartCoroutine(MoveCamera(_startCameraPos));
        }

        private void ResetSpawnPointPosition()
        {
            _spawnPoint.transform.position = _startSpawnPointPos;
        }
    }
}
