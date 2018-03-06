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
    private State playerDefault;

    [SerializeField]
    private State dashState;

    private Vector3 jumpTargetPos;

    public LayerMask moveMask;

    [SerializeField]
    private bool isOverPit;

    [SerializeField]
    private bool isOnPlatform;

    [SerializeField]
    private float minJumpDist;

    public SpriteRenderer sprite;

    [SerializeField]
    private float maxJumpDist;

	private Transform shadow;

    #endregion

    #region INSTANCE_METHODS
    public void Start()
    {
		shadow = transform.Find ("Margaux Shadow");
        sprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        getSelf().addAbility(Ability.get("Place Stasis"));
        getSelf().addAbility(Ability.get("Dash"));
        getSelf().died += deathReset;
        getSelf().getAbility(1).resetCooldown();
    }

    public override void Update()
    {
		
        // Conditions for stopping the Player from updating
        // (a) If the camera is not zoomed out 
		if (GameManager.inst != null && !GameManager.CameraIsZoomedOut())
        {
            base.Update();
        }
        CheckForGround();
        sprite.sortingOrder = SpriteOrderer.inst.OrderMe(transform);
		SpriteRenderer shadowRend = shadow.GetComponent<SpriteRenderer> ();
		if(shadowRend != null)
			shadowRend.sortingOrder = SpriteOrderer.inst.OrderMe(shadow);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnDestroy()
    {
        getSelf().died -= deathReset;
    }
    private void deathReset()
    {
        setState(playerDefault);
        getSelf().getAbility(1).active = true;
    }

	#region GETTERS_SETTERS

	public Vector3 getJumpTargetPos()
	{
		return jumpTargetPos;
	}

	public void setJumpTargetPos(Vector3 jumpTarget)
	{
		jumpTargetPos = jumpTarget;
	}

	public float getMaxJumpDist()
	{
		return maxJumpDist;
	}

	public float getMinJumpDist()
	{
		return minJumpDist;
	}
	#endregion

	#region SPECIAL_BEHAVIOUR
    public void enterPushState()
    {
        prePushState = getState();
        setState(pushState);
    }

    public void exitPushState()
    {
        setState(prePushState);
    }

    public bool pushing()
    {
        return (pushState == getState());
    }

    public void enterDashState()
    {
        setState(dashState);
    }

    public bool dashing()
    {
        return (dashState == getState());
    }

	/// <summary>
	/// Standard movement behavior for the Player.
	/// DON'T CALL THIS
	/// </summary>
    public void move()
    {
        Vector2 movementVector = Vector2.zero;

        if (Input.GetKey(PlayerControlManager.RH_Up) || Input.GetKey(PlayerControlManager.LH_Up)) // UP
            movementVector += Vector2.up;
        if (Input.GetKey(PlayerControlManager.RH_Left) || Input.GetKey(PlayerControlManager.LH_Left)) // LEFT
            movementVector += Vector2.left;
        if (Input.GetKey(PlayerControlManager.RH_Down) || Input.GetKey(PlayerControlManager.LH_Down)) // DOWN
            movementVector += Vector2.down;
        if (Input.GetKey(PlayerControlManager.RH_Right) || Input.GetKey(PlayerControlManager.LH_Right)) // RIGHT
            movementVector += Vector2.right;

        movementVector = movementVector.normalized * getSelf().getMovespeed() * Time.deltaTime;
		physbody.velocity += movementVector;
		if (physbody.velocity.magnitude > getSelf ().getMovespeed ())
			physbody.velocity = physbody.velocity.normalized * getSelf ().getMovespeed ();

        // Wall check / check for moving platforms
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = 0;
        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask(moveMask);
        hitCount = GetComponent<Collider2D>().Cast(movementVector, cf, hits, getSelf().getMovespeed() * Time.deltaTime);
        if (hitCount <= 0)
            transform.Translate((Vector3)movementVector);
    }

	/// <summary>
	/// Sets the target jump position for the dash.
	/// DON'T CALL THIS!
	/// </summary>
	public void findTarget()
	{
		BoxCollider2D collider = GetComponent<BoxCollider2D> ();
		Vector2 colliderSize = new Vector2(collider.size.x * transform.localScale.x, collider.size.y * transform.localScale.y) * .5f;
		Vector2 colliderOffset = collider.offset;
		Vector3 pospoff = transform.position + (Vector3)colliderOffset;

		float jumpDistance = maxJumpDist;
		Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pospoff;

		//do a boxcast from the player to the furthest possible jump position
		RaycastHit2D[] pathCheck = Physics2D.BoxCastAll(pospoff, colliderSize, 0.0f, dir, jumpDistance, moveMask.value);

		if (pathCheck != null)
		{
			//find the nearest collision returned by the circlecast, ifex
			RaycastHit2D nearest = default(RaycastHit2D);
			foreach (RaycastHit2D hit in pathCheck)
			{
				if (hit.collider.isTrigger || hit.collider == GetComponent<Collider2D>())
					continue;

				if (nearest == default(RaycastHit2D))
					nearest = hit;
				else if (hit.distance < nearest.distance)
					nearest = hit;
			}

			//set the jump distance to the distance to the nearest collision, ifex
			if (nearest != default(RaycastHit2D) && nearest.distance < jumpDistance)
				jumpDistance = nearest.distance;
		}

		Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float dist = Vector2.Distance(mp, pospoff);
		if (dist > jumpDistance) //if the distance to the mouse is greater than our max jump distance, limit the target position to our max jump distance
			jumpTargetPos = (Vector2)transform.position + colliderOffset + (mp - (Vector2)transform.position - colliderOffset).normalized * jumpDistance;
		else if (dist < minJumpDist) // if there's no concept of a minimum distance, cut this line out as well as the line with the if statement
			jumpTargetPos = (Vector2)transform.position + colliderOffset + (mp - (Vector2)transform.position - colliderOffset).normalized * Mathf.Min(jumpDistance, minJumpDist);
		else //mouse is within the max distance, no need to limit
			jumpTargetPos = mp;
	}

    //checks for pits and moving platforms under the players feet
    public void CheckForGround()
    {
        Collider2D[] colsHit = Physics2D.OverlapPointAll(transform.position + (Vector3)gameObject.GetComponent<BoxCollider2D>().offset,  1 << LayerMask.NameToLayer("Pits"));
		Collider2D[] platformsFound = Physics2D.OverlapBoxAll (transform.position + (Vector3)gameObject.GetComponent<BoxCollider2D> ().offset, gameObject.GetComponent<BoxCollider2D>().size * 0.5f, 0,  1 << LayerMask.NameToLayer ("SkyEnts"));
        bool seesMP = false;
        bool seesPit = false;
        for (int i = 0; i < colsHit.Length; i++)
        {
            if (colsHit[i].gameObject.CompareTag("Pit"))
            {
                seesPit = true;
            }
        }
		for(int i = 0; i < platformsFound.Length; i++)
		{

			if (platformsFound[i].gameObject.CompareTag("MovingPlatform"))
			{
				seesMP = true;
			}
		}
        isOnPlatform = seesMP;
        isOverPit = seesPit;
        if (isOverPit && !isOnPlatform && !dashing())
        {
            Entity ent = gameObject.GetComponent<Entity>();
            if (ent != null)
                ent.onDeath();
        }
    }

	public void StartWait()
	{
		StartCoroutine(WaitForTether());
	}

	public IEnumerator WaitForTether()
	{
		enterPushState();
		yield return new WaitForSeconds(.5f);
		exitPushState();
	}
	#endregion
    
    #region ISAVABLE_METHODS

    public override SeedBase saveData()
    {
        return new PSeed(this);
    }

    public override void loadData(SeedBase seed)
    {
        base.loadData(seed);
        PSeed p = (PSeed)seed;
        jumpTargetPos = p.jumpTargetPos;
    }

    #endregion

	public override void OnDrawGizmos()
	{
		// Dash debug
		BoxCollider2D col =  GetComponent<BoxCollider2D>();
		if (jumpTargetPos != Vector3.zero)
		{
			Gizmos.color = getState().color;
			Vector2 csize = col.size;
			Vector3 actualSize = new Vector3(csize.x * transform.localScale.x, csize.y * transform.localScale.y);
			Gizmos.DrawWireCube(jumpTargetPos, actualSize);
		}

		#if UNITY_EDITOR
		UnityEditor.Handles.color = getState().color;
		UnityEditor.Handles.DrawWireDisc(transform.position + (Vector3)col.offset, Vector3.forward, minJumpDist);
		UnityEditor.Handles.DrawWireDisc(transform.position + (Vector3)col.offset, Vector3.forward, maxJumpDist);
		#endif
	}
    #endregion

    #region INTERNAL_TYPES
    private class PSeed : Seed
    {
        public Vector3 jumpTargetPos;

        public PSeed(Controller c) : base(c)
        {
            Player p = State.cast<Player>(c);
            jumpTargetPos = p.jumpTargetPos;
        }
    }
    #endregion
}
