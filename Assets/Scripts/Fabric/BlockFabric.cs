using Assets.Scripts.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Fabric
{
    public class BlockFabric : MonoBehaviour
    {
        public delegate void BlockGenerated();
        public BlockGenerated onBlockGenerated;

        [SerializeField] private GameController _gameController;

        [Header("Options")]
        [Range(0.001f, 0.05f)]
        [SerializeField] private float _colorDelta = 0.003f;

        [Header("Parameters")]
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private GameObject _spawnPoint;        

        private Gradient _gradient;
        private GradientColorKey[] _colorKeys;
        private GradientAlphaKey[] _alphaKeys;        

        private float _currentEvaluate = 0;   
        private Vector3 _lastStackedBlockSize;

        private GameObject _newBlock;

        private List<GameObject> _allBlocks = new List<GameObject>();

        private void Awake()
        {
            PrepareDelegates();

            ResetColorOptions();
        }

        public GameObject CreateNewBlock()
        {
            _newBlock = Instantiate(_blockPrefab, _spawnPoint.transform.position, Quaternion.identity);
            _allBlocks.Add(_newBlock);
            SetupNewBlock();

            onBlockGenerated?.Invoke();

            return (_newBlock);
        }

        private void PrepareDelegates()
        {
            _gameController.onGameLost += OnGameLost;
        }

        private void OnGameLost()
        {
            ResetColorOptions();
            DestroyAllBlocks();
        }

        private void DestroyAllBlocks()
        {
            foreach (var item in _allBlocks)
            {
                Destroy(item);
            }

            _allBlocks = new List<GameObject>();
        }

        private void ResetColorOptions()
        {
            _gradient = new Gradient();
             
            _colorKeys = new GradientColorKey[2];
            _alphaKeys = new GradientAlphaKey[2];

            _colorKeys[0].color = GetRandomColor();
            _colorKeys[0].time = 0.0f;

            _alphaKeys[0].alpha = 1.0f;
            _alphaKeys[0].time = 0.0f;

            _colorKeys[1].color = GetRandomColor();                
            _colorKeys[1].time = 1.0f;  

            _alphaKeys[1].alpha = 0.0f;
            _alphaKeys[1].time = 1.0f;

            _gradient.SetKeys(_colorKeys, _alphaKeys);

            _currentEvaluate = 0f;
        }

        private Color GetRandomColor()
        {
            return UnityEngine.Random.ColorHSV();
        }   

        private void SetupNewBlock()
        {
            SetColor();
            SetSize();
        }

        private void SetColor()
        {
            _currentEvaluate += _colorDelta;
            _newBlock.GetComponent<MeshRenderer>().material.color = _gradient.Evaluate(_currentEvaluate);
        }

        private void SetSize()
        {

        }
         
    }
}
