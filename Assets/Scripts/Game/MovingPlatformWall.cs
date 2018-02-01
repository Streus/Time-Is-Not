using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformWall : MonoBehaviour 
{
	//is the wall enabled or not
	private bool isEnabled = true;

	private LayerMask _pitLayer;

	//radius of the pit check
	private float checkSize = 0.1f;

	// Use this for initialization
	void Start () 
	{
		
	}

	// Is called every fixed framerate frame
	void FixedUpdate()
	{
		bool check = CheckForPit ();
		if(isEnabled != check)
		{
			isEnabled = check;

			if (!isEnabled)
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			else
				gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		}
	}

	/// <summary>
	/// Checks for a pit under the wall, if none is found, the wall opens.
	/// </summary>
	bool CheckForPit()
	{
		LayerMask mask = 1 << LayerMask.NameToLayer("Pits");
		Collider2D colHit = Physics2D.OverlapCircle(transform.position, checkSize, mask);
		return (colHit != null);
	}
}
