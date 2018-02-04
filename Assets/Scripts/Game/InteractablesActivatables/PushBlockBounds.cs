using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PushBlockBounds : MonoBehaviour 
{
	BoxCollider2D col; 

	public Color gizmosBoundsColor; 

	// Use this for initialization
	void Start () 
	{
		col = GetComponent<BoxCollider2D>(); 
	}

	void OnDrawGizmos()
	{
		if (col == null)
		{
			col = GetComponent<BoxCollider2D>();
		}
			
		Gizmos.color = gizmosBoundsColor; 
		Gizmos.DrawCube(col.bounds.center, new Vector3(col.bounds.size.x, col.bounds.size.y, 0.1f)); 
	}
}
