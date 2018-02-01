using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : Interactable, IActivatable, ISavable, IStasisable
{
	[Tooltip("How far can the laser go? 0-Infinity.")]
	[SerializeField]
	private float _distance = Mathf.Infinity;

	[Tooltip("Layers the laser can collide with.")]
	[SerializeField]
	private LayerMask _layersToHit;

	[Tooltip("The gradient for the laser's color.")]
	[SerializeField]
	private Gradient _laserColor;

	[Tooltip("Is the laser a death laser or a trigger?")]
	[SerializeField]
	private LaserType _type;

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

	//the line renderer for the laser
	private LineRenderer _laserLine;

	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = false;

	enum LaserType {Death, Trigger};

	// Use this for initialization
	void Start()
	{
		_laserLine = gameObject.GetComponent<LineRenderer> ();
		_laserLine.colorGradient = _laserColor;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isEnabled()) 
		{
			rayCast ();
			if (!_laserLine.enabled)
				_laserLine.enabled = true;
		}
		else
		{
			if (_laserLine.enabled)
				_laserLine.enabled = false;
		}
			
	}

	//Draw lines to all linked activatables
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		for(int i = 0; i < _activatables.Length; i++)
		{
			Gizmos.DrawLine (transform.position, _activatables[i].transform.position);
		}

	}

	/// <summary>
	/// Raycasts forward and draws the laser line.
	/// </summary>
	void rayCast()
	{
		RaycastHit2D hit = Physics2D.Raycast (transform.position, transform.up, _distance, _layersToHit);

		_laserLine.SetPosition (0, transform.position);

		if (hit.collider == null) 
		{
			_laserLine.SetPosition (1, transform.position + (transform.up * _distance));
			return;
		} else
		{
			_laserLine.SetPosition (1, hit.point);
		}
		if (hit.collider.gameObject.GetComponent<Entity> () != null) 
		{
			Entity entityHit = hit.collider.gameObject.GetComponent<Entity> ();
			if (entityHit.getFaction () == Entity.Faction.player) 
			{
				trigger (entityHit);
			}
		}
	}

	/// <summary>
	/// Triggers Activatables.
	/// </summary>
	public override void onInteract ()
	{
		disable ();
		foreach(GameObject activatable in _activatables)
		{
			if(activatable.GetComponent<IActivatable>() != null)
				activatable.GetComponent<IActivatable>().onActivate ();
		}
	}

	/// <summary>
	/// Activates the laser's effect.
	/// </summary>
	public void trigger (Entity entity)
	{
		switch(_type)
		{
		case LaserType.Trigger:
			onInteract ();
			break;
		case LaserType.Death:
			entity.onDeath ();
			break;
		}
	}

	/// <summary>
	/// Toggles the laser's state and returns it.
	/// </summary>
	public bool onActivate()
	{
		if (isEnabled ())
			disable ();
		else
			enable ();
		return isEnabled();
	}

	/// <summary>
	/// Sets the laser's state and returns it.
	/// </summary>
	public bool onActivate(bool state)
	{
		if (state)
			enable ();
		else
			disable ();
		return isEnabled();
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed (gameObject);

		seed.isOn = isEnabled ();

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		if (s == null)
			return;

		if (!inStasis)
		{
			return; 
		}

		Seed seed = (Seed)s;

		s.defaultLoad (gameObject);

		if (seed.isOn)
			enable ();
		else
			disable ();
	}

	/// <summary>
	/// Checks if the object should be able to be reset.
	/// </summary>
	/// <returns><c>true</c>, if it should ignore it, <c>false</c> otherwise.</returns>
	public bool shouldIgnoreReset() 
	{ 
		return !inStasis; 
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the laser on?
		public bool isOn;

		public Seed(GameObject subject) : base(subject) {}

	}


	//****Stasisable Object Functions****

	/// <summary>
	/// Toggles if the object is in stasis.
	/// </summary>
	/// <param name="turnOn">If set to <c>true</c> turn on.</param>
	public void ToggleStasis(bool turnOn)
	{
		inStasis = turnOn;
	}

	/// <summary>
	/// shows if the object is in stasis
	/// </summary>
	/// <returns><c>true</c>, if stasis is active, <c>false</c> otherwise.</returns>
	public bool InStasis()
	{
		return inStasis; 
	}

}
