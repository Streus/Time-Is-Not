using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherMenuSounds : MonoBehaviour 
{

	public void PlayTetherMenuInSound()
	{
		Debug.Log("Play tether menu in sound");
        if (GlobalAudio.ClipIsPlaying(AudioLibrary.inst.tetherMenuClose))
        {
            //GlobalAudio.StopPlayingClip(AudioLibrary.inst.tetherMenuClose);
            AudioLibrary.PlayTetherMenuOpen();
        }
        else
        {
            AudioLibrary.PlayTetherMenuOpen();
        }
	}

	public void PlayTetherMenuOutSound()
	{
		Debug.Log("Play tether menu out sound");
        if (GlobalAudio.ClipIsPlaying(AudioLibrary.inst.tetherMenuOpen))
        {
            //GlobalAudio.StopPlayingClip(AudioLibrary.inst.tetherMenuOpen);
            AudioLibrary.PlayTetherMenuClose();
        }
        else
        {
            AudioLibrary.PlayTetherMenuClose();
        }
    }
}
