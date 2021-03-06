﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller
{
	#region STATIC_VARS

	public const int PAUSEMASK_MOVE = ~0x0;
	#endregion

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

	//how long it takes the player to fall into a pit while walking against it
	[SerializeField]
	private float pitFallDelay = 1f;

	[SerializeField]
	private float pitSpeedAdjustment = 0.2f;

	//how long the player has been walking against a pit
	private float pitTimer = 0f;

	private Vector3 TESTPOSITION;
	private Vector3 TESTSIZE;

	//for delaying some checks
	private float miscDelay;
	private bool placingTether = true;
	private bool shootingSB = false;

	public delegate void AnimEvent ();
	public event AnimEvent tetherAnchorEnded;

	// Dash delegates/events
	public delegate void DashEvent(); 
	public event DashEvent dashStarted;  

    #endregion

    #region INSTANCE_METHODS

    public void Start()
    {
		shadow = transform.Find ("Margaux Shadow");
		sprite = gameObject.GetComponent <SpriteRenderer> ();
        getSelf().addAbility(Ability.get("Place Stasis"));
        getSelf().addAbility(Ability.get("Dash"));
        getSelf().died += deathReset;
		getSelf ().getAbility (0).resetCooldown ();
        getSelf().getAbility(1).resetCooldown();

		anim = gameObject.GetComponent <Animator>();
    }

    public override void Update()
    {
        // Conditions for stopping the Player from updating
        // (a) If the camera is not zoomed out 
		if (!GameManager.CheckPause (PAUSEMASK_MOVE) && !placingTether)
		{
			base.Update ();
		}
		else
		{
			anim.SetBool ("isMoving", false);
		}

		//wait for anchor anim to finish
		if (placingTether)
		{
			anim.SetBool ("isMoving", false);
			miscDelay -= Time.deltaTime;
			if (!inPlaceTetherAnim () && miscDelay <= 0f)
			{
				endAnchorAnim ();
			}
		}

		//wait for shooting anim
		if (shootingSB)
		{
			anim.SetBool ("isMoving", false);
			miscDelay -= Time.deltaTime;
			if (!inShootingAnim () && miscDelay <= 0f)
			{
				shootingSB = false;
			}
		}

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
		//go to default state
        setState(playerDefault);

		//unpause stasis shot and dash cooldowns
		getSelf().getAbility(0).active = true;
        getSelf().getAbility(1).active = true;

		//stop animations
		anim.SetBool ("Dash", false);
		anim.SetBool ("isMoving", false);
		anim.SetBool("isPushing", false);
    }

	#region GETTERS_SETTERS

	public void setPlaceAnchorAnim()
	{
		placingTether = true;
		anim.SetBool ("isMoving", false);
		anim.SetTrigger ("PlaceAnchor");
		miscDelay = 0.1f;
	}

	private void endAnchorAnim()
	{
		placingTether = false;
		if (tetherAnchorEnded != null)
			tetherAnchorEnded ();
	}

	public void setActivateTTAnim()
	{
		anim.SetTrigger ("ActivateTimeTether");
	}

	public bool inPlaceTetherAnim()
	{
		return anim.GetCurrentAnimatorStateInfo (1).IsName ("Animation_MargauxPlaceTetherAnchor"); 
	}

	public bool inShootingAnim()
	{
		return anim.GetCurrentAnimatorStateInfo (1).IsName ("Animation_Margaux_StasisBubble");
	}

	public void setReappearAnim()
	{
		anim.SetTrigger ("Reappear");
		anim.SetTrigger ("StopFalling");
		anim.SetBool ("Stumbling", false);
	}

	public void setDashAngle(float angle)
	{
		anim.SetFloat ("Angle", angle);
	}

	public void setStasisShootAnim()
	{
		anim.SetTrigger ("StasisBubble");
		anim.SetBool ("isMoving", false);
		placingTether = true;
		miscDelay = 0.3f;
	}

	public void setDashingAnim(bool val)
	{
		anim.SetBool ("Dash", val);
	}

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

	public bool inDefault()
	{
		return getState ().name == "PlayerDefault";
	}

	public bool pushing()
	{
		return (pushState.Equals(getState()));
	}

	public bool dashing()
	{
		return (dashState.Equals(getState()));
	}
	#endregion

	#region SPECIAL_BEHAVIOUR
    public void enterPushState()
    {
        prePushState = getState();
        setState(pushState);

		anim.SetBool ("Dash", false);
		anim.SetBool ("isMoving", true);
		anim.SetBool ("isPushing", true);
    }

    public void exitPushState()
    {
        setState(prePushState);

		anim.SetBool ("isMoving", false);
		anim.SetBool ("isPushing", false);
    }
		
    public void enterDashState()
    {
        setState(dashState);

		if (dashStarted != null)
		{
			dashStarted(); 
		}
    }

	/// <summary>
	/// Standard movement behavior for the Player.
	/// DON'T CALL THIS
	/// </summary>
    public void move()
    {
        Vector2 movementVector = Vector2.zero;

        if (PlayerControlManager.GetKey(ControlInput.UP)) // UP
        {
            movementVector += Vector2.up;
        }
        if (PlayerControlManager.GetKey(ControlInput.LEFT)) // LEFT
        {
            movementVector += Vector2.left;
        }
		if (PlayerControlManager.GetKey(ControlInput.DOWN)) // DOWN
        {
            movementVector += Vector2.down;
        }
        if (PlayerControlManager.GetKey(ControlInput.RIGHT)) // RIGHT
        {
            movementVector += Vector2.right;
        }

		//update animator on movement direction, if we're moving
		if(movementVector != Vector2.zero)
		{
			float zRot = Vector2.SignedAngle (Vector2.right, movementVector);
			zRot += zRot < 0f ? 360f : 0f;
			int quadrant = (int)((zRot / 360f) * 4f);
			anim.SetInteger ("Direction", quadrant + 1);
		}

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

		//Check for Pits/platforms
		Collider2D[] pitsFound = new Collider2D[1];
		Collider2D[] platformsFound = new Collider2D[1];
		int pitCount = 0;
		int platformCount = 0;
		ContactFilter2D pitFilter = new ContactFilter2D();
		ContactFilter2D platformFilter = new ContactFilter2D();
		pitFilter.SetLayerMask(1 << LayerMask.NameToLayer("Pits"));
		pitFilter.useTriggers = true;
		platformFilter.SetLayerMask(1 << LayerMask.NameToLayer("SkyEnts"));
		platformFilter.useTriggers = true;

		Vector2 colSize = gameObject.GetComponent<BoxCollider2D> ().size;
		Vector2 moveVect = new Vector2(movementVector.normalized.x * colSize.x * 0.5f, movementVector.normalized.y * colSize.y);
		platformCount = Physics2D.OverlapBox((Vector3)GetComponent<Collider2D>().offset + transform.position + (Vector3)(moveVect), GetComponent<BoxCollider2D>().size, 0, platformFilter, platformsFound);
		pitCount = Physics2D.OverlapBox((Vector3)GetComponent<Collider2D>().offset + transform.position + (Vector3)(moveVect), GetComponent<BoxCollider2D>().size, 0, pitFilter, pitsFound);

		TESTPOSITION = (Vector3)GetComponent<Collider2D> ().offset + transform.position + (Vector3)(moveVect);
		TESTSIZE = GetComponent<BoxCollider2D> ().size;

		bool seesPlatform = false;
		for(int i = 0; i < platformsFound.Length; i++)
		{
			if(platformsFound[i] != null) 
			{
				if (platformsFound [i].CompareTag ("MovingPlatform"))
					seesPlatform = true;
			}
		}

		bool seesPit = (pitCount > 0 && !seesPlatform);
		//if there are no walls or pits, move
		if (hitCount <= 0 && !seesPit && movementVector != Vector2.zero)
		{
			transform.Translate ((Vector3)movementVector);
			anim.SetBool ("isMoving", true);
		}
		else
			anim.SetBool ("isMoving", false);

		//if there are no walls and there is a pit, incriment the pit timer
		if (seesPit && hitCount <= 0) 
		{
			anim.SetBool ("Stumbling", true);
			physbody.velocity -= (physbody.velocity * pitSpeedAdjustment);
			pitTimer += Time.deltaTime;
		}
		else 
		{
			anim.SetBool ("Stumbling", false);
			pitTimer = 0;
		}

		//if the pit timer fills, kill the player
		if (pitTimer >= pitFallDelay || (seesPit && movementVector == Vector2.zero) || (movementVector != Vector2.zero && !CheckForGround())) 
		{
			transform.position = transform.position + (Vector3)(moveVect * 3);
			anim.SetTrigger ("Fall");
			getSelf ().onDeath ();
		}
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

    //checks for pits and moving platforms under the players feet, returns false if the player is over a pit
	public bool CheckForGround()
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
		return !(isOverPit && !isOnPlatform && !dashing ());
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

		//Platform Debug
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube (TESTPOSITION, TESTSIZE);

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

	public void PlayTetherAnimation ()
	{
		anim.SetTrigger ("ActivateTimeTether");
	}

	public void PlayReappearAnimation ()
	{
		anim.SetTrigger ("Reappear");
		anim.SetTrigger ("StopFalling");
		anim.SetBool ("Stumbling", false);
	}
}
