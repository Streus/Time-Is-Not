using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject[] presets;
    static int currentPreset;

    private void Awake()
    {
        currentPreset = SaveManager.controlPreset;
        UpdatePresetImage();
    }

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

    public void ChangePreset(bool rightButton)
    {
        if(rightButton)
        {
            currentPreset++;
            if(currentPreset >= presets.Length)
            {
                currentPreset = 0;
            }
        }
        else
        {
            currentPreset--;
            if (currentPreset < 0)
            {
                currentPreset = presets.Length - 1;
            }
        }
    }

    public void UpdatePresetImage()
    {
        for (int i = 0; i < presets.Length; i++)
        {
            if (i == currentPreset)
            {
                presets[i].SetActive(true);
            }
            else
            {
                presets[i].SetActive(false);
            }
        }
    }

    public void SavePreset()
    {
        SaveManager.controlPreset = currentPreset;
        SaveManager.Save(false);
    }
}
