using System.Collections;
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
        getSelf().died += deathReset;
    }

	public override void Update()
	{
		// Conditions for stopping the Player from updating
		// (a) If the camera is not zoomed out 
		if (!GameManager.CameraIsZoomedOut())
		{
			base.Update(); 
		}
	}

	public override void FixedUpdate ()
	{
		base.FixedUpdate ();
	}

    private void OnDestroy()
    {
        getSelf().died -= deathReset;
        
    }
    private void deathReset()
    {
        Debug.Log("I reset");
        setState(playerDefault);
        getSelf().getAbility(1).active = true;
        
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

        // Wall check / check for moving platforms
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = 0;
        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask(moveMask);
        hitCount = GetComponent<Collider2D>().Cast(movementVector, cf, hits, getSelf().getMovespeed() * Time.deltaTime);
		Collider2D[] colsHit = Physics2D.OverlapCircleAll(transform.position + (Vector3)GetComponent<BoxCollider2D>().offset, movementVector.magnitude + 0.5f, 1 << LayerMask.NameToLayer("SkyEnts"));
		bool seesMP = false;
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

	public void OnDrawGizmos()
	{
		// Dash debug
		if (jumpTargetPos != Vector3.zero)
		{
			Gizmos.color = Color.green;
			Vector2 csize = GetComponent<BoxCollider2D> ().size;
			Vector3 actualSize = new Vector3 (csize.x * transform.localScale.x, csize.y * transform.localScale.y);
			Gizmos.DrawWireCube (jumpTargetPos, actualSize);
		}
	}
	#region ISAVABLE_METHODS

	public override SeedBase saveData ()
	{
		return new PSeed (this);
	}

	public override void loadData (SeedBase seed)
	{
		base.loadData (seed);
		PSeed p = (PSeed)seed;
		jumpTargetPos = p.jumpTargetPos;
	}

	#endregion

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
