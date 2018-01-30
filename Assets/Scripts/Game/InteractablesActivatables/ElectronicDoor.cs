using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicDoor : MonoBehaviour, IActivatable
{
	[Tooltip("Shows if the door is open or not.")]
	[SerializeField]
	public bool _isOpen = false;

	private Animator anim;

	// Use this for initialization
	void Start () 
	{
		//TODO: get animator
	}

	/// <summary>
	/// Toggles the door's state and returns it.
	/// </summary>
	public bool onActivate()
	{
		_isOpen = !_isOpen;
		//TODO: Trigger animator
		return _isOpen;
	}

	/// <summary>
	/// Sets the door's state and returns it.
	/// </summary>
	public bool onActivate(bool state)
	{
		_isOpen = state;
		//TODO: Trigger animator
		return state;
	}
}
