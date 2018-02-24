using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
	[Header("Cursor functionality")]
	[Tooltip("The current state of the cursor")]
	public CursorType cursorType; 
	CursorType lastCursorType; 

	public Texture2D cursorTexture_GAMEPLAY;
	public Texture2D cursorTexture_UI_HOVER; 
	public Texture2D cursorTexture_DEACTIVATED;

	// Dynamically keeps track of any ui bounds that the cursor is currently within
	List<CursorBoundsCollider> collidingCursorBounds; 

	// When true, bounds interaction will not change the cursor type
	public bool lockCursorType;

	void Awake()
	{
		collidingCursorBounds = new List<CursorBoundsCollider> (); 

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
		if (cursorType != lastCursorType)
		{
			RefreshCursorType(); 
		}
	}

	void RefreshCursorType()
	{
		lastCursorType = cursorType; 

		// The second parameter should be half of the cursor's size so that is is centered
		switch (cursorType)
		{
		case CursorType.GAMEPLAY:
			Cursor.SetCursor(cursorTexture_GAMEPLAY, new Vector2 (cursorTexture_GAMEPLAY.width/2, cursorTexture_GAMEPLAY.height/2), CursorMode.Auto);
			break; 
		case CursorType.UI_HOVER:
			Cursor.SetCursor(cursorTexture_UI_HOVER, new Vector2 (cursorTexture_UI_HOVER.width/2, cursorTexture_UI_HOVER.height/2), CursorMode.Auto);
			break;
		case CursorType.DEACTIVATED:
			Cursor.SetCursor(cursorTexture_DEACTIVATED, new Vector2 (cursorTexture_DEACTIVATED.width/2, cursorTexture_DEACTIVATED.height), CursorMode.Auto);
			break;
		default:
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			break;
		}
	}

	public static bool CursorInGameplayState()
	{
		if (inst.cursorType == CursorType.GAMEPLAY)
		{
			return true; 
		}
		return false; 
	}

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
			cursorType = CursorType.GAMEPLAY;
		}
		else
		{
			cursorType = CursorType.UI_HOVER; 
		}
	}
}
