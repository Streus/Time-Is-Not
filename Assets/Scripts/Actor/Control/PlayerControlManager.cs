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
	ZOOM_OUT
};

public class PlayerControlManager : Singleton<PlayerControlManager>
{
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


	/*
	 * Get key functions
	 */ 

	public static bool GetKey(ControlInput inputType)
	{
		KeyCode[] inputKeys = GetKeyCodesFor(inputType); 

		if (Input.GetKey(inputKeys[0]) || Input.GetKey(inputKeys[1]))
		{
			return true; 
		}

		return false; 

	}

	public static bool GetKeyDown(ControlInput inputType)
	{
		KeyCode[] inputKeys = GetKeyCodesFor(inputType); 

		if (Input.GetKeyDown(inputKeys[0]) || Input.GetKey(inputKeys[1]))
		{
			return true; 
		}

		return false; 
	}

	public static bool GetKeyUp(ControlInput inputType)
	{
		KeyCode[] inputKeys = GetKeyCodesFor(inputType); 

		if (Input.GetKeyUp(inputKeys[0]) || Input.GetKey(inputKeys[1]))
		{
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
			default:
				return null; 
		}
	}
}
