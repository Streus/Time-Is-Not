using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/HermitCrab/Standing")]
public class HCStanding : Fork
{
	public override bool check (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);

		return hc.updateStandDuration (Time.deltaTime);
	}
}
