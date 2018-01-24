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


	// KeyCodes
	public KeyCode createTetherPointKey; 
	public KeyCode loadTetherPointKey;

	void Update()
	{
		if (Input.GetKeyDown(createTetherPointKey))
		{
			addState(); 
		}

		if (Input.GetKeyDown(loadTetherPointKey))
		{
			loadState(); 
		}
	}

	// Internal State Methods 
	bool addState()
	{
		if (m_curState + 1 >= maxNumStates)
		{
			return false; 
		}

		m_curState++; 

		// TODO Need a good way to find all objects with the IReapable interface. Alternatively, maybe an events system could be useful here?

		if (stateAdded != null)
		{
			stateAdded(true); 
		}

		return true; 
	}

	bool loadState()
	{
		return loadState(curState - 1); 
	}

	bool loadState(int state)
	{
		if (m_curState == 0)
		{
			return false; 
		}

		m_curState = state; 

		if (stateLoaded != null)
		{
			stateLoaded(true); 
		}

		return true;
	}

	// State change methods
	public static bool canCreateTetherPoint()
	{
		if (curState + 1 >= maxNumStates)
		{
			return false; 
		}
		return true; 
	}

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
	public static void loadTetherPoint(int state)
	{
		// Check that state is valid
		// TODO check that state < curState is correct
		if (state >= 0 && state < curState)
		{
			inst.loadState(state); 
		}
	}


}
