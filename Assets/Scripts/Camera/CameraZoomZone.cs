using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomZone : MonoBehaviour 
{
	[Header("Custom zoom size")] 
	public float targetCameraSize = 10; 

	public Color gizmosColor; 

	public Collider2D zoneCol; 

	void OnDrawGizmos()
	{
		if (zoneCol != null)
		{
			Gizmos.color = gizmosColor; 
			Gizmos.DrawCube(zoneCol.bounds.center, zoneCol.bounds.size); 
		}
	}
}
