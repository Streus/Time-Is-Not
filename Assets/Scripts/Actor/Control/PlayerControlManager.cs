using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
	PAUSE_MENU,
	REMOVAL
};

[CreateAssetMenu(menuName = "Controls/Player Control Manager")]
[ExecuteInEditMode]
public class PlayerControlManager : ScriptableObject
{
	#region STATIC_VARS

	public static readonly int ciLength = Enum.GetNames (typeof(ControlInput)).Length;

	private static PlayerControlManager primary;
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

	public void OnEnable()
	{
		if (isPrimary())
			Debug.Log ("This is the primary PCM.");
	}

	public void Reset()
	{
		Awake();
	}

	public void Awake()
	{
		Debug.Log("ci length: " + ciLength); 

		if (primary == null)
		{
			setAsPrimary ();
		}
		else
		{
			Debug.LogWarning ("There is already a PlayerControlManager somewhere else.");
			#if UNITY_EDITOR
			UnityEditor.EditorGUIUtility.PingObject(primary);
			#endif
		}

		setCount = 1;
		currentSet = 0;
		bindings = new KeyCode[ciLength * setCount];
		bindings [0] = KeyCode.W;
		bindings [1] = KeyCode.S;
		bindings [2] = KeyCode.A;
		bindings [3] = KeyCode.D;
		bindings [4] = KeyCode.E;
		bindings [5] = KeyCode.Space;
		bindings [6] = KeyCode.LeftShift;
		bindings [7] = KeyCode.Mouse0;
		bindings [8] = KeyCode.Mouse1;
		bindings [9] = KeyCode.Q;
		bindings [10] = KeyCode.Escape;
		bindings [11] = KeyCode.R;

		setNames = new string[setCount];
		setNames [0] = "DEFAULT";
	}

	#region GETTERS_SETTERS

	public bool isPrimary()
	{
		return primary == this;
	}
	public void setAsPrimary()
	{
		primary = this;
	}

	public int getSetCount()
	{
		return setCount;
	}
	public void setSetCount(int val)
	{
		if (val != setCount)
		{
			//resize bindings
			KeyCode[] newBin = new KeyCode[val * ciLength];
			for (int i = 0, c = Mathf.Min(newBin.Length, bindings.Length); i < c; i++)
				newBin [i] = bindings [i];
			bindings = newBin;

			//resize set names
			string[] newNam = new string[val];
			for (int i = 0, c = Mathf.Min (newNam.Length, setNames.Length); i < c; i++)
				newNam [i] = setNames [i];
			setNames = newNam;
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
	public void setCurrentSet(string name)
	{
		for (int i = 0; i < setNames.Length; i++)
		{
			if (setNames [i] == name)
				setCurrentSet (i);
		}
		throw new ArgumentException (name + " is not currently a managed bindings set.");
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
		if (index < 0 || index >= bindings.Length)
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

	public KeyCode getBinding(ControlInput input)
	{
		return bindings [(int)input + (currentSet * ciLength)];
	}
	#endregion

	/// <summary>
	/// Similar to Input.GetKey(), but takes ControlInput parameter instead of KeyCode, which can map to multiple buttons as defined in the PlayerControlManager's Inspector
	/// </summary>
	/// <returns><c>true</c>, if key is held, <c>false</c> otherwise.</returns>
	/// <param name="inputType">ControlInput enum for a type of input action</param>
	public static bool GetKey(ControlInput inputType)
	{
		return Input.GetKey (primary.getBinding(inputType));
	}

	/// <summary>
	/// Similar to Input.GetKeyDown(), but takes ControlInput parameter instead of Keycode, which can map to multiple buttons as defined in the PlayerControlManager's Inspector
	/// </summary>
	/// <returns><c>true</c>, if key is pressed, <c>false</c> otherwise.</returns>
	/// <param name="inputType">ControlInput enum for a type of input action</param>
	public static bool GetKeyDown(ControlInput inputType)
	{
		return Input.GetKeyDown (primary.getBinding(inputType));
	}

	/// <summary>
	/// Similar to Input.GetKeyUp(), but takes ControlInput parameter instead of Keycode, which can map to multiple buttons as defined in the PlayerControlManager's Inspector
	/// </summary>
	/// <returns><c>true</c>, if key is released, <c>false</c> otherwise.</returns>
	/// <param name="inputType">ControlInput enum for a type of input action</param>
	public static bool GetKeyUp(ControlInput inputType)
	{
		return Input.GetKeyUp (primary.getBinding(inputType));
	}
	#endregion
}
