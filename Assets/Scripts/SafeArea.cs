using UnityEngine;

namespace Crystal
{
    public class SafeArea : MonoBehaviour
    {
        #region Simulations
        /// <summary>
        /// Simulation device that uses safe area due to a physical notch or software home bar. For use in Editor only.
        /// </summary>
        public enum SimDevice { None, iPhoneX }

        /// <summary>
        /// Simulation mode for use in editor only. This can be edited at runtime to toggle between different safe areas.
        /// </summary>
        public static SimDevice Sim = SimDevice.None;

        /// <summary>
        /// Normalised safe areas for iPhone X with Home indicator. Absolute values:
        ///  PortraitU x=0, y=102, w=1125, h=2202 on full extents w=1125, h=2436;
        ///  PortraitD x=0, y=102, w=1125, h=2202 on full extents w=1125, h=2436 (not supported, remains in Portrait Up);
        ///  LandscapeL x=132, y=63, w=2172, h=1062 on full extents w=2436, h=1125;
        ///  LandscapeR x=132, y=63, w=2172, h=1062 on full extents w=2436, h=1125.
        ///  Aspect Ratio: ~19.5:9.
        /// </summary>
        private Rect[] NSA_iPhoneX = {
            new Rect (0f, 102f / 2436f, 1f, 2202f / 2436f),  // Portrait
            new Rect (132f / 2436f, 63f / 1125f, 2172f / 2436f, 1062f / 1125f)  // Landscape
        };
        #endregion

        [Header("Components")]
        [SerializeField] private RectTransform _panel;

        [Header("EditorMode")]
        [SerializeField] bool _isRunInEditor = false;

        [Header("Options")]
        [SerializeField] bool _confirmXmin = true;
        [SerializeField] bool _confirmXmax = true;
        [SerializeField] bool _confirmYmin = true;       
        [SerializeField] bool _confirmYmax = true;

        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);

        private void Awake()
        {
            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        private void Refresh()
        {
            Rect safeArea = GetSafeArea();

            if (safeArea != _lastSafeArea)
            {
                ApplySafeArea(safeArea);
            }
        }

        private Rect GetSafeArea()
        {
            Rect safeArea = Screen.safeArea;

            if(_isRunInEditor)
            {                 
                Rect nsa = new Rect(0, 0, Screen.width, Screen.height);
                nsa = NSA_iPhoneX[0];

                safeArea = new Rect(Screen.width * nsa.x, Screen.height * nsa.y, Screen.width * nsa.width, Screen.height * nsa.height);
            }
            else
            {
                if (Application.isEditor && Sim != SimDevice.None)
                {
                    Rect nsa = new Rect(0, 0, Screen.width, Screen.height);

                    switch (Sim)
                    {
                        case SimDevice.iPhoneX:
                            if (Screen.height > Screen.width)
                                nsa = NSA_iPhoneX[0];
                            else
                                nsa = NSA_iPhoneX[1];
                            break;
                        default:
                            break;
                    }

                    safeArea = new Rect(Screen.width * nsa.x, Screen.height * nsa.y, Screen.width * nsa.width, Screen.height * nsa.height);
                }
            }
 
            return safeArea;
        }

        private void ApplySafeArea(Rect r)
        {           
            _lastSafeArea = r;

            Vector2 anchorMin = new Vector2(0,0);
            Vector2 anchorMax = new Vector2(Screen.width , Screen.height);

            if(_confirmYmax)
            {
                anchorMax.y = r.yMax;
            }

            if(_confirmYmin)
            {
                anchorMin.y = r.yMin;
            }

            if (_confirmXmax)
            {
                anchorMax.x = r.xMax;
            }

            if (_confirmXmin)
            {
                anchorMin.x = r.xMin;
            }
            
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _panel.anchorMin = anchorMin;
            _panel.anchorMax = anchorMax;
        }
    }
}