using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void LoadGame()
    {
        SaveManager.Load();
        SceneManager.LoadScene(SaveManager.level);
    }

    public void NewGame()
    {
        SaveManager.NewGame();
        SceneManager.LoadScene(SaveManager.level);
    }

    public void OpenPanel(GameObject panelToOpen)
    {
        panelToOpen.SetActive(true);
    }

    public void ClosePanel(GameObject panelToClose)
    {
        panelToClose.SetActive(false);
    }
}
