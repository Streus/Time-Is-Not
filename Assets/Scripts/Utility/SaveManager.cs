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

    static string saveLocation;

    //public string l;

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

    //public int cp;

    static int m_controlPreset;

    public static int controlPreset
    {
        get
        {
            return m_controlPreset;
        }
        set
        {
            m_controlPreset = value;
        }
    }

    void Awake()
    {
        saveLocation = string.Format("{0}/SavedData.dat", Application.persistentDataPath);

        if (File.Exists(saveLocation))
        {
            Load();
        }
    }

    /*private void Update()
    {
        cp = controlPreset;
        l = level;
    }*/

  /*  public static void Save()
    {
        print("SAVINGDATANOW");
        
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;

        if(saveLocation == null)
        {
            saveLocation = string.Format("{0}/SavedData.dat", Application.persistentDataPath);
        }

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

            savedData.m_level = SaveManager.m_level;
            savedData.m_controlPreset = SaveManager.m_controlPreset;

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
    }*/

    public static void Save(bool allData)
    {
        print("SAVINGDATANOW");

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;

        if (saveLocation == null)
        {
            saveLocation = string.Format("{0}/SavedData.dat", Application.persistentDataPath);
        }

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

            if (allData)
            {
                savedData.m_level = SaveManager.m_level;
                savedData.m_controlPreset = SaveManager.m_controlPreset;
            }
            else
            {
                savedData.m_controlPreset = SaveManager.m_controlPreset;
            }

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

        if (saveLocation == null)
        {
            saveLocation = string.Format("{0}/SavedData.dat", Application.persistentDataPath);
        }

        try
        {
            if (File.Exists(saveLocation))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = File.Open(saveLocation, FileMode.Open);

                VariablesToSave savedData = (VariablesToSave)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();

                SaveManager.m_level = savedData.m_level;
                SaveManager.m_controlPreset = savedData.m_controlPreset;
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
        SaveManager.level = "Vertical Slice";
        SaveManager.controlPreset = 0;
        Save(true);
        Load();
    }

    public static bool HasData()
    {
        if(File.Exists(saveLocation))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnApplicationQuit()
    {
        Save(true);
    }

    [Serializable]
    public class VariablesToSave
    {
        public string m_level;
        public int m_controlPreset;
    }

}
