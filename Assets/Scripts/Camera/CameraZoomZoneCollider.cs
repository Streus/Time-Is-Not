using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomZoneCollider : MonoBehaviour 
{
	[SerializeField] bool m_collisionActive; 
	public bool collisionActive
	{
		get{
			return m_collisionActive; 
		}
	}

	float m_targetCameraSize;
	public float targetCameraSize
	{
		get{
			return m_targetCameraSize; 
		}
	}

	// Internal collision
	[SerializeField] List<CameraZoomZone> collidingZones; 

	/*
	void FixedUpdate()
	{
		m_collisionActive = false; 
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.GetComponent<CameraZoomZone>() != null)
		{
			m_targetCameraSize = col.GetComponent<CameraZoomZone>().targetCameraSize; 
			m_collisionActive = true; 
		}
	}
	*/

	void Update()
	{
		if (collidingZones.Count == 0)
		{
			m_collisionActive = false; 
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<CameraZoomZone>() != null)
		{
			CameraZoomZone zone = col.GetComponent<CameraZoomZone>();  

			if (!collidingZones.Contains(zone))
			{
				collidingZones.Add(zone); 
				m_targetCameraSize = zone.targetCameraSize; 
				m_collisionActive = true; 
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.GetComponent<CameraZoomZone>() != null)
		{
			CameraZoomZone zone = col.GetComponent<CameraZoomZone>(); 

			if (collidingZones.Contains(zone))
			{
				collidingZones.Remove(zone); 
			}
		}
	}
}
