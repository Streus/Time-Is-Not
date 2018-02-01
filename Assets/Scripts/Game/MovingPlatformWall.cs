using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformWall : MonoBehaviour 
{
	//is the wall enabled or not
	private bool isEnabled = true;

	//radius of the pit check
	[SerializeField]
	private float checkSize = 0.2f;

	// Use this for initialization
	void Start () {
		
	}

	// Is called every fixed framerate frame
	void FixedUpdate()
	{
		
	}

	/// <summary>
	/// Checks for a pit under the wall, if none is found, the wall opens.
	/// </summary>
	void CheckForPit()
	{
		Physics2D.OverlapCircle(transform.position, checkSize);
	}
}
