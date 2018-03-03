using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/HermitCrab/Returning")]
public class HCReturning : Action
{
	public override void perform (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);

		hc.doNullify ();

		if (hc.updateReturnTimer (Time.deltaTime))
		{
			float totalDist = Vector2.Distance (c.transform.position, hc.getHome ());
			bool nearHome = totalDist < c.getMap ().cellDimension;

			Vector3 navPos = hc.transform.position;
			if (c.getMap () != null)
			{
				if(nearHome)
				{
					navPos = hc.getHome ();
				}
				else if (!c.currentPosition (out navPos))
				{
					c.setPath (hc.getHome ());
				}

				if(!nearHome)
					c.currentPosition (out navPos);
			}
			else
				Debug.LogError (c.name + " needs an Atlas!");

			float dist = Vector2.Distance (c.transform.position, navPos);
			float moveDist = c.getSelf ().getMovespeed () * Time.deltaTime;
			if (moveDist > dist)
				moveDist = dist;

			//move
			Vector3 dir = navPos - c.transform.position;
			c.transform.Translate (dir.normalized * moveDist, Space.World);

			//determine facing sprite using dir (defined above)
			//TODO crab walking animation here
			if (dir != Vector3.zero)
			{
				Quaternion qdir = Quaternion.LookRotation (dir, Vector3.back);
				if (qdir.eulerAngles.z > 315 || qdir.eulerAngles.z < 45)
				{
					//up

				}
				if (qdir.eulerAngles.z > 45 && qdir.eulerAngles.z < 135)
				{
					//left

				}
				if (qdir.eulerAngles.z > 135 && qdir.eulerAngles.z < 225)
				{
					//down

				}
				if (qdir.eulerAngles.z > 225 && qdir.eulerAngles.z < 315)
				{
					//right

				}
			}

			//if near the next point in the path, look ahead
			if (c.getMap () != null && dist < c.getMap ().cellDimension / 1.5f)
				c.nextPosition (out navPos);
		}
	}
}
