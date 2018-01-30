using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable 
{
	/// <summary>
	/// Toggle the Activatable's state.
	/// </summary>
	bool onActivate();

	/// <summary>
	/// Set the Activatable's state.
	/// </summary>
	bool onActivate (bool state);
}
