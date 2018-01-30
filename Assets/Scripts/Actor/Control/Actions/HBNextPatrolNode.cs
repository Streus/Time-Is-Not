using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Hummingbird/HBNextPatrolNode")]
public class HBNextPatrolNode : Action
{
	public override void perform (Controller c)
	{
		Hummingbird bird = State.cast<Hummingbird> (c);

		bird.nextPatrolNode ();
	}
}
