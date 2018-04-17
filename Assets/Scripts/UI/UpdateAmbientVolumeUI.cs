using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAmbientVolumeUI : MonoBehaviour
{
    Slider ambientVolumeSlider;
    Text ambientVolumeText;

	// Use this for initialization
	void Start ()
    {
        ambientVolumeSlider = this.GetComponentInChildren<Slider>();
        ambientVolumeText = this.GetComponentInChildren<Text>();
        ambientVolumeSlider.value = (int)(UIManager.inst.GetAmbientVolume());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (ambientVolumeText != null)
        {
            ambientVolumeText.text = "Ambient Volume: " + (int)(((UIManager.inst.GetAmbientVolume() - -50) / (0 - -50)) * 100);
        }
        else
        {
            //Debug.Log("No ambientText available");
        }
    }
}
