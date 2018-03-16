﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMasterVolumeUI : MonoBehaviour
{
    Slider masterVolumeSlider;
    Text masterVolumeText;

	// Use this for initialization
	void Start ()
    {
        masterVolumeSlider = this.GetComponentInChildren<Slider>();
        masterVolumeText = this.GetComponentInChildren<Text>();
        masterVolumeSlider.value = (int)(UIManager.inst.GetMasterVolume());
    }
	
	// Update is called once per frame
	void Update ()
    {
        masterVolumeText.text = "Master Volume: " + (int)(((UIManager.inst.GetMasterVolume() - -80) / (0 - -80)) * 100);
    }
}