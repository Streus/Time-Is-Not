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

	#endregion

	#region INSTANCE_METHODS

	public override void Awake()
	{
		//NOTE: use protected anim and physbody from Controller

		base.Awake ();

		defaultState = getState();
		if (patrolStart == null && getState().name == "HBPatrol")
		{
			Debug.LogError ("A patrol path must be defined for the patrol state!" +
				"\n" + gameObject.name + " does not have a defined patrol path.");
		}
	}


	public override void Update()
	{
		//calculate sprite direction
//		anim = transform.GetChild(1).GetComponent<Animator>();
		Debug.Log (anim.gameObject.name);
		if(transform.eulerAngles.z > 315 || transform.eulerAngles.z < 45)
		{
			Debug.Log ("up");
			anim.SetInteger ("Direction", 1);
		}
		if(transform.eulerAngles.z > 45 && transform.eulerAngles.z < 135)
		{
			Debug.Log ("left");
			anim.SetInteger ("Direction", 4);
		}
		if(transform.eulerAngles.z > 135 && transform.eulerAngles.z < 225)
		{
			Debug.Log ("down");
			anim.SetInteger ("Direction", 3);
		}
		if(transform.eulerAngles.z > 225 && transform.eulerAngles.z < 315)
		{
			Debug.Log ("right");
			anim.SetInteger ("Direction", 2);
		}
		//TODO: check if hummingbird is moving
		anim.SetBool("isMoving", true);
		//TODO: check if hummingbird is attacking
		base.Update();
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
