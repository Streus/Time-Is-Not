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
        ambientVolumeText.text = "Ambient Volume: " + (int)(((UIManager.inst.GetAmbientVolume() - -80) / (0 - -80)) * 100);
    }
}
