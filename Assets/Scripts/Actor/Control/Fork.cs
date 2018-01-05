using UnityEngine;

/// <summary>
/// A logic check used by a state machine to determine the next state.
/// </summary>
public abstract class Fork : ScriptableObject
{
	public abstract bool check (Controller c);
}
