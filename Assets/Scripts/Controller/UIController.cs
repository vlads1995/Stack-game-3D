using Assets.Scripts.Data;
using Assets.Scripts.Service;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        [SerializeField] private LocalDataService _localDataService;
        [SerializeField] private CameraZoomService _cameraZoomService;
        [SerializeField] private BlockController _blockController;

        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _bestScoreText;

        [SerializeField] private RawImage _menuBack;
        [SerializeField] private Material _gameBackMaterial;
        
        [SerializeField] private Animator _animator;

        private int _currentScore = 0;
        private int _bestScore = 0;

        private PlayerData _playerData;

        private Texture2D backgroundTexture;

        private void Awake()
        {
            PrepareDelegates();
            PrepareTexture();
        }

        private void PrepareTexture()
        {
            backgroundTexture = new Texture2D(1, 2);
            backgroundTexture.wrapMode = TextureWrapMode.Clamp;
            backgroundTexture.filterMode = FilterMode.Bilinear;           
        }

        private void Start()
        {
            UpdateScores();
        }

        private void PrepareDelegates()
        {
            _gameController.onGameLostAnimations += LostGame;
            _gameController.onGameStart += StartGame;

            _blockController.onBlockStacked += IncreaseScore;

            _localDataService.onDataLoaded += SetupDataFromLocal;
            _localDataService.onScreenShootLoaded += SetupBackGround;

            _cameraZoomService.onZoomedOut += GameLost;
        }

        public void UpdateScreenShoot()
        {
            _localDataService.UpdateScreenShoot();
        }

        public void SetGameLost()
        {
            _gameController.InvokeGameLost();
        }

        private void StartGame()
        {
            SetRandomBack();
            _cameraZoomService.ZoomIn();
            _animator.SetTrigger("NewGame");
        }

        private void SetRandomBack()
        {           
            SetColor(UnityEngine.Random.ColorHSV(), UnityEngine.Random.ColorHSV());
        }

        public void SetColor(Color color1, Color color2)
        {
            backgroundTexture.SetPixels(new Color[] { color1, color2 });
            backgroundTexture.Apply();            
            _gameBackMaterial.SetTexture("_MainTex", backgroundTexture);
        }  

        private void LostGame()
        {
            _cameraZoomService.ZoomOut();
        }

        private void SetupBackGround(Texture2D screenShoot)
        {            
            if(screenShoot == null)
            {
                SetRandomBack();

                screenShoot = backgroundTexture;
            }

            _menuBack.texture = screenShoot;
        }

        private void SetupDataFromLocal(PlayerData currentData)
        {
            _playerData = currentData;
            _bestScore = _playerData.playerScore;
            UpdateScoreText(_bestScoreText, _bestScore);
        } 

        private void IncreaseScore()
        {
            _currentScore++;
            UpdateScoreText(_scoreText, _currentScore);
        }       

        private void UpdateScores()
        {     
            if (_currentScore > _bestScore)
            {
                _bestScore = _currentScore;
                _playerData.playerScore = _bestScore;                
                UpdateScoreText(_bestScoreText, _bestScore);
            }

            _currentScore = 0;
            UpdateScoreText(_scoreText, _currentScore);

            _localDataService.SetNewDataAndSave(_playerData);            
        } 

        private void UpdateScoreText(Text updateText, int score)
        {
            updateText.text = score.ToString();
        }       

        private void GameLost()
        {
            UpdateScores();
            _animator.SetTrigger("LostGame");

            UpdateScreenShoot();
        }
    }
}