using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class BlockController : MonoBehaviour
    {
        private GameObject _currentBlock;

        public void SetNewBlock(GameObject newBlock)
        {
            _currentBlock = newBlock;
            //move block
        }
    }
}