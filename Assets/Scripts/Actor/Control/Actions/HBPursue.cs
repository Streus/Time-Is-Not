using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Hummingbird/HBPursue")]
public class HBPursue : Action
{
	public float killDistance = 0.6f;

	public override void perform (Controller c)
	{
		Hummingbird bird = State.cast<Hummingbird> (c);

		bird.facePoint (bird.getPursuitTarget().position, bird.getTurnSpeed() * Time.deltaTime);

		float moveDist = bird.getSelf ().getMovespeed () * Time.deltaTime;
		bird.transform.Translate (
			(bird.getPursuitTarget().position - bird.transform.position).normalized *
			moveDist, Space.World);

		if (Vector3.Distance (bird.transform.position, bird.getPursuitTarget ().position) < killDistance)
			bird.getPursuitTarget ().GetComponent<Entity> ().onDeath ();
	}
}
