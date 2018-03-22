using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TetherIndicator : MonoBehaviour 
{
	[HideInInspector] public int tetherIndex; 

	Transform moveParent; 
	Vector3 offsetFromMoveParent; 

	[Tooltip("If true, the player can walk up to a tether point, see a prompt icon, and press e to remove the tether point")] 
	public bool allowRadiusRemoval; 
	public float removeRadius = 1; 
	bool playerInRemoveRadius; 

	[Tooltip("If true, using an input (right click) removes a tether point")] 
	public bool allowKeyRemoval; 
	public Collider2D clickCollider; 

	public GameObject tetherPointSprite; 
	public GameObject removePrompt; 

	[Header("Drag in animation controllers")] 
	//public AnimatorContr firstTetherAnimator;
	//public Animator otherTetherAnimator; 
	public Animator tetherAnimator; 
	public Animator silverAnchorAnimator;
	public Animator goldAnchorAnimator;


	// Use this for initialization
	void Start () 
	{
		if (removePrompt != null)
			removePrompt.SetActive(false); 
	}

	public void SetAnimationController()
	{
		tetherAnimator = silverAnchorAnimator;
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


		/*
		 * Radius Tether point removal
		 * Possibly deprecated
		 */ 

		playerInRemoveRadius = false; 

		if (allowRadiusRemoval)
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

		/*
		 * Input (right click) Tether point removal
		 */ 
		if (allowKeyRemoval && tetherIndex != 0 && MouseIsOver() && PlayerControlManager.GetKeyDown(ControlInput.FIRE_STASIS) && !LevelStateManager.CursorIsOverAStasisBubble())
		{
			TetherManager.inst.RemoveTetherPoint(tetherIndex); 
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

	/// <summary>
	/// Returns true if the mouse is hovering over this object
	/// </summary>
	public bool MouseIsOver()
	{
		Vector2 mouseWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

		if (clickCollider != null && clickCollider.bounds.Contains(mouseWorldPos) && tetherIndex != 0)
		{
			return true; 
		}

		return false; 
	}
}
