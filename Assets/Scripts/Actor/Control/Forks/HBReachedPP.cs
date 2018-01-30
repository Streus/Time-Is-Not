using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Hummingbird/HBReachedPP")]
public class HBReachedPP : Fork
{
	public override bool check (Controller c)
	{
		Hummingbird bird = State.cast<Hummingbird> (c);

		PatrolNode n = bird.getPatrolTarget ();
		float dist = Vector2.Distance (bird.transform.position, n.transform.position);
		float moveDist = bird.getSelf ().getMovespeed () * Time.deltaTime;

		return dist < moveDist;
	}
}
