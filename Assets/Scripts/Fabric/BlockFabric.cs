using Assets.Scripts.Controller;
using Assets.Scripts.Data;
using Assets.Scripts.Model;
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
        [Range(0.05f, 0.2f)]
        [SerializeField] private float _colorDelta = 0.1f;

        [Header("Parameters")]
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private GameObject _spawnPlane;
        [SerializeField] private GameObject _startBlock; 
        [SerializeField] private List<BlockGenerateData> _blockDatas = new List<BlockGenerateData>();

        private Gradient _gradient;
        private GradientColorKey[] _colorKeys;
        private GradientAlphaKey[] _alphaKeys;

        private Color _lastColor;

        private float _currentEvaluate = 0;   
        private Vector3 _lastStackedBlockSize;
 
        private List<GameObject> _allBlocks = new List<GameObject>();

        private void Awake()
        {
            PrepareDelegates();

            CreateNewGradient(GetRandomColor(), GetRandomColor());

            SetupStartBlock();
        }

        private void SetupStartBlock()
        {
            SetColor(_startBlock);
        }

        public GameObject CreateNewBlock()
        {
            BlockGenerateData newData = _blockDatas[UnityEngine.Random.Range(0, _blockDatas.Count)];
            GameObject newSpawnPoint = newData.spawnPoints[UnityEngine.Random.Range(0, newData.spawnPoints.Count)];

            GameObject newBlock = Instantiate(_blockPrefab, newSpawnPoint.transform.position, Quaternion.identity);
            newBlock.GetComponent<Block>().SetNewData(newData);

            _allBlocks.Add(newBlock);
            SetupNewBlock(newBlock);

            onBlockGenerated?.Invoke();

            return (newBlock);
        }

        private void PrepareDelegates()
        {
            _gameController.onGameLost += OnGameLost;
        }

        private void OnGameLost()
        {
            CreateNewGradient(GetRandomColor(), GetRandomColor());
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

        private void CreateNewGradient(Color firstColor, Color secondColor)
        {
            _gradient = new Gradient();
             
            _colorKeys = new GradientColorKey[2];
            _alphaKeys = new GradientAlphaKey[2];

            _colorKeys[0].color = firstColor;
            _colorKeys[0].time = 0.0f;

            _alphaKeys[0].alpha = 1.0f;
            _alphaKeys[0].time = 0.0f;

            _colorKeys[1].color = secondColor;                
            _colorKeys[1].time = 1.0f;  

            _alphaKeys[1].alpha = 0.0f;
            _alphaKeys[1].time = 1.0f;

            _gradient.SetKeys(_colorKeys, _alphaKeys);

            _lastColor = secondColor;
            _currentEvaluate = 0f;
        }

        private Color GetRandomColor()
        {
            return UnityEngine.Random.ColorHSV();
        }   

        private void SetupNewBlock(GameObject newBlock)
        {
            SetColor(newBlock);
            SetSize(newBlock);
        }

        private void SetColor(GameObject newBlock)
        {
            _currentEvaluate += _colorDelta;

            CheckIsColorReached();

            newBlock.GetComponent<MeshRenderer>().material.color = _gradient.Evaluate(_currentEvaluate);
        }

        private void CheckIsColorReached()
        {
            if(_currentEvaluate >= 1)
            {
                CreateNewGradient(_lastColor, GetRandomColor());
                _currentEvaluate = 0f;
            }
        }

        private void SetSize(GameObject newBlock)
        {

        }
         
    }
}
