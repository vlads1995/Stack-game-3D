using Assets.Scripts.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Service
{
    public class CameraZoomService : MonoBehaviour
    {
        public delegate void ZoomedOut();
        public ZoomedOut onZoomedOut;
        
        private const float DEFAULT_SIZE = 20f;
        private const float EPSILON = 0.1f;

        public float TARGET_SIZE = 40f;

        public float _stageFlippingSpeed;  

        private IEnumerator _currentCoroutine;
         
        public void ZoomIn()
        {
            Camera.main.orthographicSize = DEFAULT_SIZE;
            
        }

        public void ZoomOut()
        {           
            StartCoroutine(MoveCamera(TARGET_SIZE));
        }

        private IEnumerator MoveCamera(float zoomTarget)
        {
            float startTime = Time.realtimeSinceStartup;
            float fraction = 0;
             
            float startSize = Camera.main.orthographicSize;

            while (fraction < 1)
            {
                fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / _stageFlippingSpeed);
                
                Camera.main.orthographicSize = Mathf.Lerp(startSize, zoomTarget, fraction);

                if (fraction == 1)
                {
                    onZoomedOut?.Invoke();
                }

                yield return null;
            }
        }    
    }
}
