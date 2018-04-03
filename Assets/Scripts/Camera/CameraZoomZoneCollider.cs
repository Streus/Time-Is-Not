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

	[SerializeField] float m_targetCameraSize;
	public float targetCameraSize
	{
		get{
			return m_targetCameraSize; 
		}
	}

	[SerializeField] Bounds colBounds; 

	// Internal collision
	//[SerializeField] List<CameraZoomZone> collidingZones; 

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
		/*
		if (collidingZones.Count == 0)
		{
			m_collisionActive = false; 
		}
		*/ 

		// Change to overlap box

		CheckZoneOverlap(); 
	}

	void CheckZoneOverlap()
	{
		Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)(transform.position + colBounds.center), (Vector2)colBounds.size, 0); 

		for (int i = 0; i < cols.Length; i++)
		{
			if (cols[i].GetComponent<CameraZoomZone>())
			{
				// Note: this picks the first targetCameraSize it finds. Multiple camera zones with different sizes may produce unexpected behavior
				m_targetCameraSize = cols[i].GetComponent<CameraZoomZone>().targetCameraSize;
				m_collisionActive = true; 
				return; 
			}
		}

		m_collisionActive = false; 
	}

	/*
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
	*/ 

	void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color (0, 0.5f, 0.5f, 0.5f);
		Gizmos.DrawCube(transform.position + colBounds.center, colBounds.size); 
		//Gizmos.DrawCube(transform.position, new Vector3 (1, 1, 1)); 
	}
}
