using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single state for use in the Controller state machine.
/// </summary>
[CreateAssetMenu(menuName = "AI/State")]
public sealed class State : ScriptableObject
{
	#region INSTANCE_VARS
	[Tooltip("Used for debugging purposes.")]
	[SerializeField]
	private Color color;

	[Tooltip("Behavior performed when this state is entered")]
	[SerializeField]
	private Action enterAction;

	[Tooltip("Behaviors performed on every update on this state.")]
	[SerializeField]
	private Action[] actions;

	[Tooltip("Behavior performed when this state is exited.")]
	[SerializeField]
	private Action exitAction;

	[Tooltip("A list of subsequent states to execute upon exiting this one," +
		" and the conditions by which the next state will be selected")]
	[SerializeField]
	private Path[] paths;
	#endregion

	#region INSTANCE_METHODS
	public void enter(Controller c)
	{
		if (enterAction != null)
			enterAction.perform (c);
	}

	public void update(Controller c)
	{
		int i;
		for (i = 0; i < actions.Length; i++)
			actions [i].perform (c);

		for (i = 0; i < paths.Length; i++)
		{
			if (paths [i].decision.check (c))
			{
				if (paths [i].success != null)
					c.setState (paths [i].success);
				else
					Debug.LogWarning ("Null Success State! Path: [" + paths[i].name + "] State: [" + name + "]");
			}
			else if (paths [i].failure != null)
				c.setState (paths [i].failure);
			else
				Debug.LogWarning ("Null Failure State! Path: [" + paths[i].name + "] State: [" + name + "]");
		}
	}

	public void exit(Controller c)
	{
		if (exitAction != null)
			exitAction.perform (c);
	}
	#endregion

	/// <summary>
	/// Used in the Controller state machine to determine the course taken between states.
	/// </summary>
	[System.Serializable]
	public class Path
	{
		public string name;
		public State success, failure;
		public Fork decision;
	}
}
