using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CodeName
{
	CODE_1, 
	CODE_2,
	CODE_3,
	CODE_4,
	CODE_5,
	CODE_6,
	CODE_7
};

[RequireComponent(typeof(RegisteredObject))]
public class GameManager : Singleton<GameManager> , ISavable
{
	[Tooltip("Empty gameobject containing all security doors in the level that should be tripped at once")]
	public GameObject securityDoors;

	// Pause functionality
	bool paused; 
	bool pauseLock; 

	// Ability setup
	public bool canUseStasis; 
	public bool canUseDash; 

	// Death functionality
	bool isDead; 

	// Actions
	public event StateToggled pauseToggled; 
	public event StateToggled pauseLockedToggled; 
	public event CodesUpdated codesUpdated; 

	// Delegates
	public delegate void StateToggled(bool state); 
	public delegate void CodesUpdated (); 

	// Reference variables
	[SerializeField] private GameObject playerObj; 

	// Savable data
	[SerializeField] List<CodeName> codes; 

	// --- ISavable Methods ---
	public SeedBase saveData()
	{
		//SeedBase seed = new SeedBase (gameObject);
		Seed seed = new Seed ();
		seed.codes = codes; 

		return seed;
	}
	public void loadData(SeedBase s)
	{
		Seed seed = (Seed)s;

		codes = seed.codes;

		if (isDead)
			isDead = false; 
	}

	public class Seed : SeedBase
	{
		// Define all the extra variables that should be saved here
		public List<CodeName> codes; 
	}

	// MonoBehaviors
	public void Awake()
	{
		if (playerObj != null)
		{
			playerObj.GetComponent<Entity> ().died += killPlayer;
		}
	}

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

	public static bool isPlayerDead()
	{
		return inst.isDead; 
	}

	public static void killPlayer()
	{
		inst.isDead = true; 
		inst.paused = true;
	}

	public static GameObject GetPlayer()
	{
		return inst.playerObj; 
	}

	public static bool AddCode(CodeName codeName)
	{
		if (inst.codes.Contains(codeName))
		{
			return false; 
		}

		inst.codes.Add(codeName); 

		if (inst.codesUpdated != null)
		{
			inst.codesUpdated(); 
		}

		return true; 
	}

	public static bool HasCode(CodeName codeName)
	{
		if (inst.codes.Contains(codeName))
		{
			return true; 
		}
		return false; 
	}

}
