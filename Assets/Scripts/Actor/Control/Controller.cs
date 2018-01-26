using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour //,IReapable TODO uncomment this when IReapable is implemented
{
	#region INSTANCE_VARS

	[Tooltip("Allow the data from this Controller to be reset by a level state reload?")]
	[SerializeField]
	private bool allowReset = true;

	[Tooltip("The current state this controller is using in its state machine.")]
	[SerializeField]
	private State state;

	protected Entity self;
	protected Animator anim;
	protected Rigidbody2D physbody;

	private Queue<Vector3> path;
	#endregion

	#region INSTANCE_METHODS

	public void Awake()
	{
		self = GetComponent<Entity> ();
		anim = GetComponent<Animator> ();
		physbody = GetComponent<Rigidbody2D> ();

		path = null;
	}
		
	public virtual void Update()
	{
		if (state != null)
			state.update (this);
	}

	public virtual void FixedUpdate()
	{

	}

	public void setState(State s)
	{
		state.exit (this);
		state = s;
		state.enter (this);
	}
		
	public Entity getSelf()
	{
		return self;
	}

	public void setPath(Vector3 target)
	{
		if (!Atlas.instance.findPath (transform.position, target, out path))
			Debug.LogError ("Could not find path to " + target.ToString () + ".");
	}

	public bool currentPosition(out Vector3 pos)
	{
		pos = Vector3.zero;
		if (path == null || path.Count <= 0)
			return false;
		pos = path.Peek ();
		return true;
	}

	public bool nextPosition(out Vector3 pos)
	{
		pos = Vector3.zero;
		if (path == null || path.Count <= 0)
			return false;
		pos = path.Dequeue ();
		return true;
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = state != null ? state.color : Color.white;
		Gizmos.DrawWireSphere (transform.position, 1f);
	}

	//TODO IReapable integration for Controller
	//public SeedBase reap()
	//public void sow(SeedBase s)
	//public bool ignoreReset()
	#endregion

	#region INTERNAL_TYPES
	/*
	private class Seed : SeedBase
	{

	}
	*/
	#endregion
}
