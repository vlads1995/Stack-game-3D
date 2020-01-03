using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Service
{
    public class CameraZoomService : MonoBehaviour
    {
        public delegate void ZoomedOut();
        public ZoomedOut onZoomedOut;

        [SerializeField] private GameObject _blocksParent;        
        [SerializeField] private Transform _startBlock;

        private Transform _lastBlock;
        
        private const float DEFAULT_SIZE = 20f;
        private const float EPSILON = 0.1f;
         
        private IEnumerator _currentCoroutine;
         
        public void ZoomIn()
        {
            Camera.main.orthographicSize = DEFAULT_SIZE;            
        }

        public void ZoomOut()
        {
            _lastBlock = _blocksParent.transform.GetChild(0);
            StartCoroutine(MoveCamera());
        }

        private IEnumerator MoveCamera()
        {           
            float delta = 0.2f;

            bool isFirstBlockOnScreen = false;
            bool isLastBlockOnScreen = false;
           
            while (!isFirstBlockOnScreen || !isLastBlockOnScreen)
            {
                Vector3 screenPointFirst = Camera.main.WorldToViewportPoint(_startBlock.position);
                isFirstBlockOnScreen = screenPointFirst.z > 0 && screenPointFirst.x > 0 && screenPointFirst.x < 1 && screenPointFirst.y > 0 && screenPointFirst.y < 1;

                Vector3 screenPointLast = Camera.main.WorldToViewportPoint(_lastBlock.position);
                isLastBlockOnScreen = screenPointLast.z > 0 && screenPointLast.x > 0 && screenPointLast.x < 1 && screenPointLast.y > 0 && screenPointLast.y < 1;
              
                Camera.main.orthographicSize += delta;

                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            onZoomedOut?.Invoke(); 
        }    
    }
}