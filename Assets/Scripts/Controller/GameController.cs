using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class GameController : MonoBehaviour
    {
        public delegate void LoseGame();
        public LoseGame onGameLost;

        public delegate void StartGame();
        public StartGame onGameStart;

        public delegate void StartLoseAnimations();
        public StartLoseAnimations onGameLostAnimations;

        public void StartNewGame()
        {            
            onGameStart?.Invoke();
        }     

        public void StartAnimationsOnLose()
        {
            onGameLostAnimations?.Invoke();
        }      
        
        public void InvokeGameLost()
        {
            onGameLost?.Invoke();
        }
    }
}