using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Hummingbird/VerifyRotation")]
public class VerifyRotation : Fork
{
	public float tolerance = 0.1f;

	public override bool check (Controller c)
	{
		Hummingbird bird = State.cast<Hummingbird> (c);

		PatrolNode p = bird.getPatrolTarget ();

		Debug.Log (Vector3.Angle (bird.transform.position - p.transform.position, bird.transform.up));
		return Vector3.Angle (bird.transform.position - p.transform.position, -bird.transform.up) < tolerance;
	}
}
