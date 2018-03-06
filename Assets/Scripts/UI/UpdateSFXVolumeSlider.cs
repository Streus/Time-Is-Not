using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSFXVolumeSlider : MonoBehaviour
{
    Slider sfxSlider;

    // Use this for initialization
    void Start()
    {
        sfxSlider = this.GetComponent<Slider>();
        sfxSlider.value = (int)(UIManager.inst.GetSFXVolume());
    }
}
