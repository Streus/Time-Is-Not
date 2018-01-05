using UnityEngine;

/// <summary>
/// A behavior preformed by a state machine on a Controller.
/// </summary>
public abstract class Action : ScriptableObject
{
	public abstract void perform(Controller c);
}
