using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/HermitCrab/Sitting")]
public class HCSitting : Fork
{
	public override bool check (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);

		if (hc.GetComponent<PushBlock> ()._beingPushed)
		{
			hc.setWasPushed (true);
			hc.resetSitDuration ();
			return false;
		}
		else
			return hc.updateSitDuration (Time.deltaTime) && hc.getStands();
	}
}
