using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/HermitCrab/Returning")]
public class HCReturning : Action
{
	public override void perform (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);

		if (hc.updateReturnTimer (Time.deltaTime))
		{
			float totalDist = Vector2.Distance (c.transform.position, hc.getHome ());
			bool nearHome = totalDist < c.getMap ().cellDimension / 2f;

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
			c.transform.Translate (
				(navPos - c.transform.position).normalized *
				moveDist, Space.World);

			//if near the next point in the path, look ahead
			if (c.getMap () != null && dist < c.getMap ().cellDimension / 2f)
				c.nextPosition (out navPos);
		}
	}
}
