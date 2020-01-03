﻿using Assets.Scripts.Data;
using TapticPlugin;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Block : MonoBehaviour
    {
        public delegate void BlockStacked();
        public BlockStacked onBlockStacked;  
        
        public BlockGenerateData blockData { get; private set; }

        public void SetNewData(BlockGenerateData data)
        {
            blockData = data;
        }

        private void OnCollisionEnter(Collision collision)
        {
            TapticManager.Impact(ImpactFeedback.Light);
            FreezeBlockPosition();
            onBlockStacked?.Invoke();
        }

        private void FreezeBlockPosition()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;            
        }
    }
}