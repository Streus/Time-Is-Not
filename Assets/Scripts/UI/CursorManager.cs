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
			if (dashTargetLock)
			{
				dashCursor.transform.position = dashTargetLockPos; 
			}
			else if (Vector3.Distance(mouseWorldPos, GameManager.GetPlayer().transform.position) < GameManager.getPlayerMaxJumpDist())
			{
				dashCursor.transform.position = mouseWorldPos; 
			}
			else
			{
				// TODO
				Vector3 mouseDir = (mouseWorldPos - GameManager.GetPlayer().transform.position).normalized; 
				dashCursor.transform.position = GameManager.GetPlayer().transform.position + (mouseDir * GameManager.getPlayerMaxJumpDist()); 
			}
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
		// Main cursor (stasis)
		// TODO: This might be the wrong check. Might need to check where the player checks if can shoot a stasis bubble
		if (LevelStateManager.canAddStasisBubble())
		{
			mainCursorRend.sprite = cursorStasisEnabled; 
		}
		else
		{
			mainCursorRend.sprite = cursorStasisDisabled; 
		}

		// Secondary cursor (dash target)

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
