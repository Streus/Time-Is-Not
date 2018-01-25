using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Discuss changing this to a MonoBehavior
public class LevelStateManager : Singleton<LevelStateManager> 
{
	// Events
	public event StateChange stateAdded;
	public event StateChange stateLoaded; 
	public event StateGeneric stateListFull; 

	public event StasisChange stasisAdded;
	public event StasisChange stasisRemoved;


	// Delegates
	public delegate void StateGeneric();
	public delegate void StateChange(bool success);

	public delegate void StasisChange(bool success);

	// State Variables

	// curState represents the last save state available
	// If pointA represents the last save state made, pointB represents the next available save state, and the arrow 
	// 		in between represents the player's current pos in the timeline, then curState equals pointA
	private int m_curState;
	public static int curState
	{
		get{
			return inst.m_curState; 
		}
	}

	[SerializeField] private int m_maxNumStates; 
	public static int maxNumStates
	{
		get{
			return inst.m_maxNumStates; 
		}
	}

	// State data
	List<Dictionary<string, SeedBase>> stateSeeds; 


	void Start()
	{
		stateSeeds = new List<Dictionary<string, SeedBase>> (); 

		for (int i = 0; i < m_maxNumStates; i++)
		{
			stateSeeds.Add(new Dictionary<string, SeedBase> ()); 
		}

		// Save default state seeds
		m_curState = -1; 
		addState(); 
	}

	void Update()
	{
		
	}

	// Internal State Methods 
	bool addState()
	{
		if (m_curState + 1 > maxNumStates)
		{
			return false; 
		}

		// Create a new Dictionary to save at the current state
		// Actually, maybe don't do this because store() might need to use the next dictionary
		//stateSeeds[m_curState] = new Dictionary<string, SeedBase> (); 

		// Increment the current state
		m_curState++; 

		// Check if a Dictionary doesn't exist because the count of stateSeeds is too low/zero
		if (m_curState + 1 > stateSeeds.Count)
		{
			Debug.LogError("Trying to access an out-of-bounds state in stateSeeds. stateSeeds.count = " + stateSeeds.Count + "; m_curState = " + m_curState); 
			//stateSeeds.Add(new Dictionary<string, SeedBase> ()); 
			return false; 
		}

		// Check if the Dictionary position of curState is null, and instantiate a new dictionary if so
		if (stateSeeds[m_curState] == null)
		{
			Debug.LogError("Trying to access a null Dictionary in stateSeeds");
			//stateSeeds[m_curState] = new Dictionary<string, SeedBase> (); 
			return false; 
		}

		//add each RO's data to the dictionary
		foreach (RegisteredObject ro in RegisteredObject.getObjects())
		{
			if (stateSeeds[m_curState].ContainsKey (ro.rID))
				stateSeeds[m_curState].Remove (ro.rID);
			stateSeeds[m_curState].Add (ro.rID, ro.reap ());
		}

		if (stateAdded != null)
		{
			stateAdded(true); 
		}

		return true; 
	}

	// Called by RegisteredObjects when their client components are destroyed in gameplay.
	// Places the passed seed into the current scene's dictionary
	/*
	public void store(string ID, SeedBase seed)
	{
		Debug.Log("Store destroyed object"); 

		//add the entry to the current scene dictionary
		if (stateSeeds[m_curState + 1].ContainsKey (ID))
			stateSeeds[m_curState + 1].Remove (ID);
		stateSeeds[m_curState + 1].Add (ID, seed);
	}
	*/ 

	bool loadState()
	{
		return loadState(curState); 
	}

	bool loadState(int state)
	{
		if (state < 0 || state > m_curState)
		{
			Debug.LogError("Tried to load invalid state: " + state); 
			return false; 
		}

		m_curState = state; 


		if (stateSeeds[state] == null)
		{
			Debug.LogError("Tried to load state " + state + " for which the dictionary in stateSeeds[" + state + "] is null."); 
			return false; 
		}


		// We have a registered object that is not in our seed base list
		// Destroy objects that no longer exist in the scene
		// First, create a hash set for faster comparisons
		HashSet<string> seedSet = new HashSet<string> (); 

		foreach (SeedBase sb in stateSeeds[state].Values)
		{
			seedSet.Add(sb.registeredID); 
		}

		RegisteredObject[] registeredObjects = RegisteredObject.getObjects(); 

		for (int i = 0; i < registeredObjects.Length; i++)
		{
			// If there is a registered object id for a seed that doesn't exist
			if (!seedSet.Contains(registeredObjects[i].rID))
			{
				Destroy(registeredObjects[i].gameObject); 
			}
		}

		// If there is a seed for a registered object that doesn't exist
		HashSet<string> roSet = new HashSet<string>();
		foreach (RegisteredObject r in registeredObjects)
			roSet.Add(r.rID);
			
		foreach (SeedBase sb in stateSeeds[state].Values)
		{
			if (!roSet.Contains(sb.registeredID))
			{
				if (RegisteredObject.recreate (sb.prefabPath, sb.registeredID, sb.parentID) != null)
					Debug.Log ("[SSM] Respawned prefab object: " + sb.registeredID + ".");
				else
					Debug.Log ("[SSM] Failed to respawn prefab object: " + sb.registeredID + ".");
			}
		}

		/*
		// We have a seed for a registered object that doesn't exist
		//spawn prefabs before starting sow cycle
		foreach (SeedBase sb in stateSeeds[state].Values)
		{
			if (sb.prefabPath != "")
			{
				if (RegisteredObject.recreate (sb.prefabPath, sb.registeredID, sb.parentID) != null)
					Debug.Log ("[SSM] Respawned prefab object: " + sb.registeredID + ".");
				else
					Debug.Log ("[SSM] Failed to respawn prefab object: " + sb.registeredID + ".");
			}
		}
		*/ 

		//iterate over the list of ROs and pass them data
		foreach (RegisteredObject ro in RegisteredObject.getObjects())
		{
			SeedBase data;
			if (stateSeeds[state].TryGetValue (ro.rID, out data))
				ro.sow (data);
		}

		// Reset state data for dictionaries after the current new state, if they've already been instantiated
		for (int i = state + 1; i < stateSeeds.Count; i++)
		{
			stateSeeds[i] = new Dictionary<string, SeedBase> (); 
		}

		if (stateLoaded != null)
		{
			stateLoaded(true); 
		}

		return true;
	}

	// Public state query methods
	public static bool canCreateTetherPoint()
	{
		if (curState + 1 >= maxNumStates)
		{
			return false; 
		}
		return true; 
	}

	public static bool canLoadTetherPoint(int state)
	{
		if (state < 0 || state > curState)
		{
			return false; 
		}
		return true; 
	}

	// State change methods
	public static void createTetherPoint()
	{
		if (canCreateTetherPoint())
		{
			inst.addState(); 
		}
	}

	/// <summary>
	/// Public function for loading a specific tether point
	/// </summary>
	/// <param name="state">State.</param>
	public static bool loadTetherPoint(int state)
	{
		// Check that state is valid
		// TODO check that state < curState is correct
		if (state >= 0 && state <= curState)
		{
			inst.loadState(state); 
			return true; 
		}
		else
		{
			return false; 
		}
	}


}
