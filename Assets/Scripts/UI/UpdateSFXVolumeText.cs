using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSFXVolumeText : MonoBehaviour {

    Text sfxVolumeText;

    void Start()
    {
        sfxVolumeText = this.GetComponent<Text>();
    }

    void Update()
    {
        sfxVolumeText.text = "SFX Volume: " + (int)(((UIManager.inst.GetSFXVolume() - -80) / (0 - -80)) * 100);
    }
}
