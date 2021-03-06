﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Patroller/Patrol")]
public class HBPatrol : Action
{
	[Tooltip("How close the patroller will get to facing the next patrol point before moving to it")]
	public float turnTolerance = 0.1f;

	public float moveTolerance = 0.05f;

	public override void perform (Controller c)
	{
		PatrollingEnemy patroller = State.cast<PatrollingEnemy> (c);
		PatrolNode n = patroller.getPatrolTarget ();

		//can't do anything without a patrol target
		if (n == null)
			return;

		Vector3 navPos;
		if (c.getMap () != null)
		{
			if (Vector2.Distance (patroller.transform.position, n.transform.position) <= c.getMap().cellDimension)
			{
				navPos = n.transform.position;
			}
			else if (!c.currentPosition (out navPos))
			{
				c.setPath (n.transform.position);

				//throw out the first node in the path
				Vector3 trash;
				c.nextPosition (out trash);
			}
		}
		else
			navPos = n.transform.position;

		//turn to face move direction; wait until turning is done
		patroller.facePoint (navPos, patroller.getTurnSpeed () * Time.deltaTime);
		if (Vector3.Angle (patroller.transform.position - navPos, -patroller.transform.up) < turnTolerance)
		{
			float dist = Vector2.Distance (c.transform.position, navPos);
			float moveDist = c.getSelf ().getMovespeed () * Time.deltaTime;
			if (moveDist > dist)
				moveDist = dist;

			//move
			Vector2 dir = navPos - c.transform.position;
			RaycastHit2D hit = Physics2D.Raycast (c.transform.position, dir, moveDist * 20, ~0);
			Door d = null;
			if (hit.collider != null)
			{
				d = hit.collider.GetComponent<Door> ();
				if (d == null || d.isOpen ())
				{
					c.transform.Translate (
					dir.normalized *
					moveDist, Space.World);
				}
			}

		
			//if near the next point in the path, look ahead
			if (c.getMap() != null && dist < c.getMap ().cellDimension / 2f)
				c.nextPosition (out navPos);

//			float patrolTolerance = c.getMap () != null ? c.getMap ().cellDimension : moveTolerance;

			//if near the target patrol node, look to the next one
			if (Vector2.Distance (patroller.transform.position, n.transform.position) < moveTolerance)
			{
				patroller.nextPatrolNode ();
				if (c.getMap () != null && patroller.getPatrolTarget() != null)
				{
					c.setPath (patroller.getPatrolTarget ().transform.position);

					//throw out the first node in the path
					Vector3 trash;
					c.nextPosition (out trash);
				}
			}
		}
	}
}
