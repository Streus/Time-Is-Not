using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : Interactable, ISavable
{
	[SerializeField]
	private Vector2 _range = new Vector2(1,1);

	[SerializeField]
	private bool _resetsOnLoad = false;

	private Vector2 _upperBound;
	private Vector2 _lowerBound;

	private GameObject _player;

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

	// Use this for initialization
	void Start () 
	{
		_player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		_upperBound = new Vector2 (transform.position.x + (_range.x/2), transform.position.y + (_range.y/2));
		_lowerBound = new Vector2 (transform.position.x - (_range.x/2), transform.position.y - (_range.y/2));
		if(CheckForPlayer())
		{
			onInteract ();
		}
	}

	bool CheckForPlayer()
	{
		bool xCheck = (_player.transform.position.x > _lowerBound.x && _player.transform.position.x < _upperBound.x);
		bool yCheck = (_player.transform.position.y > _lowerBound.y && _player.transform.position.y < _upperBound.y);
		return (xCheck && yCheck);
	}

	public override void onInteract ()
	{
		if (!isEnabled())
			return;
		ToggleSecurityDoors ();
		disable ();
		if (_activatables == null || _activatables.Length == 0)
			return;
		foreach(GameObject activatable in _activatables)
		{
			if (activatable != null) 
			{
				if (activatable.GetComponent<IActivatable> () != null)
					activatable.GetComponent<IActivatable> ().onActivate ();
			}
		}
	}

	void OnDrawGizmos()
	{
		float xdist = _range.x / 2;
		float ydist = _range.y / 2;
		Vector2 center = (transform.position);
		Gizmos.color = Color.green;
		Gizmos.DrawLine (center + new Vector2(-xdist, ydist), center + new Vector2(xdist, ydist));
		Gizmos.DrawLine (center + new Vector2(xdist, -ydist), center + new Vector2(xdist, ydist));
		Gizmos.DrawLine (center + new Vector2(-xdist, -ydist), center + new Vector2(xdist, -ydist));
		Gizmos.DrawLine (center + new Vector2(-xdist, ydist), center + new Vector2(-xdist, -ydist));
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

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

		Seed seed = (Seed)s;
		if (!_resetsOnLoad)
			return;
		if (seed.isOn)
			enable ();
		else
			disable ();
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the laser on?
		public bool isOn;
	}
}
