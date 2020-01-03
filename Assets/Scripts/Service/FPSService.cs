using UnityEngine;

namespace Assets.Scripts.Service
{
    public class FPSService : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}
