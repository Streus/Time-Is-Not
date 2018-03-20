using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitCrab : Controller
{
	#region INSTANCE_VARS
	// Where this crab wil return to if moved
	private Vector3 home;
	private Animator animationController;
    private SpriteRenderer nullifierField;

	[Tooltip("How long this crab will remain sitting")]
	[SerializeField]
	private float sitDurationMax = 5f;
	private float sitDuration;

	[Tooltip("How long this crab will remain standing")]
	[SerializeField]
	private float standDurationMax = 5f;
	private float standDuration;

	[Tooltip("How long this crab will wait before returning home")]
	[SerializeField]
	private float returnTimerMax = 5f;
	private float returnTimer;
	[Tooltip("What the return timer will be set to if the HC was pushed when sitting.")]
	[SerializeField]
	private float returnTimerOnStand = 3f;
	private bool wasPushed = false;

	[Tooltip("The radius of the ability nullifying field")]
	[SerializeField]
	private float nullFieldRadius = 2f;

	[SerializeField]
	private LayerMask nullLayers, nullObstLayers;

	[Tooltip("A multiplier applied to the length of the sitting state when stasised")]
	[SerializeField]
	private float slowedMultiplier = 2f;
	private float sitMod = 1f;
	#endregion

	#region INSTANCE_METHODS

	public override void Awake ()
	{
		base.Awake ();
		home = transform.position;

		GetComponent<RegisteredObject> ().allowResetChanged += onStasised;

        Transform nullifierChild = transform.Find("NullifierField");
        nullifierField = nullifierChild.GetComponent<SpriteRenderer>();
        animationController = gameObject.GetComponent <Animator> ();
		animationController.SetBool ("Hide", true);
	}

	public void OnDestroy()
	{
		GetComponent<RegisteredObject> ().allowResetChanged -= onStasised;
	}

	private void onStasised(bool val)
	{
		sitMod = val ? slowedMultiplier : 1f;
		
		//TODO some visual effect for HC stasis'd?
	}

	#region GETTERS_SETTERS

	public bool updateSitDuration(float delta)
	{
		return (sitDuration += delta) >= sitDurationMax * sitMod;
	}
	public void resetSitDuration()
	{
		sitDuration = 0f;

		//crab is about to sit down
		//TODO crab sit animation here
		animationController.SetInteger ("Direction", 0);
		animationController.SetBool ("Hide", true);
        nullifierField.enabled = true;
	}

	public bool updateStandDuration(float delta)
	{
		return (standDuration -= delta) <= 0f;
	}
	public void resetStandDuration()
	{
		standDuration = standDurationMax;

		//crab is about to stand up
		//TODO crab stand animation here
		animationController.SetBool ("Hide", false);
        nullifierField.enabled = false;
    }

	public bool updateReturnTimer(float delta)
	{
		return (returnTimer -= delta) <= 0f;
	}
	public void resetReturnTimer()
	{
		returnTimer = returnTimerMax;

		//crab is about to return home
		//TODO iunno some animtion I guess?
	}
	public void setReturnTimerOnStand()
	{
		returnTimer = returnTimerOnStand;
	}

	public bool getWasPushed()
	{
		return wasPushed;
	}
	public void setWasPushed(bool val)
	{
		wasPushed = val;
	}

	public Vector3 getHome()
	{
		return home;
	}

	public float getNullFieldRadius()
	{
		return nullFieldRadius;
	}
	#endregion

	public void doNullify()
	{
		RaycastHit2D[] hits = Physics2D.CircleCastAll (transform.position, nullFieldRadius, Vector2.zero, 0f, nullLayers);
		for (int i = 0; i < hits.Length; i++)
		{
			RaycastHit2D losCheck;
			losCheck = Physics2D.Raycast (
				transform.position,
				hits[i].collider.transform.position - transform.position,
				Vector2.Distance (transform.position, hits[i].collider.transform.position),
				nullObstLayers.value);
			if (losCheck.collider != null)
				continue;

			Entity e = hits [i].collider.GetComponent<Entity> ();
			if (e != null && e.getFaction () == Entity.Faction.player)
			{
				e.addStatus (Status.get ("Nullified", 0.1f));
			}

			StasisBubble sb = hits [i].collider.GetComponent<StasisBubble> ();
			if (sb != null)
			{
				LevelStateManager.removeStasisBubble (sb);
			}
		}
	}

	#region ISAVABLE_METHODS

	public override SeedBase saveData ()
	{
		CSeed s = new CSeed (this);
		return s;
	}

	public override void loadData (SeedBase seed)
	{
		base.loadData (seed);
		CSeed c = (CSeed)seed;
		home = c.home;
		sitDuration = c.sitDuration;
		standDuration = c.standDuration;
		returnTimer = c.returnTimer;
	}
	#endregion

	public override void OnDrawGizmos()
	{
		if (getState () != null)
		{
			Gizmos.color = getState ().color;
			Gizmos.DrawWireSphere (transform.position, nullFieldRadius);
		}

		if (getMap () != null && Application.isPlaying)
		{
			Color c = getMap ().graphColor;
			Gizmos.color = new Color (1.0f - c.r, 1.0f - c.g, 1.0f - c.b);
			Gizmos.DrawLine (transform.position, getHome ());
		}
	}
	#endregion

	#region INTERNAL_TYPES

	private class CSeed : Seed
	{
		public Vector3 home;
		public float sitDuration, standDuration, returnTimer;

		public CSeed(Controller c) : base(c)
		{
			HermitCrab h = State.cast<HermitCrab>(c);
			home = h.home;
			sitDuration = h.sitDuration;
			standDuration = h.standDuration;
			returnTimer = h.returnTimer;
		}
	}
	#endregion
}
