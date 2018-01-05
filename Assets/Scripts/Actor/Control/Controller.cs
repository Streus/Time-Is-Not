using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour //,IReapable TODO uncomment thsi when IReapable is implemented
{
	#region INSTANCE_VARS
	[Tooltip("Allow the data from this Controller to be reset by a level state reload?")]
	[SerializeField]
	private bool allowReset = true;

	[Tooltip("The current state this controller is using in its state machine.")]
	[SerializeField]
	private State state;

	#endregion

	#region INSTANCE_METHODS
	public void Awake()
	{

	}

	public void Update()
	{

	}

	public void FixedUpdate()
	{

	}

	public void setState(State s)
	{

	}

	//TODO IReapable integration for Controller
	//public SeedBase reap()
	//public void sow(SeedBase s)
	//public bool ignoreReset()
	#endregion

	/*
	private class Seed : SeedBase
	{

	}
	*/
}
