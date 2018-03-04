using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherIndicator : MonoBehaviour 
{
	[HideInInspector] public int tetherIndex; 

	Transform moveParent; 
	Vector3 offsetFromMoveParent; 

	public bool allowRemoval; 
	public float removeRadius = 1; 
	bool playerInRemoveRadius; 

	public GameObject tetherPointSprite; 
	public GameObject removePrompt; 

	// Use this for initialization
	void Start () 
	{
		if (removePrompt != null)
			removePrompt.SetActive(false); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Overlap circle all 
		// Check to see if any colliders are a moving platform (layer or tag or script)
		// Save a transform ref to the moving platform
		// Vector2 offset
		// Update position of point every frame to be moving platform position + offset
		Collider2D[] hits = Physics2D.OverlapCircleAll(tetherPointSprite.transform.position, transform.localScale.x); 

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

		playerInRemoveRadius = false; 

		if (allowRemoval)
		{
			hits = Physics2D.OverlapCircleAll(tetherPointSprite.transform.position, removeRadius); 

			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].CompareTag("Player"))
				{
					playerInRemoveRadius = true; 
				}
			}
		}

		// Don't allow the remove prompt/action for the first tether point (when tetherIndex == 0)
		if (playerInRemoveRadius && tetherIndex != 0)
		{
			// Display the remove prompt
			if (removePrompt != null)
				removePrompt.SetActive(true); 

			// Test for remove action
			// TODO- might want to have this be a hold action
			if (PlayerControlManager.GetKeyDown(ControlInput.INTERACT))
			{
				TetherManager.inst.RemoveTetherPoint(tetherIndex); 
			}
		}
		else
		{
			if (removePrompt != null)
				removePrompt.SetActive(false); 
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
