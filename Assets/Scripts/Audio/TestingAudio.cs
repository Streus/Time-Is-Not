using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingAudio : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.M))
        {
            AudioLibrary.PlayTetherPlacementSound();
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            AudioLibrary.PlayTetherRewindSound();
        }
	}

    public void UIClick()
    {
        AudioLibrary.PlayUIButtonClick();
    }
}
