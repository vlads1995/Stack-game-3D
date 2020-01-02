using Assets.Scripts.Data.Enum;
using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class BlockController : MonoBehaviour
    {
        [SerializeField] public float _speed = 2.0f;

        private const int MaxBoundary = 10;

        private GameObject _currentBlock;
        private bool _isForward = true;
       
        public void SetNewBlock(GameObject newBlock)
        {
            _currentBlock = newBlock;
            StartCoroutine(MoveBlock(_currentBlock.GetComponent<Block>().blockData.MoveDirection));          
        }

        public void ReleaseBlock()
        {
            Debug.Log(_currentBlock);
            _currentBlock.GetComponent<Rigidbody>().useGravity = true;
            StopAllCoroutines();
        } 

        private IEnumerator MoveBlock(Vector3 direction)
        {  
            while (true)
            {                
                if (_isForward)
                {
                    _currentBlock.transform.Translate(direction * _speed * Time.deltaTime);
                }
                else
                {
                    _currentBlock.transform.Translate(-direction * _speed * Time.deltaTime);
                }

                if (_currentBlock.transform.position.x >= MaxBoundary || _currentBlock.transform.position.z >= MaxBoundary)
                {
                    _isForward = false;
                }

                if (_currentBlock.transform.position.x <= -MaxBoundary || _currentBlock.transform.position.z <= -MaxBoundary)
                {
                    _isForward = true;
                }

                yield return new WaitForEndOfFrame();
            }
        } 

    }
}