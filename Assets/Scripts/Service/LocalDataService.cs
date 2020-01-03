using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Service
{
    public class LocalDataService : MonoBehaviour
    {
        public delegate void GotDataFromFile(PlayerData currentData);
        public GotDataFromFile onDataLoaded;

        public delegate void ScreenShootLoaded(Sprite screenShoot);
        public ScreenShootLoaded onScreenShootLoaded;

        [SerializeField] private Sprite _defaultScreenSprite;


        private PlayerData _playerData = new PlayerData();

        private const string _FILE_NAME = "StackData.bin";
        private const string _SHCREENSHOOT_NAME = "Back.png";
        private string _filePath;
        private string _screenShootPath;

        private Sprite _actualScreenShoot;

        private void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            _filePath = Application.persistentDataPath + "/" + _FILE_NAME;
            _screenShootPath = Application.persistentDataPath + "/" + _SHCREENSHOOT_NAME;
            
            LoadProgress();
        }

        public void SetNewDataAndSave(PlayerData newData)
        {
            _playerData = newData;
            SaveProgress();
        }
  
        private void LoadProgress()
        {
            _playerData  = new PlayerData();          

            try
            {
                if (File.Exists(_filePath))
                {
                    Debug.Log("File Exists");
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(_filePath, FileMode.Open, FileAccess.ReadWrite);
                    _playerData = (PlayerData)bf.Deserialize(file);

                    file.Close();
                    LoadScreenShoot();
                    ShareLocalData();
                    return;
                }
                else
                {
                    Debug.Log("File Not Exists");
                    SetDefaultData();
                }
            }
            catch (Exception exception)
            {
                File.Delete(_filePath);
                SetDefaultData();
                Debug.LogError("Local File Deleted! Exception: " + exception);
            }
        }

        public void UpdateScreenShoot()
        {
            ScreenCapture.CaptureScreenshot(_screenShootPath, 2);
            LoadScreenShoot();
        }

        public void LoadScreenShoot()
        {
            Sprite newSprite = LoadPNG(_screenShootPath);
            onScreenShootLoaded.Invoke(newSprite);            
        }

        public Sprite LoadPNG(string filePath)
        {
            Texture2D tex = null;
            byte[] fileData;

            Sprite newSprite;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(500 , 500);
                tex.LoadImage(fileData);

                newSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                newSprite = _defaultScreenSprite;
            }

            return newSprite;       
        }

        private void SetDefaultData()
        {
            _playerData.playerScore = 0;            
            SaveProgress();
            onScreenShootLoaded.Invoke(_defaultScreenSprite);
            ShareLocalData();
        }

        private void SaveProgress()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(_filePath);
            bf.Serialize(file, _playerData);
            file.Close();
        }

        private void ShareLocalData()
        {
            Debug.Log("ShareLocalData");
            onDataLoaded.Invoke(_playerData);
        }
    }
}
