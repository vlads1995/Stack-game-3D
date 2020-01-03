﻿using Assets.Scripts.Fabric;
using Assets.Scripts.Model;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class BlockController : MonoBehaviour
    {
        public delegate void BlockStacked();
        public BlockStacked onBlockStacked;

        [SerializeField] GameController _gameController;
        [SerializeField] BlockFabric _blockFabric;

        [SerializeField] public float _speed = 2.0f;        

        private const int MaxBoundary = 10;

        [SerializeField] private GameObject _startBlock;

        private GameObject _currentBlock;
        private GameObject _lastBlock;

        private bool _isForward = true;

        private void Awake()
        {
            _gameController.onGameLost += StopMoving;
            _gameController.onGameStart += GenerateNewBlock;
        }

        private void Start()
        {
            _lastBlock = _startBlock;
        }

        public void SetNewBlock(GameObject newBlock)
        {
            if(_currentBlock)
            {
                _currentBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                _lastBlock = _currentBlock;
            }            

            _currentBlock = newBlock;

            if(_lastBlock != _startBlock)
            {
                _currentBlock.transform.localScale = _lastBlock.transform.localScale;
            }
           
            StartCoroutine(MoveBlock(_currentBlock.GetComponent<Block>().blockData.MoveDirection));          
        }

        public void ReleaseBlock()
        {
            StopAllCoroutines();

            _currentBlock.GetComponent<Rigidbody>().useGravity = true;
            
            float hangover = _currentBlock.transform.position.z - _lastBlock.transform.position.z;
            float direction = hangover > 0 ? 1f : -1f;

            if(Mathf.Abs(hangover) >= _lastBlock.transform.localScale.z)
            {
                _lastBlock = _startBlock;
                _currentBlock = null;
                _gameController.StartAnimationsOnLose();
                return;
            }

            SplitBlockOnZ(hangover, direction);
            onBlockStacked?.Invoke();            
            

            GenerateNewBlock();
        }

        public void GenerateNewBlock()
        {           
            SetNewBlock(_blockFabric.CreateNewBlock());
        }

        private void SplitBlockOnZ(float hangover, float direction)
        {           
            float newSize = _lastBlock.transform.localScale.z - Mathf.Abs(hangover);
            float fallingBlockSize = _currentBlock.transform.localScale.z - newSize;

            float newPos = _lastBlock.transform.position.z + (hangover / 2);

            _currentBlock.transform.localScale = new Vector3(_currentBlock.transform.localScale.x, _currentBlock.transform.localScale.y, newSize);
            _currentBlock.transform.position = new Vector3(_currentBlock.transform.position.x, _currentBlock.transform.position.y, newPos);

            float cubeEdge = _currentBlock.transform.position.z + (newSize / 2f * direction);
            float fallingBlockPos = cubeEdge + fallingBlockSize / 2f * direction;

            Material material = _currentBlock.GetComponent<Renderer>().material;

            SpawnFallingCube(fallingBlockPos, fallingBlockSize, material);

        }

        private void SpawnFallingCube(float fallingBlockZPos, float fallingBlockSize, Material blockMaterial)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(_currentBlock.transform.localScale.x, _currentBlock.transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(_currentBlock.transform.position.x, _currentBlock.transform.position.y, fallingBlockZPos);

            cube.GetComponent<Renderer>().material = blockMaterial;
            cube.AddComponent<Rigidbody>();

            cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            cube.GetComponent<Rigidbody>().mass = 5f;

            Destroy(cube.gameObject, 1f);
        }

        private IEnumerator MoveBlock(Vector3 direction)
        {  
            while (true)
            {                
                if (_isForward)
                {
                    _currentBlock.transform.Translate(direction * _speed * Time.deltaTime);
                }
                else
                {
                    _currentBlock.transform.Translate(-direction * _speed * Time.deltaTime);
                }

                if (_currentBlock.transform.position.x >= MaxBoundary || _currentBlock.transform.position.z >= MaxBoundary)
                {
                    _isForward = false;
                }

                if (_currentBlock.transform.position.x <= -MaxBoundary || _currentBlock.transform.position.z <= -MaxBoundary)
                {
                    _isForward = true;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private void StopMoving()
        {
            StopAllCoroutines();
            _currentBlock = null;
            _lastBlock = _startBlock;
             
        }

    }
}