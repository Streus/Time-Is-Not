using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller
{
	#region INSTANCE_VARS
	[SerializeField]
	private State pushState;

	private State prePushState;
	#endregion

	#region INSTANCE_METHODS
	public void Start ()
	{
		getSelf ().addAbility (Ability.get ("Place Stasis"));
        getSelf().addAbility(Ability.get("Dash"));
	}

	public override void FixedUpdate ()
	{
		base.FixedUpdate ();
	}

	public void enterPushState()
	{
		prePushState = getState();
		setState (pushState);
	}

	public void exitPushState()
	{
		setState (prePushState);
	}

	public bool pushing()
	{
		return (pushState == getState ());
	}

	#region ISAVABLE_METHODS
	/*
	public override SeedBase saveData ()
	{
		return base.saveData ();
	}

	public override void loadData (SeedBase seed)
	{
		base.loadData (seed);
	}
	*/
	#endregion

	#endregion

	#region INTERNAL_TYPES
	/*
	private class PSeed : Seed
	{

		public PSeed(GameObject g) : base(g)
		{

		}
	}
	*/
	#endregion
}
