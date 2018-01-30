using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RegisteredObject))]
public class GameManager : Singleton<GameManager> , ISavable
{
	// Pause functionality
	bool paused; 
	bool pauseLock; 

	// Actions
	public event StateToggled pauseToggled; 
	public event StateToggled pauseLockedToggled; 

	// Delegates
	public delegate void StateToggled(bool state); 

	// --- ISavable Methods ---
	public SeedBase saveData()
	{
		//SeedBase seed = new SeedBase (gameObject);
		Seed seed = new Seed (gameObject);
		//seed.variable = variable; 

		return seed;
	}
	public void loadData(SeedBase s)
	{
		if (s == null)
			return;

		Seed seed = (Seed)s;

		s.defaultLoad (gameObject);

		//variable = seed.variable; 
	}
	public bool shouldIgnoreReset() { return false; }

	public class Seed : SeedBase
	{
		// Define all the extra variables that should be saved here



		public Seed(GameObject subject) : base(subject) { }

	}

	// MonoBehaviors

	public static void setPause(bool state)
	{
		// Return if locked or the state isn't actually changing
		if (inst.pauseLock || inst.paused == state)
		{
			return; 
		}

		inst.paused = state; 

		if (inst.pauseToggled != null)
		{
			inst.pauseToggled(state); 
		}

	}

	public static bool isPaused()
	{
		return inst.paused; 
	}

	public static void setPauseLock(bool state)
	{
		inst.pauseLock = state; 

		if (inst.pauseLockedToggled != null)
		{
			inst.pauseLockedToggled(state); 
		}
	}

}
