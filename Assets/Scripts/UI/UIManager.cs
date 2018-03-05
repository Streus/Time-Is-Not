using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject[] presets;
    static int currentPreset;

    [SerializeField] AudioMixer mainMixer;

    //Sets the preset in the main menu
    private void Awake()
    {
        currentPreset = SaveManager.controlPreset;
        UpdatePresetImage();
    }

    //Loads the saved data and loads the scene and presets 
    public void LoadGame()
    {
        SaveManager.Load();
        SceneManager.LoadScene(SaveManager.level);
    }

    //Resets the saved data and loads the start of the game
    public void NewGame()
    {
        SaveManager.NewGame();
        SceneManager.LoadScene(SaveManager.level);
    }

    //Activates the assigned panel/ gameobject
    public void OpenPanel(GameObject panelToOpen)
    {
        panelToOpen.SetActive(true);
    }

    //Deactives the assigned panel/ gameobject
    public void ClosePanel(GameObject panelToClose)
    {
        panelToClose.SetActive(false);
    }

    //Increments/Decrements the current preset
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

    //Updates the active preset panel to current one
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

    //Saves the current preset to the SaveManager
    public void SavePreset()
    {
        SaveManager.controlPreset = currentPreset;
        SaveManager.Save();
    }

    public void SetMasterVolume(float masterVolLevel)
    {
        mainMixer.SetFloat("masterVolume", masterVolLevel);
    }

    public void SetSFXVolume(float sfxVolumeLevel)
    {
        mainMixer.SetFloat("sfxVolume", sfxVolumeLevel);
    }

    public float GetMasterVolume()
    {
        float value;

        bool result = mainMixer.GetFloat("masterVolume", out value);
        if(result)
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }

    public float GetSFXVolume()
    {
        float value;

        bool result = mainMixer.GetFloat("sfxVolume", out value);
        if (result)
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }
}
