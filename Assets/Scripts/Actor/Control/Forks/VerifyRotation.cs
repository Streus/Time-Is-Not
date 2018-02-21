using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Has been merged into the HB Patrol Action")]
public class VerifyRotation : Fork
{
	public float tolerance = 0.1f;

	public override bool check (Controller c)
	{
		Hummingbird bird = State.cast<Hummingbird> (c);

		PatrolNode p = bird.getPatrolTarget ();

		return Vector3.Angle (bird.transform.position - p.transform.position, -bird.transform.up) < tolerance;
	}
}
