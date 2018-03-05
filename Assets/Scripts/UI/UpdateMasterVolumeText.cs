using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMasterVolumeText : MonoBehaviour
{
    Text masterVolumeText;

    void Start()
    {
        masterVolumeText = this.GetComponent<Text>();    
    }

    void Update ()
    {
        masterVolumeText.text = "Master Volume: " + (int)(((UIManager.inst.GetMasterVolume() - -80)/(0 - -80)) * 100); 
	}
}
