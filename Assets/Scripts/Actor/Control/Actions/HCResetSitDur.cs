using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/HermitCrab/ResetSitDuration")]
public class HCResetSitDur : Action
{
    AudioSource source;
	public override void perform (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);
        source = c.GetComponent<AudioSource>();
        source.Stop();
        source.PlayOneShot(AudioLibrary.inst.hermitCrabStand);
        hc.GetComponent<PushBlock> ().enableMovement ();
		hc.resetSitDuration ();
	}
}