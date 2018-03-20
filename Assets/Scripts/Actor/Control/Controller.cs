﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour, ISavable
{
	#region INSTANCE_VARS

	[Tooltip("The current state this controller is using in its state machine.")]
	[SerializeField]
	private State state;

	[Tooltip("The graph data this Actor will use to navigate the scene.")]
	[SerializeField]
	private Atlas map;

	protected Entity self;
	protected Animator anim;
	protected Rigidbody2D physbody;

	private Stack<Vector3> path;
	#endregion

	#region INSTANCE_METHODS

	public virtual void Awake()
	{
		self = GetComponent<Entity> ();
		anim = GetComponent<Animator> ();
		physbody = GetComponent<Rigidbody2D> ();

		path = null;

		if (state != null)
			state.enter (this);
	}
		
	public virtual void Update()
	{
		if (state != null && (GameManager.inst == null || !GameManager.isPaused()))
			state.update (this);
	}

	public virtual void FixedUpdate()
	{

	}

	public State getState()
	{
		return state;
	}

	public void setState(State s)
	{
		if (s == null)
		{
			Debug.LogError ("Attempted to transition to a null state from " + state.name + "!");
			return;
		}

		state.exit (this);
		state = s;
		state.enter (this);
	}
		
	public Entity getSelf()
	{
		return self;
	}

	public Atlas getMap()
	{
		return map;
	}

	public void setPath(Vector3 target)
	{
		if (map == null)
		{
			Debug.LogError (gameObject.name + " is trying to navigate without an Atlas!");
			return;
		}

		if (!map.findPath (transform.position, target, out path))
			Debug.LogError ("Could not find path to " + target.ToString () + ".");
	}

	public void discardPath()
	{
		path.Clear ();
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
		pos = path.Pop ();
		return true;
	}

	public void facePoint(Vector2 point, float maxDelta = 360f)
	{
		Quaternion rot = Quaternion.LookRotation (transform.position - new Vector3(point.x, point.y, -100f), Vector3.forward);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, maxDelta);
		transform.eulerAngles = new Vector3 (0f, 0f, transform.eulerAngles.z);
	}

	public void faceTarget(Transform target)
	{
		if (transform != null)
			facePoint (transform.position);
	}

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = state != null ? state.color : Color.white;
		Gizmos.DrawWireSphere (transform.position, 1f);
	}

	#region ISAVABLE_METHODS
	public virtual SeedBase saveData()
	{
		Seed s = new Seed (this);
		return s;
	}

	public virtual void loadData(SeedBase seed)
	{
		Seed s = (Seed)seed;
		state = s.state;
		path = s.path;
	}
	#endregion
	#endregion

	#region INTERNAL_TYPES

	protected class Seed : SeedBase
	{
		public State state;
		public Stack<Vector3> path;

		public Seed(Controller c)
		{
			state = c.state;
			path = c.path;
		}
	}
	#endregion
}
