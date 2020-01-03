using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Service
{
    public class CameraZoomService : MonoBehaviour
    {        
                
        private const float DEFAULT_SIZE = 12f;
        private const float EPSILON = 0.1f;

        public float TARGET_SIZE = 23;

        public float _stageFlippingSpeed; //public for debug, make private after define

        private IEnumerator _currentCoroutine;
         
        public void ZoomIn()
        {
            //zoom in 
        }

        public void ZoomOut()
        {
           
            StartCoroutine(MoveCamera(TARGET_SIZE));
        }

        private IEnumerator MoveCamera(float zoomTarget)
        {
            float startTime = Time.realtimeSinceStartup;
            float fraction = 0;

            Vector3 startPos = transform.position;
            float startSize = Camera.main.orthographicSize;

            while (fraction < 1)
            {
                fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / _stageFlippingSpeed);
                
                Camera.main.orthographicSize = Mathf.Lerp(startSize, zoomTarget, fraction);

                if (fraction == 1)
                {  
                    //set trigger
                }

                yield return null;
            }
        }    
    }
}
