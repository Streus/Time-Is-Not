using System.Collections;
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
        if (masterVolumeText != null)
        {
            masterVolumeText.text = "Master Volume: " + (int)(((UIManager.inst.GetMasterVolume() - -50) / (0 - -50)) * 100);
        }
        else
        {
            //Debug.Log("No masterText available");
        }
    }
}
