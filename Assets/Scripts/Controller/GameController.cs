using Assets.Scripts.Fabric;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class GameController : MonoBehaviour
    {
        public delegate void LoseGame();
        public LoseGame onGameLost;

        [SerializeField] private BlockFabric _blockFabric;
        [SerializeField] private BlockController _blockController;

        public void GameLost()
        {
            onGameLost?.Invoke();
        }

        public void GenerateBlock()
        {
            _blockController.SetNewBlock(_blockFabric.CreateNewBlock());
        }

    }
}