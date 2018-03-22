using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisabler : MonoBehaviour, IActivatable, ISavable
{
	private bool isInverted;

	// Use this for initialization
	void Start () 
	{
		isInverted = gameObject.activeInHierarchy;	
	}

	/// <summary>
	/// Toggle the object's state.
	/// </summary>
	public bool onActivate()
	{
		gameObject.SetActive (!gameObject.activeInHierarchy);
		return gameObject.activeInHierarchy;
	}

	/// <summary>
	/// Set the object's state.
	/// </summary>
	public bool onActivate (bool state)
	{
		//if the door is inverted, a true state disables the door
		if(isInverted)
		{
			gameObject.SetActive (!state);
		}
		else
		{
			gameObject.SetActive (state);
		}
		return gameObject.activeInHierarchy;
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

		seed.isActive = gameObject.activeInHierarchy;

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		Seed seed = (Seed)s;

		gameObject.SetActive (seed.isActive);
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the door open?
		public bool isActive;
	}

}
