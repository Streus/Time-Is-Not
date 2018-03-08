using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/HermitCrab/ResetReturnTimer")]
public class HCResetReturnTimer : Action
{
	public override void perform (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);

		if (hc.getWasPushed ())
			hc.setReturnTimerOnStand ();
		else
			hc.resetReturnTimer ();
	}
}
