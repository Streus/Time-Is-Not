using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//public class CursorBoundsCollider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
public class CursorBoundsCollider : MonoBehaviour
{
	public bool isOver = false;

	void OnMouseEnter() 
	{
		isOver = true;
		GameManager.inst.AddCursorBound(this);
	}

	void OnMouseExit() 
	{
		isOver = false;
		GameManager.inst.RemoveCursorBound(this);
	}
}
