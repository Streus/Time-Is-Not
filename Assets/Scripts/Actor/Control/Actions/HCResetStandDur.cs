using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/HermitCrab/ResetStandDuration")]
public class HCResetStandDur : Action
{
	public override void perform (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);

		hc.GetComponent<PushBlock> ().disableMovement ();
		hc.resetStandDuration ();
	}
}
