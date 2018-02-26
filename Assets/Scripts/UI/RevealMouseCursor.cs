using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealMouseCursor : MonoBehaviour 
{
	public bool revealOnStart; 

	// Use this for initialization
	void Start () 
	{
		if (revealOnStart)
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			Cursor.visible = true;
		}
	}
}
