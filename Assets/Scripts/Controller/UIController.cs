using System;
using Assets.Scripts.Data;
using Assets.Scripts.Service;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _bestScoreText;

        [SerializeField] private Image _backGround;

        [SerializeField] private GameController _gameController;
        [SerializeField] private LocalDataService _localDataService;
 
        [SerializeField] private Animator _animator;

        private int _currentScore = 0;
        private int _bestScore = 0;

        private PlayerData _playerData;

        private void Awake()
        {
            PrepareDelegates();
        }

        private void PrepareDelegates()
        {
            _gameController.onGameLost += GameLost;
            _gameController.onGameStart += StartGame;
            _gameController.onBlockStacked += IncreaseScore;
            _localDataService.onDataLoaded += SetupDataFromLocal;
            _localDataService.onScreenShootLoaded += SetupBackGround;
        }

        private void StartGame()
        {
            _animator.SetTrigger("NewGame");
        }

        private void SetupBackGround(Sprite screenShoot)
        {
            Debug.Log(screenShoot.GetHashCode());
            _backGround.sprite = screenShoot;
        }

        private void SetupDataFromLocal(PlayerData currentData)
        {
            _playerData = currentData;
            _bestScore = _playerData.playerScore;
            UpdateScoreText(_bestScoreText, _bestScore);
        } 

        public void IncreaseScore()
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

        private void UpdateScreenShoot()
        {
            _localDataService.UpdateScreenShoot();
        }

        private void GameLost()
        {
            UpdateScores();
            UpdateScreenShoot();
            _animator.SetTrigger("LostGame");
            
        }      
    }
}