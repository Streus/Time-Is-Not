using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public enum CodeName
{
	CODE_1, 
	CODE_2,
	CODE_3,
	CODE_4,
	CODE_5,
	CODE_6,
	CODE_7,
	CODE_8
};

public enum CursorType
{
	GAMEPLAY,
	UI_HOVER,
	DEACTIVATED,
	DEFAULT
}

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
	[SerializeField] List<CodeName> m_codes; 
	public List<CodeName> codes
	{
		get{
			return m_codes; 
		}
	}

	private string[] codeEnumNames; 

	[Header("Cursor functionality")]
	[Tooltip("The current state of the cursor")]
	public CursorType cursorType; 
	CursorType lastCursorType; 

	public Texture2D cursorTexture_GAMEPLAY;
	public Texture2D cursorTexture_UI_HOVER; 
	public Texture2D cursorTexture_DEACTIVATED;

	// Dynamically keeps track of any ui bounds that the cursor is currently within
	List<CursorBoundsCollider> collidingCursorBounds; 

	// When true, bounds interaction will not change the cursor type
	public bool lockCursorType;

	// --- ISavable Methods ---
	public SeedBase saveData()
	{
		//SeedBase seed = new SeedBase (gameObject);
		Seed seed = new Seed ();
		seed.codes = m_codes; 

		return seed;
	}
	public void loadData(SeedBase s)
	{
		Seed seed = (Seed)s;

		m_codes = seed.codes;

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
		codeEnumNames = System.Enum.GetNames (typeof(CodeName));

		collidingCursorBounds = new List<CursorBoundsCollider> (); 

		RefreshCursorType(); 
	}

	void Update()
	{
		// Update cursor type if it has changed (detected via lastCursorType)
		if (cursorType != lastCursorType)
		{
			RefreshCursorType(); 
		}
	}

	void RefreshCursorType()
	{
		lastCursorType = cursorType; 

		// The second parameter should be half of the cursor's size so that is is centered
		switch (cursorType)
		{
			case CursorType.GAMEPLAY:
			Cursor.SetCursor(cursorTexture_GAMEPLAY, new Vector2 (cursorTexture_GAMEPLAY.width/2, cursorTexture_GAMEPLAY.height/2), CursorMode.Auto);
				break; 
			case CursorType.UI_HOVER:
			Cursor.SetCursor(cursorTexture_UI_HOVER, new Vector2 (cursorTexture_UI_HOVER.width/2, cursorTexture_UI_HOVER.height/2), CursorMode.Auto);
				break;
			case CursorType.DEACTIVATED:
			Cursor.SetCursor(cursorTexture_DEACTIVATED, new Vector2 (cursorTexture_DEACTIVATED.width/2, cursorTexture_DEACTIVATED.height), CursorMode.Auto);
				break;
			default:
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			break;
		}
	}

	public static bool CursorInGameplayState()
	{
		if (inst.cursorType == CursorType.GAMEPLAY)
		{
			return true; 
		}
		return false; 
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
		if (inst.m_codes.Contains(codeName))
		{
			return false; 
		}

		inst.m_codes.Add(codeName); 

		if (inst.codesUpdated != null)
		{
			inst.codesUpdated(); 
		}

		return true; 
	}

	public static bool HasCode(CodeName codeName)
	{
		if (inst.m_codes.Contains(codeName))
		{
			return true; 
		}
		return false; 
	}

	/// <summary>
	/// Returns an int representing the highest code found. This is useful for UI calculations
	/// </summary>
	public static int HighestCodeFound()
	{
		int result = 0; 
		int i = 0; 

		foreach(CodeName codeN in Enum.GetValues(typeof(CodeName)))
		{
			i++; 
			if (inst.m_codes.Contains(codeN))
			{
				result = i; 
			}
		}
		return result; 
	}

	public static int NumCodesFound()
	{
		return inst.m_codes.Count; 
	}


	/*
	 *  Cursor functionality
	 */

	public void AddCursorBound(CursorBoundsCollider boundCol)
	{
		if (collidingCursorBounds.Contains(boundCol))
		{
			return; 
		}

		collidingCursorBounds.Add(boundCol); 
		OnCursorBoundsUpdated();
	}

	public void RemoveCursorBound(CursorBoundsCollider boundCol)
	{
		if (!collidingCursorBounds.Contains(boundCol))
		{
			return; 
		}
		collidingCursorBounds.Remove(boundCol); 
		OnCursorBoundsUpdated(); 
	}

	public void OnCursorBoundsUpdated()
	{
		if (lockCursorType)
		{
			return; 
		}

		if (collidingCursorBounds.Count == 0)
		{
			cursorType = CursorType.GAMEPLAY;
		}
		else
		{
			cursorType = CursorType.UI_HOVER; 
		}
	}

}
