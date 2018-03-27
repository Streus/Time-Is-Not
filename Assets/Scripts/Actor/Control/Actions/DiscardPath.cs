using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/General/DiscardPath")]
public class DiscardPath : Action
{
    AudioSource source;
	public override void perform (Controller c)
	{
        
        source = c.GetComponent<AudioSource>();
        if (source != null)
        {
            source.clip = AudioLibrary.inst.hummingBirdMoving;
            source.loop = true;
            source.Play();
        }
        c.discardPath();
    }
}
