using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltdownController : MonoBehaviour, IActivatable
{
	[SerializeField]
	private int _state;

	public bool onActivate()
	{
		_state++;
		return true;
	}

	public bool onActivate (bool state)
	{
		_state++;
		return true;
	}

	public int State
	{
		get{return _state;}
	}
}
