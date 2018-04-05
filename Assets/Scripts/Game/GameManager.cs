using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.SceneManagement; 

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

public enum PauseType
{
	// ## Outside pause graph ##
	// The game is not paused
	NONE = 0x0,

	// ## Top level ##
	// The game is completely paused via a pause menu that overrides all other types of pauses
	GAME = 0x1,

	// ## Bottom level ##
	// The game is performing a transition involving death and/or the time tether. This is only exited when the transition is finished
	// Fully pauses world entities
	TETHER_TRANSITION = 0x2,

	// The tether selection menu is up
	TETHER_MENU = 0x4,

	// The player has zoomed out, which pauses player movement but NOT the actions of most entities.
	// If the player dies while zoomed out, the game must first unpause before then setting the pause type to TETHER_TRANSITION
	// Does not fully pause world entities
	ZOOM = 0x8,

	// The game is in some other pause state relating to a cutscene.
	// Fully pauses world entities
	CUTSCENE = 0x10
}

[RequireComponent(typeof(RegisteredObject))]
public class GameManager : Singleton<GameManager> , ISavable
{
	[Tooltip("Empty gameobject containing all security doors in the level that should be tripped at once")]
	public GameObject securityDoors;

	public PlayerControlManager controlManager;

	// Pause functionality
	//bool paused; 
	//bool pauseLock; 

	PauseType m_pauseType; 
	public PauseType pauseType
	{
		get{
			return m_pauseType; 
		}
	}

	// Holds a backup of the pause type if the player brings up a GAME pause while in another pause state
	PauseType prevPauseType; 

	// Inspector display
	[SerializeField] PauseType pauseTypeDisplay;
	[SerializeField] PauseType prevPauseTypeDisplay; 

	// Returns true if the current pause state means that all entities in the world should be paused
	public static bool isPaused()
	{
		if (inst.pauseType == PauseType.NONE || inst.pauseType == PauseType.ZOOM)
		{
			return false;
		}
		return true; 
	}

	// Bitmasked pause variables
	public int lowerPauseStates
	{
		get{
			return (int)PauseType.CUTSCENE & (int)PauseType.TETHER_TRANSITION & (int)PauseType.TETHER_MENU & (int)PauseType.ZOOM; 
		}
	}


	// Ability setup
	// These variables can be edited in the Inspector via SceneSettings
	[HideInInspector] public bool canUseStasis; 
	[HideInInspector] public bool canUseDash; 

	// Death functionality
	bool isDead; 

	// End level timer functionality
	bool m_useEndTimer; 
	public bool useEndTimer
	{
		get{
			return m_useEndTimer; 
		}
	}

	float endTimerLength; 

	float m_endTimer;
	public float endTimer
	{
		get{
			return m_endTimer; 
		}
	}

	public string timerString
	{
		get{
			float minutes = Mathf.Floor(m_endTimer / 60);
			float seconds = (m_endTimer % 60);
			return minutes.ToString("00") + ":" + seconds.ToString("00"); 
		}
	}

	// Actions
	public event PauseStateToggled pauseTypeToggled; 
	//public event StateToggled pauseLockedToggled; 
	public event CodesUpdated codesUpdated; 

	// Delegates
	public delegate void PauseStateToggled(PauseType type); 
	public delegate void CodesUpdated (); 

	// Reference variables
	[SerializeField] private GameObject playerObj; 
	private Player playerScript; 
	private Animator playerAnimator; 
	[SerializeField] private CameraManager cameraManager; 

	// Savable data
	[SerializeField] List<CodeName> m_codes; 
	public List<CodeName> codes
	{
		get{
			return m_codes; 
		}
	}

	private string[] codeEnumNames; 



	// --- ISavable Methods ---
	public SeedBase saveData()
	{
		//SeedBase seed = new SeedBase (gameObject);
		Seed seed = new Seed ();

		//seed.codes = m_codes; 
		seed.endTimer = endTimer;
		seed.endTimerActive = m_useEndTimer; 

		return seed;
	}
	public void loadData(SeedBase s)
	{
		Seed seed = (Seed)s;

		//m_codes = seed.codes;
		m_endTimer = seed.endTimer;
		m_useEndTimer = seed.endTimerActive; 

		if (isDead)
			isDead = false; 
	}

	public class Seed : SeedBase
	{
		// Define all the extra variables that should be saved here

		// Why is there here? It doesn't appear to work
		public List<CodeName> codes; 

		public float endTimer; 
		public bool endTimerActive; 
	}

	// MonoBehaviors
	public void Awake()
	{
		if (SceneSetup.inst != null)
		{
			canUseStasis = SceneSetup.inst.canUseStasis; 
			canUseDash = SceneSetup.inst.canUseDash; 
			endTimerLength = SceneSetup.inst.endTimerLength; 
			m_useEndTimer = SceneSetup.inst.useEndTimer; 
			m_endTimer = endTimerLength; 
		}

		if (playerObj != null)
		{
			playerObj.GetComponent<Entity> ().died += killPlayer;
			playerScript = playerObj.GetComponent<Player>(); 
			playerAnimator = playerObj.GetComponent<Animator>(); 
		}
		codeEnumNames = System.Enum.GetNames (typeof(CodeName));
        
		//make sure there's a TransitionBuddy
		TransitionBuddy.getInstance ();

		controlManager.setAsPrimary ();
    }

    void Update()
	{ 
		if (m_useEndTimer && !isPaused())
		{
			m_endTimer -= Time.deltaTime; 
			if (m_endTimer <= 0)
			{
				m_endTimer = 0; 
				killPlayer(); 
			}

			//endTimerText.enabled = true; 

			float minutes = Mathf.Floor(m_endTimer / 60);
			float seconds = (m_endTimer % 60);

			//endTimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00"); 

			//Debug.Log("m_endTimer = " + m_endTimer); 
		}
		else
		{
			//Debug.Log("No timer?"); 
			//endTimerText.enabled = false; 
		}

		#if UNITY_EDITOR

		pauseTypeDisplay = m_pauseType;
		prevPauseTypeDisplay = prevPauseType; 

		#endif
	}

	public void StartEndTimer()
	{
		StartEndTimer(endTimerLength); 
	}

	public void StartEndTimer(float length)
	{
		m_useEndTimer = true; 
		m_endTimer = length; 
	}

	/*
	 * Pause functionality
	 */ 

	/*
	/// <summary>
	/// Sets whether the game is paused
	/// </summary>
	/// <returns><c>true</c>, if pause's state could be changed <c>false</c> otherwise.</returns>
	/// <param name="state">Whether to pause or resume the game</param>
	public static bool setPause(bool state)
	{
		// Return if locked or the state isn't actually changing
		if (inst.pauseLock || inst.paused == state)
		{
			return false; 
		}

		inst.paused = state; 

		if (inst.pauseToggled != null)
		{
			inst.pauseToggled(state); 
		}
		return true; 
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
	*/

	/// <summary>
	/// Tells the GameManager to enter (not exit) a specific pause state. Returns true if successful
	/// </summary>
	/// <param name="newPauseType">New pause type.</param>
	public bool EnterPauseState(PauseType newPauseType)
	{
		// Pause states have two levels
		// The lower level pause states are CUTSCENE, TETHER_TRANSITION, and ZOOM
		// The top level pause state is a full GAME pause, which can be accessed independently or on top of a lower level
		// If entering a top level pause through a lower level, when exiting the state machine must pass back down through the lower level 

		// Check for redunancy
		if (pauseType == newPauseType)
		{
			Debug.LogWarning("Trying to change GameManager pause type to the same pause type"); 
			return false; 
		}

		// In default, dashing, pushing

		// Don't allow entering a pause state of NONE (that would be exiting!)
		if (newPauseType == PauseType.NONE)
		{
			Debug.LogError("EnterPauseState cannot accept PauseType.NONE as a parameter - it would be considered exiting"); 
			return false; 
		}

		// Reset the backup pause state
		prevPauseType = PauseType.NONE; 

		// If the current pause type is NONE, allow entry into any pause state
		if (pauseType == PauseType.NONE)
		{
			m_pauseType = newPauseType; 
			if (pauseTypeToggled != null)
			{
				pauseTypeToggled(m_pauseType); 
			}
			return true; 
		}
		// If the current pause type is one of the lower level states (below full game pause), elevate to game paused and save the previous pause state
		//else if ((int)pauseType == lowerPauseStates && newPauseType == PauseType.GAME)
		else if (IsLowerPauseType(pauseType) && newPauseType == PauseType.GAME)
		{
			// Back up the current pause type so it can be restored when unpausing
			prevPauseType = pauseType; 

			m_pauseType = newPauseType;
			if (pauseTypeToggled != null)
			{
				pauseTypeToggled(m_pauseType); 
			}
			return true; 
		}

		return false; 
	}

	/// <summary>
	/// Exits the current pause state, moving down a level to either NONE or the backup prevPauseType
	/// </summary>
	public bool ExitPauseState()
	{
		// Throw a warning if trying to exit while not even paused
		if (pauseType == PauseType.NONE)
		{
			Debug.LogWarning("ExitPauseState cannot exit a PauseType of NONE"); 
			return false; 
		}
		// Allow fully exiting pause if in one of the lower level states
		//else if ((int)pauseType == lowerPauseStates)
		else if (IsLowerPauseType(pauseType))
		{
			m_pauseType = PauseType.NONE; 
			if (pauseTypeToggled != null)
			{
				pauseTypeToggled(m_pauseType); 
			}
			return true; 
		}
		// When exiting a top level game pause, check whether to return to NONE or a backup pause state
		else if (pauseType == PauseType.GAME)
		{
			Debug.Log("pauseType == GAME"); 

			// If a backup of the previous pause state was created, load the backup
			//if ((int)prevPauseType == lowerPauseStates)
			if (IsLowerPauseType(prevPauseType))
			{
				m_pauseType = prevPauseType; 
				Debug.Log("Set pause state to prevPauseState of " + prevPauseType); 
			}
			else
			{
				Debug.Log("prevPauseType of " + prevPauseType + " is not a lower pause type"); 
				m_pauseType = PauseType.NONE; 
			}

			// Reset the backup pause state
			prevPauseType = PauseType.NONE; 

			if (pauseTypeToggled != null)
			{
				pauseTypeToggled(m_pauseType); 
			}
			return true; 
		}
		return false; 
	}

	public bool IsLowerPauseType(PauseType testType)
	{
		int check = (int)PauseType.CUTSCENE | (int)PauseType.TETHER_MENU | (int)PauseType.TETHER_TRANSITION | (int)PauseType.ZOOM; 
		//Debug.Log(System.Convert.ToString(check, 2).PadLeft(32, '0')); //DEBUG
		//Debug.Log(System.Convert.ToString((int)testType, 2).PadLeft(32, '0')); //DEBUG

		//if (testType == PauseType.CUTSCENE || testType == PauseType.TETHER_MENU || testType == PauseType.TETHER_TRANSITION || testType == PauseType.ZOOM)
		if (((int)testType & check) != 0)
		{
			return true; 
		}
		return false; 
	}

	public bool IsWorldPauseType(PauseType testType)
	{
		int check = (int)PauseType.CUTSCENE | (int)PauseType.TETHER_MENU | (int)PauseType.TETHER_TRANSITION | (int)PauseType.GAME; 
	
		if (((int)testType & check) != 0)
		{
			return true; 
		}
		return false; 
	}

	// Usage: int check = PauseType.a | PauseType.b
	public static bool CheckPause(int check)
	{
		if (inst != null)
			return ((int)inst.m_pauseType & check) != 0; 
		return false; 
	}



	/*
	 * Player information getters
	 */ 

	public static bool isPlayerDead()
	{
		return inst.isDead; 
	}

	public static void killPlayer()
	{
		inst.isDead = true; 
		//inst.paused = true;

		// Set the pause state to TETHER_MENU
		inst.EnterPauseState(PauseType.TETHER_MENU); 
	}

	public static GameObject GetPlayer()
	{
		return inst.playerObj; 
	}

	public static Animator GetPlayerAnimator()
	{
		return inst.playerAnimator; 
	}

	public static int getPlayerMoveMask()
	{
		return inst.playerScript.moveMask.value; 
	}

	public static float getPlayerMaxJumpDist()
	{
		return inst.playerScript.getMaxJumpDist();
	}

	public static Vector3 getPlayerJumpTarget()
	{
		return inst.playerScript.getJumpTargetPos(); 
	}

	public static bool isPlayerDashing()
	{
		return inst.playerScript.dashing(); 
	}

	public static bool dashIsCharged()
	{
		if (inst.playerScript.getSelf().getAbility(1).cooldownPercentage() == 0)
		{
			return true;
		}
		return false; 
	}

	public static float dashChargePercNormalized()
	{
		return inst.playerScript.getSelf().getAbility(1).cooldownPercentage(); 
	}

	/*
	 * Camera information getters
	 */ 
		
	public static CameraManager GetCameraManager()
	{
		return inst.cameraManager; 
	} 

	public static bool CameraIsZoomedOut()
	{
		return inst.cameraManager.zoomState; 
	}

	/*
	 * Keycode functionality
	 */ 

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
	[System.Obsolete("Keycodes are no longer numbered")]
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
	 * Temp Scene loading functionality
	 */ 

	/*
	public static void LoadScene(string name)
	{
		if (SceneManager.GetSceneByName(name) != null)
		{
			SceneManager.LoadScene(name); 
		}
		else
		{
			Debug.LogError("Scene name " + name + " cannot be loaded"); 
		}
	}
	*/ 

}
