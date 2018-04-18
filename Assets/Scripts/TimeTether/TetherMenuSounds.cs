using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherMenuSounds : MonoBehaviour 
{

	public void PlayTetherMenuInSound()
	{
		Debug.Log("Play tether menu in sound");
        if(!GlobalAudio.ClipIsPlaying(AudioLibrary.inst.tetherMenuOpen))
        {
            Debug.Log("Playing Open");
            AudioLibrary.PlayTetherMenuOpen();
        }
        if(GlobalAudio.ClipIsPlaying(AudioLibrary.inst.tetherMenuClose))
        {
            Debug.Log("Stopping Close");
            GlobalAudio.StopPlayingClip(AudioLibrary.inst.tetherMenuClose);
        }
    }

	public void PlayTetherMenuOutSound()
	{
		Debug.Log("Play tether menu out sound");
        if (!GlobalAudio.ClipIsPlaying(AudioLibrary.inst.tetherMenuClose))
        {
            Debug.Log("Playing Close");
            AudioLibrary.PlayTetherMenuClose();
        }
        if (GlobalAudio.ClipIsPlaying(AudioLibrary.inst.tetherMenuOpen))
        {
            Debug.Log("Stopping Open");
            GlobalAudio.StopPlayingClip(AudioLibrary.inst.tetherMenuOpen);
        }
    }
}
