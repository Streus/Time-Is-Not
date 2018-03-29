using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANDInput : MonoBehaviour, IActivatable, ISavable
{
	[SerializeField]
	private bool _state;
	// Use this for initialization
	void Awake () 
	{
		
	}

	/// <summary>
	/// Toggles the laser's state and returns it.
	/// </summary>
	public bool onActivate()
	{
		_state = !_state;
		return _state;
	}

	/// <summary>
	/// Sets the laser's state and returns it.
	/// </summary>
	public bool onActivate(bool state)
	{
		_state = state;
		return _state;
	}

	public bool State
	{
		get{return _state;}
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

		seed.state = _state;

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

		_state = seed.state;
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the plate pressed?
		public bool state;
	}
}
