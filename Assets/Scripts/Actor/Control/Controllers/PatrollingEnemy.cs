using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Controller
{
	#region INSTANCE_VARS

	[Header("Patrolling")]

	[SerializeField]
	protected PatrolNode patrolStart;
	private PatrolNode prevNode;

	[SerializeField]
	[Range(1f, 360f)]
	protected float turnSpeed = 90f;
	#endregion

	#region INSTANCE_METHODS

	public PatrolNode getPrevNode()
	{
		return prevNode;
	}

	public PatrolNode getPatrolTarget()
	{
		return patrolStart;
	}

	public PatrolNode nextPatrolNode()
	{
		if (patrolStart != null)
		{
			prevNode = patrolStart;
			return patrolStart = patrolStart.getNext ();
		}
		return null;
	}

	public float getTurnSpeed()
	{
		return turnSpeed;
	}

	public override void OnDrawGizmos ()
	{
		Vector3 navPos;
		if (getMap() != null)
		{
			Color c = getMap ().graphColor;
			Gizmos.color = new Color (1.0f - c.r, 1.0f - c.g, 1.0f - c.b);

			if(getPatrolTarget() != null)
				Gizmos.DrawLine (transform.position, getPatrolTarget().transform.position);

			if(getPrevNode() != null)
				Gizmos.DrawLine (transform.position, getPrevNode ().transform.position);
		}
	}
	#endregion
}
