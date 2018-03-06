using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMasterVolumeSlider : MonoBehaviour
{

    Slider masterSlider;

	// Use this for initialization
	void Start ()
    {
        masterSlider = this.GetComponent<Slider>();
        masterSlider.value = (int)(UIManager.inst.GetMasterVolume());
    }
}
