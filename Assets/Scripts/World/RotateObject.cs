using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour, IActivatable
{
	[Tooltip("Which direction to rotate.")]
	[SerializeField]
	private bool _clockwise = true;

	[Tooltip("Is the object moving?")]
	[SerializeField]
	private bool _active = true;

	[Tooltip("How fast the object turns.")]
	[SerializeField]
	private float _turnSpeed = 1;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Rotate ();
	}

	void Rotate()
	{
		if (!_active)
			return;
		int turnDirection = 1;
		if (_clockwise)
			turnDirection = -1;
			
		transform.Rotate (Vector3.forward * _turnSpeed * turnDirection * Time.deltaTime);
	}

	/// <summary>
	/// Toggles the object's state and returns it.
	/// </summary>
	public bool onActivate()
	{
		_active = !_active;
		return _active;
	}

	public bool onActivate (bool state)
	{
		_active = state;
		return _active;
	}
}
