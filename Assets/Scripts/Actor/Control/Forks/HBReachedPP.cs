using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Has been merged into the HB Patrol Action")]
public class HBReachedPP : Fork
{
	public override bool check (Controller c)
	{
		PatrollingEnemy p = State.cast<PatrollingEnemy> (c);

		PatrolNode n = p.getPatrolTarget ();
		float dist = Vector2.Distance (c.transform.position, n.transform.position);
		float moveDist = c.getSelf ().getMovespeed () * Time.deltaTime;

		return dist < moveDist;
	}
}
