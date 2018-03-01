using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class SaveManager : Singleton<SaveManager>
{
    //-------- Important for JavaScript to Access Data ---------------------------------------------------------------------//
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);

    //public static SaveManager saveManager;
    static string saveLocation;
    static string m_level; 
    public static string level
    {
        get
        {
            return m_level;
        }
        set
        {
            m_level = value;
        }
    }

    private void Awake()
    {
        /*if(saveManager == null)
        {
            DontDestroyOnLoad(gameObject);
            saveManager = this;
        }*/
        saveLocation = string.Format("{0}/SavedData.dat", Application.persistentDataPath);
        if (File.Exists(saveLocation))
        {
            Load();
        }
    }

    void Update()
    {
        
    }

    public static void Save()
    {
        print("SAVINGDATANOW");
        
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;

        try
        {
            if (File.Exists(saveLocation))
            {
                File.WriteAllText(saveLocation, string.Empty);
                fileStream = File.Open(saveLocation, FileMode.Open);
                print("File exists and saved");
            }
            else
            {
                fileStream = File.Create(saveLocation);
                print("Creating file");
            }

            VariablesToSave savedData = new VariablesToSave();

            savedData.m_level = SaveManager.m_level;//saveManager.m_level;

            binaryFormatter.Serialize(fileStream, savedData);
            fileStream.Close();

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                SyncFiles();
            }
        }
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to Save: " + e.Message);
        }
    }

    public static void Load()
    {
        print("LOADINGDATANOW");
        
        saveLocation = string.Format("{0}/SavedData.dat", Application.persistentDataPath);

        try
        {
            if (File.Exists(saveLocation))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = File.Open(saveLocation, FileMode.Open);

                VariablesToSave savedData = (VariablesToSave)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();

                //saveManager.m_level = savedData.m_level;
                SaveManager.m_level = savedData.m_level;
            }
        }
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to Load: " + e.Message);
        }
    }

    private static void PlatformSafeMessage(string message)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            WindowAlert(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public static void NewGame()
    {
        ClearData();
    }

    public static void ClearData()
    {
        print("Clear Data");
        SaveManager.level = "Vertical Slice";//saveManager.level = "Vertical Slice";
        print("Data Cleared");
        Save();
        Load();
    }

    public void OnApplicationQuit()
    {
        Save();
    }

    [Serializable]
    public class VariablesToSave
    {
        public string m_level;
    }

}
