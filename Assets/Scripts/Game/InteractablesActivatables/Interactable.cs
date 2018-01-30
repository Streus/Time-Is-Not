using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour 
{
	[Tooltip("Can the interactable be interacted with?")]
	[SerializeField]
	private bool _enabled = true;

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private IActivatable[] _activatables;

	/// <summary>
	/// Trigger the Interactable.
	/// </summary>
	public abstract void onInteract ();

	/// <summary>
	/// Triggers the activatables.
	/// </summary>
	public void toggleActivatables()
	{
		foreach(IActivatable activatable in _activatables)
		{
			activatable.onActivate ();
		}
	}

	/// <summary>
	/// Triggers a specific activatable.
	/// </summary>
	public void ToggleActivatable(int index)
	{
		_activatables [index].onActivate ();
	}

	/// <summary>
	/// Sets the activatables.
	/// </summary>
	public void toggleActivatables(bool state)
	{
		foreach(IActivatable activatable in _activatables)
		{
			activatable.onActivate (state);
		}
	}

	/// <summary>
	/// Sets a specific activatable.
	/// </summary>
	public void ToggleActivatable(int index, bool state)
	{
		_activatables [index].onActivate (state);
	}

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
