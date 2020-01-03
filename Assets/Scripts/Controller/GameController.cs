using Assets.Scripts.Fabric;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class GameController : MonoBehaviour
    {
        public delegate void LoseGame();
        public LoseGame onGameLost;

        public delegate void StartGame();
        public StartGame onGameStart;

        public delegate void BlockStacked();
        public BlockStacked onBlockStacked;

        public delegate void StartLoseAnimations();
        public StartLoseAnimations onGameLostAnimations;

        [SerializeField] private BlockFabric _blockFabric;
        [SerializeField] private BlockController _blockController;

        private Block _currentBlock;

        public void StartNewGame()
        {
            GenerateBlock();
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

        private void GenerateBlock()
        {
            DisableOldBlock();
            
            GameObject newBlock = _blockFabric.CreateNewBlock();
            _blockController.SetNewBlock(newBlock);
            _currentBlock = newBlock.GetComponent<Block>();

            NewBlockSubscribe();
        }

        private void DisableOldBlock()
        {
            if(_currentBlock)
            {               
                _currentBlock.onBlockStacked -= GenerateBlock;
                Destroy(_currentBlock);
                onBlockStacked?.Invoke();
            }
        }

        private void NewBlockSubscribe()
        {
            if (_currentBlock)
            {
                _currentBlock.onBlockStacked += GenerateBlock;
            }
        }

    }
}