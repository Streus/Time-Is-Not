using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum CursorState
{
	GAMEPLAY,
	MENU,
	DEACTIVATED,
	DEFAULT
}

public class CursorManager : Singleton<CursorManager>
{
	[Header("Cursor functionality")]
	[Tooltip("The current state of the cursor")]
	public CursorState cursorState; 
	CursorState lastCursorState; 

	[Tooltip("(Drag-in) The main cursor image that follows the mouse position.")]
	[SerializeField] GameObject mainCursor; 
	SpriteRenderer mainCursorRend; 

	[Tooltip("(Drag-in) The secondary cursor image that follows the main pointer.")]
	[SerializeField] GameObject dashCursor; 
	SpriteRenderer dashCursorRend; 

	[Header("Stasis Cursor sprites")]
	[SerializeField] Sprite cursorStasisEnabled;
	[SerializeField] Sprite cursorStasisDisabled;
	[SerializeField] Sprite cursorStasisHover; 

	[Header("Menu Cursor sprites")]
	[SerializeField] Sprite cursorMenu;
	[SerializeField] Sprite cursorDeactivated; 

	[Header("Dash Cursor sprites")]
	[SerializeField] Sprite cursorDashEnabled;
	[SerializeField] Sprite cursorDashDisabled; 
	[SerializeField] Sprite cursorDashTarget; 


	bool dashTargetLock; 
	Vector3 dashTargetLockPos; 


	// Dynamically keeps track of any ui bounds that the cursor is currently within
	// This functionality might be deprecated at this point
	List<CursorBoundsCollider> collidingCursorBounds; 

	// When true, bounds interaction will not change the cursor type
	public bool lockCursorType;

	void Awake()
	{
		collidingCursorBounds = new List<CursorBoundsCollider> (); 

		mainCursorRend = mainCursor.GetComponent<SpriteRenderer>(); 
		dashCursorRend = dashCursor.GetComponent<SpriteRenderer>(); 

		RefreshCursorType(); 
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Update cursor type if it has changed (detected via lastCursorType)
		if (cursorState != lastCursorState)
		{
			RefreshCursorType(); 
		}

		if (cursorState != CursorState.DEFAULT)
		{
			Cursor.visible = false; 
		}

		if (cursorState == CursorState.GAMEPLAY)
		{
			UpdateGameplayCursor(); 
		}
	}

	void LateUpdate()
	{
		// Get the mouse's position in world space
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
		mouseWorldPos = new Vector3 (mouseWorldPos.x, mouseWorldPos.y, 0); 

		// Get the player's position in screen space
		//Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(GameManager.GetPlayer().transform.position); 

		// Set the position of the main cursor
		//mainCursor.anchoredPosition  = new Vector2 (Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
		mainCursor.transform.position = mouseWorldPos; 

		// If in gameplay state, update the position of the dash cursor
		if (cursorState == CursorState.GAMEPLAY)
		{
			GameObject p = GameManager.GetPlayer();
			Collider2D pcol = p.GetComponent<Collider2D>();
			Vector3 pPos = p.transform.position + (Vector3)pcol.offset; 
			pPos = new Vector3 (pPos.x, pPos.y, 0); 

			// If the player is dashing to a spot, the dash cursor remains fixed
			if (dashTargetLock)
			{
				dashCursor.transform.position = dashTargetLockPos; 
			}
			// If the dash cursor is within the radius of the dash
//			else if (Vector2.Distance((Vector2)mouseWorldPos, pPos) < GameManager.getPlayerMaxJumpDist())
//			{
//				dashCursor.transform.position = mouseWorldPos; 
//			}
			// If the dash cursor is outside the dash radius
			else
			{
				dashCursor.transform.position = GameManager.getPlayerJumpTarget(); 
			}

			/*
			// If the player is dashing to a spot, the dash cursor remains fixed
			if (dashTargetLock)
			{
				dashCursor.transform.position = dashTargetLockPos; 
			}
			// If the dash cursor is within the radius of the dash
//			else if (Vector2.Distance((Vector2)mouseWorldPos, pPos) < GameManager.getPlayerMaxJumpDist())
//			{
//				dashCursor.transform.position = mouseWorldPos; 
//			}
			// If the dash cursor is outside the dash radius
			else
			{
				// Determine the jump distance
				float finalDist = GameManager.getPlayerMaxJumpDist(); 

				// If within the radius of the dash, the final distance is determined by the distance between the dash cursor and the player foot position
				if (Vector2.Distance((Vector2)mouseWorldPos, pPos) < GameManager.getPlayerMaxJumpDist())
				{
					finalDist = Vector2.Distance((Vector2)mouseWorldPos, pPos); 
				}

				// Get the radius of the dash cursor, assuming uniform size
				float dashCursorRadius = dashCursorRend.bounds.extents.x; 

				// Get the direction from the player position to the mouse
				Vector3 mouseDir = (mouseWorldPos - pPos).normalized; 

				// Raycast to find any colliders in between the player position and the endpoint of the dash cursor
				RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)pPos, mouseDir, finalDist + dashCursorRadius, GameManager.getPlayerMoveMask()); 

				// Keep track of the nearest RaycastHit
				RaycastHit2D nearest = default(RaycastHit2D);

				// Find the nearest RaycastHit
				for (int i = 0; i < hits.Length; i++)
				{
					if (hits[i].collider.isTrigger || hits[i].collider == GameManager.GetPlayer().GetComponent<Collider2D>())
						continue; 

					if (nearest == default(RaycastHit2D))
						nearest = hits[i];
					else if (hits[i].distance < nearest.distance)
						nearest = hits[i];
				}

				// If a RaycastHit was found, the distance to it replaces the default finalDist
				if (nearest != default(RaycastHit2D))
				{
					Debug.Log("Has nearest. Dist: " + nearest.distance); 
					finalDist = nearest.distance - dashCursorRadius; 
				}

				dashCursor.transform.position = pPos + (mouseDir * finalDist);
				//dashCursor.transform.position = pPos + (mouseDir * GameManager.getPlayerMaxJumpDist());
			}
			*/ 
		}
	}

	void RefreshCursorType()
	{
		lastCursorState = cursorState; 

		// If setting actual Cursor texture, the second parameter should be half of the cursor's size so that is is centered

		switch (cursorState)
		{
		case CursorState.GAMEPLAY:
			break; 
		case CursorState.MENU:
			mainCursorRend.sprite = cursorMenu; 
			break;
		case CursorState.DEACTIVATED:
			mainCursorRend.sprite = cursorDeactivated; 
			break;
		default:
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			Cursor.visible = true;
			break;
		}

		if (cursorState == CursorState.GAMEPLAY)
		{
			dashCursorRend.enabled = true; 
		}
		else
		{
			dashCursorRend.enabled = false; 
		}
	}

	public static bool CursorInGameplayState()
	{
		if (inst.cursorState == CursorState.GAMEPLAY)
		{
			return true; 
		}
		return false; 
	}

	void UpdateGameplayCursor()
	{
		// Choose sprite for main cursor (stasis)

		// If hovering over a stasis bubble
		if (LevelStateManager.CursorIsOverAStasisBubble())
		{
			mainCursorRend.sprite = cursorStasisHover; 
		}
		// TODO: This might be the wrong check. Might need to check where the player checks if can shoot a stasis bubble
		else if (GameManager.inst.canUseStasis && LevelStateManager.canAddStasisBubble())
		{
			mainCursorRend.sprite = cursorStasisEnabled; 
		}

		else
		{
			mainCursorRend.sprite = cursorStasisDisabled; 
		}

		// Secondary cursor (dash target)
		// Temporary: hide the dash cursor while zoomed out
		// TODO: replace this with a separate CursorState for zoom out
		if (GameManager.CameraIsZoomedOut())
		{
			dashCursorRend.enabled = false; 
		}
		else
		{
			dashCursorRend.enabled = true; 
		}

		if (GameManager.isPlayerDashing() == true)
		{
			dashTargetLock = true; 
			dashTargetLockPos = dashCursor.transform.position; 
			dashCursorRend.sprite = cursorDashTarget; 
		}
		else
		{
			dashTargetLock = false; 

			if (GameManager.dashIsCharged())
			{
				dashCursorRend.sprite = cursorDashEnabled; 
			}
			else
			{
				dashCursorRend.sprite = cursorDashDisabled; 
			}
		}

		// Update texture
		//dashCursorRend.texture = cursorDashEnabled; 
	}

	/*
	void SetMainCursor(Texture2D _texture)
	{
		mainCursorImg.texture = _texture;
		mainCursor.sizeDelta = new Vector2 (_texture.width, _texture.height); 
	}
	*/ 


	/*
	 * Cursor bounds functionality
	 * Potentially deprecated
	 */

	public void AddCursorBound(CursorBoundsCollider boundCol)
	{
		if (collidingCursorBounds.Contains(boundCol))
		{
			return; 
		}

		collidingCursorBounds.Add(boundCol); 
		OnCursorBoundsUpdated();
	}

	public void RemoveCursorBound(CursorBoundsCollider boundCol)
	{
		if (!collidingCursorBounds.Contains(boundCol))
		{
			return; 
		}
		collidingCursorBounds.Remove(boundCol); 
		OnCursorBoundsUpdated(); 
	}

	public void OnCursorBoundsUpdated()
	{
		if (lockCursorType)
		{
			return; 
		}

		if (collidingCursorBounds.Count == 0)
		{
			cursorState = CursorState.GAMEPLAY;
		}
		else
		{
			cursorState = CursorState.MENU; 
		}
	}
}
