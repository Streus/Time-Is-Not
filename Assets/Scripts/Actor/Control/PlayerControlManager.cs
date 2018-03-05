using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlInput
{
	UP,
	DOWN,
	LEFT,
	RIGHT,
	INTERACT,
	DROP_TETHER,
	TETHER_MENU,
	DASH,
	FIRE_STASIS,
	ZOOM_OUT,
	PAUSE_MENU
};

public class PlayerControlManager : Singleton<PlayerControlManager>
{
	/*
	 * Right hand controls
	 */ 

    [Header("Right Hand Controls: This controls what key the player presses to move the player up")]
    [SerializeField] private KeyCode rH_Up;
    public static KeyCode RH_Up
    {
        get
        {
            return inst.rH_Up;
        }
    }
    [Header("Right Hand Controls: This controls what key the player presses to move the player down")]
    [SerializeField] private KeyCode rH_Down;
    public static KeyCode RH_Down
    {
        get
        {
            return inst.rH_Down;
        }
    }
    [Header("Right Hand Controls: This controls what key the player presses to move the player left")]
    [SerializeField] private KeyCode rH_Left;
    public static KeyCode RH_Left
    {
        get
        {
            return inst.rH_Left;
        }
    }
    [Header("Right Hand Controls: This controls what key the player presses to move the player right")]
    [SerializeField] private KeyCode rH_Right;
    public static KeyCode RH_Right
    {
        get
        {
            return inst.rH_Right;
        }
    }
    [Header("Right Hand Controls: This controls what key the player presses to interact with buttons and doors")]
    [SerializeField] private KeyCode rH_Interact;
    public static KeyCode RH_Interact
    {
        get
        {
            return inst.rH_Interact;
        }
    }
    [Header("Right Hand Controls: This controls what key the player presses to place a tether point")]
    [SerializeField] private KeyCode rH_DropTether;
    public static KeyCode RH_DropTether
    {
        get
        {
            return inst.rH_DropTether;
        }
    }
    [Header("Right Hand Controls: This controls what key the player presses to open the tether menu")]
    [SerializeField] private KeyCode rH_TetherMenu;
    public static KeyCode RH_TetherMenu
    {
        get
        {
            return inst.rH_TetherMenu;
        }
    }
    [Header("Right Hand Controls: This controls what key the player presses to dash")]
    [SerializeField] private KeyCode rH_Dash;
    public static KeyCode RH_Dash
    {
        get
        {
            return inst.rH_Dash;
        }
    }
    [Header("Right Hand Controls: This controls what key the player presses to shoot a stasis bubble")]
    [SerializeField] private KeyCode rH_FireStasis;
    public static KeyCode RH_FireStasis
    {
        get
        {
            return inst.rH_FireStasis;
        }
    }
    [Header("Right Hand Controls: This controls what key the player presses to zoom the camera out")]
    [SerializeField] private KeyCode rH_ZoomOut;
    public static KeyCode RH_ZoomOut
    {
        get
        {
            return inst.rH_ZoomOut;
        }
    }
	[Header("Right Hand Controls: This controls what key the player presses to reveal/hide the pause menu")]
	[SerializeField] private KeyCode rH_PauseMenu;
	public static KeyCode RH_PauseMenu
	{
		get
		{
			return inst.rH_PauseMenu;
		}
	}

	/*
	 * Left hand controls
	 */ 

    [Header("Left Hand Controls: This controls what key the player presses to move the player up")]
    [SerializeField] private KeyCode lH_Up;
    public static KeyCode LH_Up
    {
        get
        {
            return inst.lH_Up;
        }
    }
    [Header("Left Hand Controls: This controls what key the player presses to move the player down")]
    [SerializeField] private KeyCode lH_Down;
    public static KeyCode LH_Down
    {
        get
        {
            return inst.lH_Down;
        }
    }
    [Header("Left Hand Controls: This controls what key the player presses to move the player left")]
    [SerializeField] private KeyCode lH_Left;
    public static KeyCode LH_Left
    {
        get
        {
            return inst.lH_Left;
        }
    }
    [Header("Left Hand Controls: This controls what key the player presses to move the player right")]
    [SerializeField] private KeyCode lH_Right;
    public static KeyCode LH_Right
    {
        get
        {
            return inst.lH_Right;
        }
    }
    [Header("Left Hand Controls: This controls what key the player presses to interact with buttons and doors")]
    [SerializeField] private KeyCode lH_Interact;
    public static KeyCode LH_Interact
    {
        get
        {
            return inst.lH_Interact;
        }
    }
    [Header("Left Hand Controls: This controls what key the player presses to place a tether point")]
    [SerializeField] private KeyCode lH_DropTether;
    public static KeyCode LH_DropTether
    {
        get
        {
            return inst.lH_DropTether;
        }
    }
    [Header("Left Hand Controls: This controls what key the player presses to open the tether menu")]
    [SerializeField] private KeyCode lH_TetherMenu;
    public static KeyCode LH_TetherMenu
    {
        get
        {
            return inst.lH_TetherMenu;
        }
    }
    [Header("Left Hand Controls: This controls what key the player presses to dash")]
    [SerializeField] private KeyCode lH_Dash;
    public static KeyCode LH_Dash
    {
        get
        {
            return inst.lH_Dash;
        }
    }
    [Header("Left Hand Controls: This controls what key the player presses to shoot a stasis bubble")]
    [SerializeField] private KeyCode lH_FireStasis;
    public static KeyCode LH_FireStasis
    {
        get
        {
            return inst.lH_FireStasis;
        }
    }
    [Header("Left Hand Controls: This controls what key the player presses to zoom the camera out")]
    [SerializeField] private KeyCode lH_ZoomOut;
    public static KeyCode LH_ZoomOut
    {
        get
        {
            return inst.lH_ZoomOut;
        }
    }
	[Header("Left Hand Controls: This controls what key the player presses to reveal/hide the pause menu")]
	[SerializeField] private KeyCode lH_PauseMenu;
	public static KeyCode LH_PauseMenu
	{
		get
		{
			return inst.lH_PauseMenu;
		}
	}


	/*
	 * Get key functions
	 */ 

	/// <summary>
	/// Similar to Input.GetKey(), but takes ControlInput parameter instead of KeyCode, which can map to multiple buttons as defined in the PlayerControlManager's Inspector
	/// </summary>
	/// <returns><c>true</c>, if key is held, <c>false</c> otherwise.</returns>
	/// <param name="inputType">ControlInput enum for a type of input action</param>
	public static bool GetKey(ControlInput inputType)
	{
		KeyCode[] inputKeys = GetKeyCodesFor(inputType); 

		// Iterate through each key mapped to an action, and return true if any of them are currently being pressed
		for (int i = 0; i < inputKeys.Length; i++)
		{
			if (Input.GetKey(inputKeys[i]))
				return true; 
		}

		return false; 

	}

	/// <summary>
	/// Similar to Input.GetKeyDown(), but takes ControlInput parameter instead of Keycode, which can map to multiple buttons as defined in the PlayerControlManager's Inspector
	/// </summary>
	/// <returns><c>true</c>, if key is pressed, <c>false</c> otherwise.</returns>
	/// <param name="inputType">ControlInput enum for a type of input action</param>
	public static bool GetKeyDown(ControlInput inputType)
	{
		KeyCode[] inputKeys = GetKeyCodesFor(inputType); 

		// Iterate through each key mapped to an action, and return true if any of them have just been pressed
		for (int i = 0; i < inputKeys.Length; i++)
		{
			if (Input.GetKeyDown(inputKeys[i]))
				return true; 
		}

		return false; 
	}

	/// <summary>
	/// Similar to Input.GetKeyUp(), but takes ControlInput parameter instead of Keycode, which can map to multiple buttons as defined in the PlayerControlManager's Inspector
	/// </summary>
	/// <returns><c>true</c>, if key is released, <c>false</c> otherwise.</returns>
	/// <param name="inputType">ControlInput enum for a type of input action</param>
	public static bool GetKeyUp(ControlInput inputType)
	{
		KeyCode[] inputKeys = GetKeyCodesFor(inputType); 

		// Iterate through each key mapped to an action, and return true if any of them have just been released
		for (int i = 0; i < inputKeys.Length; i++)
		{
			if (Input.GetKeyUp(inputKeys[i]))
				return true; 
		}

		return false; 
	}

	static KeyCode[] GetKeyCodesFor(ControlInput inputType)
	{
		switch (inputType)
		{
			case ControlInput.UP:
				return new KeyCode[]{RH_Up, LH_Up}; 
			case ControlInput.DOWN:
				return new KeyCode[]{RH_Down, LH_Down};
			case ControlInput.LEFT:
				return new KeyCode[]{RH_Left, LH_Left};
			case ControlInput.RIGHT:
				return new KeyCode[]{RH_Right, LH_Right};
			case ControlInput.INTERACT:
				return new KeyCode[]{RH_Interact, LH_Interact};
			case ControlInput.DROP_TETHER:
				return new KeyCode[]{RH_DropTether, LH_DropTether};
			case ControlInput.TETHER_MENU:
				return new KeyCode[]{RH_TetherMenu, LH_TetherMenu};
			case ControlInput.DASH:
				return new KeyCode[]{RH_Dash, LH_Dash};
			case ControlInput.FIRE_STASIS:
				return new KeyCode[]{RH_FireStasis, LH_FireStasis};
			case ControlInput.ZOOM_OUT:
				return new KeyCode[]{RH_ZoomOut, LH_ZoomOut};
			case ControlInput.PAUSE_MENU:
				return new KeyCode[]{RH_PauseMenu, LH_PauseMenu};
			default:
				return null; 
		}
	}
}
