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

	// Reference variables
	[SerializeField] private GameObject playerObj; 

	// Savable data
	[SerializeField] List<string> codes; 

	// --- ISavable Methods ---
	public SeedBase saveData()
	{
		//SeedBase seed = new SeedBase (gameObject);
		Seed seed = new Seed (gameObject);
		seed.codes = codes; 

		return seed;
	}
	public void loadData(SeedBase s)
	{
		if (s == null)
			return;

		Seed seed = (Seed)s;

		codes = seed.codes;

		s.defaultLoad (gameObject); 
	}
	public bool shouldIgnoreReset() { return false; }

	public class Seed : SeedBase
	{
		// Define all the extra variables that should be saved here
		public List<string> codes; 

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

	public static GameObject GetPlayer()
	{
		return inst.playerObj; 
	}

	public static bool AddCode(string codeName)
	{
		if (inst.codes.Contains(codeName))
		{
			return false; 
		}
		if (codeName == "")
		{
			Debug.LogError("Tried to add a code with an empty string"); 
			return false; 
		}

		inst.codes.Add(codeName); 
		return true; 
	}

	public static bool HasCode(string codeName)
	{
		if (inst.codes.Contains(codeName))
		{
			return true; 
		}
		return false; 
	}

}
