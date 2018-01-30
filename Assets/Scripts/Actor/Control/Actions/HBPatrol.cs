using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Hummingbird/HBPatrol")]
public class HBPatrol : Action
{
	public override void perform (Controller c)
	{
		Hummingbird bird = State.cast<Hummingbird> (c);
			
		PatrolNode n = bird.getPatrolTarget ();

		float moveDist = bird.getSelf ().getMovespeed () * Time.deltaTime;
		bird.transform.Translate (
			(n.transform.position - bird.transform.position).normalized *
			moveDist, Space.World);
	}
}
