using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _bestScoreText;

        [SerializeField] private GameController _gameController;

        private int _currentScore = 0;
        private int _bestScore = 0;

        private void Awake()
        {
            PrepareDelegates();
        }

        private void PrepareDelegates()
        {
            _gameController.onGameLost += GameLost;
            _gameController.onBlockStacked += IncreaseScore;
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
                //save in file
                UpdateScoreText(_bestScoreText, _bestScore);
            }

            _currentScore = 0;
            UpdateScoreText(_scoreText, _currentScore);
        } 

        private void UpdateScoreText(Text updateText, int score)
        {
            updateText.text = score.ToString();
        }

        private void GameLost()
        {
            UpdateScores();
        }

    }
}