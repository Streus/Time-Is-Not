﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller
{
    #region INSTANCE_VARS
    [SerializeField]
    private State pushState;

    private State prePushState;

    [SerializeField]
    private State dashState;

    private Vector3 jumpTargetPos;

    public LayerMask moveMask;

    [SerializeField]
    private float minJumpDist;

    public float getMinJumpDist
    {
        get
        {
            return minJumpDist;
        }
    }

    [SerializeField]
    private float maxJumpDist;

    public float getMaxJumpDist
    {
        get
        {
            return maxJumpDist;
        }
    }

    public void setJumpTargetPos(Vector3 jumpTarget)
    {
        jumpTargetPos = jumpTarget;
    }

    public Vector3 getJumpTargetPos
    {
        get
        {
            return jumpTargetPos;
        }
    }

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

    public void enterDashState()
    {
        setState(dashState);
    }

    public bool dashing()
    {
        return (dashState == getState());
    }

    public void move()
    {
        Vector2 movementVector = Vector2.zero;

        if (Input.GetKey(PlayerControlManager.RH_Up) || Input.GetKey(PlayerControlManager.LH_UP)) // UP
            movementVector += Vector2.up;
        if (Input.GetKey(PlayerControlManager.RH_Left) || Input.GetKey(PlayerControlManager.LH_Left)) // LEFT
            movementVector += Vector2.left;
        if (Input.GetKey(PlayerControlManager.RH_Down) || Input.GetKey(PlayerControlManager.LH_Down)) // DOWN
            movementVector += Vector2.down;
        if (Input.GetKey(PlayerControlManager.RH_Right) || Input.GetKey(PlayerControlManager.LH_Right)) // RIGHT
            movementVector += Vector2.right;

        movementVector = movementVector.normalized * getSelf().getMovespeed() * Time.deltaTime;

        // Wall check
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = 0;
        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask(moveMask);
        hitCount = GetComponent<Collider2D>().Cast(movementVector, cf, hits, getSelf().getMovespeed() * Time.deltaTime);
		Collider2D[] colsHit = Physics2D.OverlapCircleAll(transform.position + (Vector3)GetComponent<BoxCollider2D>().offset, movementVector.magnitude + 0.5f, 1 << LayerMask.NameToLayer("SkyEnts"));
		bool seesMP = false;
		Debug.Log (colsHit.Length);
		for(int i = 0; i < colsHit.Length; i++)
		{
			if (colsHit [i].gameObject.CompareTag ("MovingPlatform"))
			{
				gameObject.layer = LayerMask.NameToLayer ("MPPassenger");
				seesMP = true;
				break;
			}
		}
		if (hitCount <= 0 || seesMP)
            transform.Translate((Vector3)movementVector);
		if(!seesMP)
			gameObject.layer = LayerMask.NameToLayer ("GroundEnts");
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
