using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/HermitCrab/ResetSitDuration")]
public class HCResetSitDur : Action
{
	public override void perform (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);

		hc.GetComponent<PushBlock> ().enableMovement ();
		hc.resetSitDuration ();
	}
}