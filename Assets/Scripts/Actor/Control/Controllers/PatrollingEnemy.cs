using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Controller
{
	#region INSTANCE_VARS

	[Header("Patrolling")]

	[SerializeField]
	protected PatrolNode patrolStart;

	[SerializeField]
	[Range(1f, 360f)]
	protected float turnSpeed = 90f;
	#endregion

	#region INSTANCE_METHODS

	public PatrolNode getPatrolTarget()
	{
		return patrolStart;
	}

	public PatrolNode nextPatrolNode()
	{
		return patrolStart = patrolStart.getNext();
	}

	public float getTurnSpeed()
	{
		return turnSpeed;
	}

	public override void OnDrawGizmos ()
	{
		Vector3 navPos;
		if (currentPosition (out navPos))
		{
			Color c = getMap ().graphColor;
			Gizmos.color = new Color (1.0f - c.r, 1.0f - c.g, 1.0f - c.b);
			Gizmos.DrawLine (transform.position, navPos);
		}
	}
	#endregion
}
