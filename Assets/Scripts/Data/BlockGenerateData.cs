using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [System.Serializable]
    public class BlockGenerateData
    {        
        [SerializeField] private List<GameObject> _spawnPoints;
        [SerializeField] private Vector3 _moveDirection;

        public List<GameObject> spawnPoints { get { return _spawnPoints; } }
        public Vector3 MoveDirection { get { return _moveDirection; } }
    }
}