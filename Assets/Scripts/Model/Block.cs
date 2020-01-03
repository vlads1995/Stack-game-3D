using Assets.Scripts.Data;
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
    }
}