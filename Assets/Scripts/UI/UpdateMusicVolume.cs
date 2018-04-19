using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMusicVolume : MonoBehaviour
{

    Slider musicVolumeSlider;
    Text musicVolumeText;

    // Use this for initialization
    void Start()
    {
        musicVolumeSlider = this.GetComponentInChildren<Slider>();
        musicVolumeText = this.GetComponentInChildren<Text>();
        musicVolumeSlider.value = (int)(UIManager.inst.GetMusicVolume());
    }

    // Update is called once per frame
    void Update()
    {
        if (musicVolumeText != null)
        {
            musicVolumeText.text = "Music Volume: " + (int)(((UIManager.inst.GetMusicVolume() - -50) / (0 - -50)) * 100);
        }
        else
        {
            //Debug.Log("No sfxText available");
        }
    }
}
