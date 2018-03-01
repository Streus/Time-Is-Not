using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempSelectorScene : MonoBehaviour
{
    public void LoadScene(string name)
    {
        if (SceneManager.GetSceneByName(name) != null)
        {
            SceneManager.LoadScene(name);
        }
        else
        {
            Debug.LogError("Scene name " + name + " cannot be loaded");
        }
    }

    public void Load()
    {
        SaveManager.Load();
        SceneManager.LoadScene(SaveManager.level);
    }

    public void NewGame()
    {
        SaveManager.NewGame();
        SceneManager.LoadScene(SaveManager.level);
    }
}
