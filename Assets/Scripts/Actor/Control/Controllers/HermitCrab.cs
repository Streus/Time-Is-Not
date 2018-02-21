using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitCrab : Controller
{
	#region INSTANCE_VARS
	// Where this crab wil return to if moved
	private Vector3 home;

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

	[Tooltip("The radius of the ability nullifying field")]
	[SerializeField]
	private float nullFieldRadius = 2f;
	#endregion

	#region INSTANCE_METHODS

	public override void Awake ()
	{
		base.Awake ();
		home = transform.position;
	}

	public bool updateSitDuration(float delta)
	{
		return (sitDuration -= delta) <= 0f;
	}
	public void resetSitDuration()
	{
		sitDuration = sitDurationMax;
	}

	public bool updateStandDuration(float delta)
	{
		return (standDuration -= delta) <= 0f;
	}
	public void resetStandDuration()
	{
		standDuration = standDurationMax;
	}

	public bool updateReturnTimer(float delta)
	{
		return (returnTimer -= delta) <= 0f;
	}
	public void resetReturnTimer()
	{
		returnTimer = returnTimerMax;
	}

	#region GETTERS_SETTERS
	public Vector3 getHome()
	{
		return home;
	}

	public float getNullFieldRadius()
	{
		return nullFieldRadius;
	}
	#endregion

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
		base.OnDrawGizmos ();

		if (getState () != null)
		{
			Gizmos.color = getState ().color;
			Gizmos.DrawWireSphere (transform.position, nullFieldRadius);
		}

		if (getMap () != null)
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
