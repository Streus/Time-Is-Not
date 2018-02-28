using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GulperEel : PatrollingEnemy
{
	#region INSTANCE_METHODS

	public override void Update ()
	{
		base.Update ();

		//TODO gulper eel movement animations
		if(transform.eulerAngles.z > 315 || transform.eulerAngles.z < 45)
		{
			//up

		}
		if(transform.eulerAngles.z > 45 && transform.eulerAngles.z < 135)
		{
			//left

		}
		if(transform.eulerAngles.z > 135 && transform.eulerAngles.z < 225)
		{
			//down

		}
		if(transform.eulerAngles.z > 225 && transform.eulerAngles.z < 315)
		{
			//right

		}
	}

	#region ISAVABLE_METHODS

	public override SeedBase saveData ()
	{
		GSeed s = new GSeed (this);
		return s;
	}

	public override void loadData (SeedBase seed)
	{
		base.loadData (seed);
		GSeed h = (GSeed)seed;
		patrolStart = h.currentNode;
	}
	#endregion
	#endregion

	#region INTERNAL_TYPES

	private class GSeed : Seed
	{
		public PatrolNode currentNode;

		public GSeed(Controller c) : base(c)
		{
			GulperEel g = State.cast<GulperEel>(c);
			currentNode = g.patrolStart;
		}
	}
	#endregion
}
