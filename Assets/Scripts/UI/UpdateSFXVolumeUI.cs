using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSFXVolumeUI : MonoBehaviour
{
    Slider sfxVolumeSlider;
    Text sfxVolumeText;

	// Use this for initialization
	void Start ()
    {
        sfxVolumeSlider = this.GetComponentInChildren<Slider>();
        sfxVolumeText = this.GetComponentInChildren<Text>();
        sfxVolumeSlider.value = (int)(UIManager.inst.GetSFXVolume());
    }
	
	// Update is called once per frame
	void Update ()
    {
        sfxVolumeText.text = "SFX Volume: " + (int)(((UIManager.inst.GetSFXVolume() - -80) / (0 - -80)) * 100);
    }
}
