using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour 
{
	[Tooltip("Can the interactable be interacted with?")]
	[SerializeField]
	private bool _enabled = true;

	/// <summary>
	/// Trigger the Interactable.
	/// </summary>
	public abstract void onInteract ();

	/// <summary>
	/// Enables the interactable.
	/// </summary>
	public void enable()
	{
		_enabled = true;
	}

	/// <summary>
	/// Disables the interactable.
	/// </summary>
	public void disable()
	{
		_enabled = false;
	}

	/// <summary>
	/// Returns if the interactable is enabled or not.
	/// </summary>
	public bool isEnabled()
	{
		return _enabled;
	}

}
