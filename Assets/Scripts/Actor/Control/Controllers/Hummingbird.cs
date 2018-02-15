using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hummingbird : Controller
{
	#region INSTANCE_VARS
	[SerializeField]
	private PatrolNode patrolStart;

	[SerializeField]
	[Range(1f, 360f)]
	private float turnSpeed = 90f;

	[SerializeField]
	[Range(1f, 100f)]
	private float sightRange = 5f;

	[SerializeField]
	[Range(1f, 360f)]
	private float fieldOfView = 90f;

	private State defaultState;

	private Transform pursuitTarget;

	private Animator _anim;
	private Rigidbody2D _rigidBody;

	#endregion

	#region INSTANCE_METHODS

	public override void Awake()
	{
		_anim = GetComponent<Animator> ();
		_rigidBody = GetComponent<Rigidbody2D> ();

		base.Awake ();

		defaultState = getState();
		if (patrolStart == null && getState().name == "HBPatrol")
		{
			Debug.LogError ("A patrol path must be defined for the patrol state!" +
				"\n" + gameObject.name + " does not have a defined patrol path.");
		}
	}

	#region GETTERS_SETTERS
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

	public float getSightRange()
	{
		return sightRange;
	}

	public float getFOV()
	{
		return fieldOfView;
	}

	public Transform getPursuitTarget()
	{
		return pursuitTarget;
	}

	public void setPursuitTarget(Transform t)
	{
		pursuitTarget = t;
	}
	#endregion

	#region ISAVABLE_METHODS

	public override SeedBase saveData ()
	{
		HSeed s = new HSeed (this);
		return s;
	}

	public override void loadData (SeedBase seed)
	{
		base.loadData (seed);
		HSeed h = (HSeed)seed;
		patrolStart = h.currentNode;
		defaultState = h.defaultState;
		pursuitTarget = h.pursuitTarget;
	}
	#endregion

	public override void OnDrawGizmos()
	{
		Gizmos.color = getState().color;

		Vector3 fwd = transform.up;
		Vector3 rightBound = (Quaternion.Euler (0, 0, fieldOfView/2f) * fwd);
		Vector3 leftBound = (Quaternion.Euler (0, 0, -fieldOfView/2f) * fwd);

		Gizmos.DrawLine (transform.position, (transform.up * sightRange) + transform.position);
		Gizmos.DrawLine (transform.position, (rightBound.normalized * sightRange) + transform.position);
		Gizmos.DrawLine (transform.position, (leftBound.normalized * sightRange) + transform.position);
	}
	#endregion

	#region INTERNAL_TYPES

	private class HSeed : Seed
	{
		public PatrolNode currentNode;
		public State defaultState;
		public Transform pursuitTarget;

		public HSeed(Controller c) : base(c)
		{
			Hummingbird h = State.cast<Hummingbird>(c);
			currentNode = h.patrolStart;
			defaultState = h.defaultState;
			pursuitTarget = h.pursuitTarget;
		}
	}
	#endregion
}
