using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherMenuSounds : MonoBehaviour 
{

	public void PlayTetherMenuInSound()
	{
		Debug.Log("Play tether menu in sound");
        AudioLibrary.PlayTetherMenuOpen();
        //GlobalAudio.StopPlayingClip(AudioLibrary.inst.tetherMenuClose);
    }

	public void PlayTetherMenuOutSound()
	{
		Debug.Log("Play tether menu out sound");
        AudioLibrary.PlayTetherMenuClose();
        //GlobalAudio.StopPlayingClip(AudioLibrary.inst.tetherMenuOpen);
    }
}
