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
	public override void Update ()
	{
		base.Update ();

		Vector2 movementVector = Vector2.zero;

		if(Input.GetKey(KeyCode.W)) // UP
			movementVector += Vector2.up;
		if (Input.GetKey (KeyCode.A)) // LEFT
			movementVector += Vector2.left;
		if(Input.GetKey(KeyCode.S)) // DOWN
			movementVector += Vector2.down;
		if (Input.GetKey (KeyCode.D)) // RIGHT
			movementVector += Vector2.right;

		movementVector = movementVector.normalized * self.getMovespeed () * Time.deltaTime;

		transform.Translate ((Vector3)movementVector);
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

	#endregion
}
