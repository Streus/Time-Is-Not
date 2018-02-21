using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Has been merged into the HB Patrol Action")]
public class HBRotate : Action 
{
	public override void perform (Controller c)
	{
		Hummingbird bird = State.cast<Hummingbird> (c);

		PatrolNode n = bird.getPatrolTarget ();
		bird.facePoint (n.transform.position, bird.getTurnSpeed() * Time.deltaTime);
	}
}
