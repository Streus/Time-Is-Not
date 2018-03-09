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

[CreateAssetMenu(menuName = "Controls/Player Control Manager", fileName = "Primary")]
public class PlayerControlManager : ScriptableObject
{
	private const int CI_LENGTH = 11;

	#region STATIC_VARS

	private PlayerControlManager primary;
	#endregion

	#region STATIC_METHODS

	#endregion

	#region INSTANCE_VARS

	// The number of binding preset sets exist
	[SerializeField]
	private int setCount;

	// The currently active preset set
	[SerializeField]
	private int currentSet;

	// All the bindings for each control in each preset set
	[SerializeField]
	private KeyCode[] bindings;

	[SerializeField]
	private string[] setNames;
	#endregion

	#region INSTANCE_METHODS

	public void Awake()
	{
		if (primary == null)
			primary = this;

		setCount = 1;
		currentSet = 0;
		bindings = new KeyCode[CI_LENGTH * setCount]; //TODO unhardcode enum length
		setNames = new string[setCount];
		setNames [0] = "DEFAULT";
	}

	#region GETTERS_SETTERS

	public int getSetCount()
	{
		return setCount;
	}
	public void setSetCount(int val)
	{
		if (val != setCount)
		{
			KeyCode[] newArr = new KeyCode[val * CI_LENGTH];
			for (int i = 0; i < bindings.Length; i++)
				newArr [i] = bindings [i];
			bindings = newArr;
		}
		setCount = val;
	}

	public int getCurrentSet()
	{
		return currentSet;
	}
	public void setCurrentSet(int val)
	{
		if (val < 0)
			currentSet = 0;
		else if (val >= setCount)
			currentSet = setCount - 1;
		else
			currentSet = val;
	}

	public KeyCode getBinding(int index)
	{
		try
		{
			return bindings[index];
		}
		catch(System.IndexOutOfRangeException ioore)
		{
			Debug.LogException (ioore);
			return KeyCode.None;
		}
	}

	public void setBinding(KeyCode key, int index)
	{
		if (index < 0 || index >= setCount)
			throw new System.IndexOutOfRangeException (index + " is out of bounds!");

		bindings [index] = key;
	}

	public string getSetName(int index)
	{
		try
		{
			return setNames[index];
		}
		catch(System.IndexOutOfRangeException ioore)
		{
			Debug.LogException (ioore);
			return "";
		}
	}
	public void setSetName(string name, int index)
	{
		if (index < 0 || index >= setCount)
			throw new System.IndexOutOfRangeException (index + " is out of bounds!");

		setNames [index] = name;
	}

	public KeyCode getKey(ControlInput input)
	{
		return bindings [(int)input + (currentSet * CI_LENGTH)];
	}
	#endregion

	/// <summary>
	/// Similar to Input.GetKey(), but takes ControlInput parameter instead of KeyCode, which can map to multiple buttons as defined in the PlayerControlManager's Inspector
	/// </summary>
	/// <returns><c>true</c>, if key is held, <c>false</c> otherwise.</returns>
	/// <param name="inputType">ControlInput enum for a type of input action</param>
	public static bool GetKey(ControlInput inputType)
	{
		return false;
	}

	/// <summary>
	/// Similar to Input.GetKeyDown(), but takes ControlInput parameter instead of Keycode, which can map to multiple buttons as defined in the PlayerControlManager's Inspector
	/// </summary>
	/// <returns><c>true</c>, if key is pressed, <c>false</c> otherwise.</returns>
	/// <param name="inputType">ControlInput enum for a type of input action</param>
	public static bool GetKeyDown(ControlInput inputType)
	{
		return false;
	}

	/// <summary>
	/// Similar to Input.GetKeyUp(), but takes ControlInput parameter instead of Keycode, which can map to multiple buttons as defined in the PlayerControlManager's Inspector
	/// </summary>
	/// <returns><c>true</c>, if key is released, <c>false</c> otherwise.</returns>
	/// <param name="inputType">ControlInput enum for a type of input action</param>
	public static bool GetKeyUp(ControlInput inputType)
	{
		return false;
	}
	#endregion
}
