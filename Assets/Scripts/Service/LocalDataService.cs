using Assets.Scripts.Data;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Service
{
    public class LocalDataService : MonoBehaviour
    {
        public delegate void GotDataFromFile(PlayerData currentData);
        public GotDataFromFile onDataLoaded;

        public delegate void ScreenShootLoaded(Texture2D screenShoot);
        public ScreenShootLoaded onScreenShootLoaded;

        private PlayerData _playerData = new PlayerData();

        private const string _FILE_NAME = "StackData.bin";
        private const string _SHCREENSHOOT_NAME = "back.png";
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
            LoadScreenShoot();
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
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(_filePath, FileMode.Open, FileAccess.ReadWrite);
                    _playerData = (PlayerData)bf.Deserialize(file);

                    file.Close();                    
                    ShareLocalData();
                    return;
                }
                else
                {                    
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
            ScreenCapture.CaptureScreenshot(_screenShootPath);             
            Invoke("LoadScreenShoot", 0.2f); 
        }

        public void LoadScreenShoot()
        {
            Texture2D newTex = LoadPNG(_screenShootPath);
            onScreenShootLoaded.Invoke(newTex);            
        } 

        public Texture2D LoadPNG(string filePath)
        {
            Texture2D tex = null;
            byte[] fileData;
             
            if (File.Exists(filePath))
            {                 
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(1080, 1920);
                tex.LoadImage(fileData); 
            }            

            return tex;       
        }

        private void SetDefaultData()
        {
            _playerData.playerScore = 0;            
            SaveProgress();           
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
            onDataLoaded.Invoke(_playerData);
        }
    }
}