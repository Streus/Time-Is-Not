using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherIndicator : MonoBehaviour 
{
	Transform moveParent; 
	Vector3 offsetFromMoveParent; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Overlap circle all 
		// Check to see if any colliders are a moving platform (layer or tag or script)
		// Save a transform ref to the moving platform
		// Vector2 offset
		// Update position of point every frame to be moving platform position + offset
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x); 

		if (moveParent == null)
		{
			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].CompareTag("MovingPlatform"))
				{
					moveParent = hits[i].transform; 
					offsetFromMoveParent = transform.position - moveParent.transform.position; 
				}
			}
		}
		else
		{
			transform.position = moveParent.position + offsetFromMoveParent; 
		}
	}

	// Called during the load process to update the position of any tether indicators with moveParents, whose RegisteredObjects have just sowed new data
	public void UpdatePosition()
	{
		if (moveParent != null)
		{
			transform.position = moveParent.position + offsetFromMoveParent; 
		}
	}
}
