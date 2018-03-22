using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTrigger : MonoBehaviour, IActivatable
{
	[SerializeField]
	private float timerDurationSeconds;

	/// <summary>
	/// Toggles the laser's state and returns it.
	/// </summary>
	public bool onActivate()
	{
		GameManager.inst.StartEndTimer (timerDurationSeconds);
		return true;
	}

	/// <summary>
	/// Sets the laser's state and returns it.
	/// </summary>
	public bool onActivate(bool state)
	{
		GameManager.inst.StartEndTimer (timerDurationSeconds);
		return true;
	}


}
