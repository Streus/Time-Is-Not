using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanLoadButton : MonoBehaviour 
{
    public int state;
	
	// Update is called once per frame
	void Update () {
		if (LevelStateManager.canLoadTetherPoint(state) && GameManager.isPaused())
        {
            this.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
	}
}
