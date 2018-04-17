using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/HermitCrab/ResetStandDuration")]
public class HCResetStandDur : Action
{
    AudioSource source;

	public override void perform (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);
        source = c.GetComponent<AudioSource>();
        if (source != null)
        {
            source.Stop();
            source.loop = false;
            source.PlayOneShot(AudioLibrary.inst.hermitCrabStand, 1);
            Debug.Log("Sit down sound");
        }
		hc.GetComponent<PushBlock> ().disableMovement ();
		hc.resetStandDuration ();
	}
}
