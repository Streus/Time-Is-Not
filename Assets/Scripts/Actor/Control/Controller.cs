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

	protected object stateData;
	#endregion

	#region INSTANCE_METHODS

	public void Awake()
	{
		self = GetComponent<Entity> ();
		anim = GetComponent<Animator> ();
		physbody = GetComponent<Rigidbody2D> ();
	}

	public void Update()
	{
		if (state != null)
			state.update (this);
	}

	public void FixedUpdate()
	{

	}

	public void setState(State s)
	{
		state.exit (this);
		state = s;
		state.enter (this);
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
