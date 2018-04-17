using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUILink : MonoBehaviour 
{
	GameObject lineRendPrefab; 

	//[Tooltip("If checked, the link point Vector2s will be in local position relative to the gameObject. Otherwise, they will be in world position")]
	//public bool localPositions = true; 

	//[Tooltip("The positions of the line renderer. Must be set manually. Uses local/world space depending on whether localPositions is checked.")]
	//[SerializeField] List<Vector2> linkPoints; 

	[Tooltip("The positions of the line renderer. Must be set using gameObject positions")]
	[SerializeField] List<GameObject> linkPoints; 

	LineRenderer lineRenderer; 

	[Tooltip("Optional color override specific to this interactable UI link, used instead of the default Color in ZoomOutUI. Only used if its alpha is > 0")] 
	public Color overrideColor; 

	//[SerializeField] GameObject lineFollowerPrefab; 
	//public float followerSpawnRate; 
	//float spawnTimer; 

	//[SerializeField] List<LineFollower> followers; 

	// Use this for initialization
	void Start () 
	{
		lineRendPrefab = (GameObject)Resources.Load("Prefabs/UILinkLine"); 

		if (lineRenderer == null)
		{
			lineRenderer = GameObject.Instantiate(lineRendPrefab, transform).GetComponent<LineRenderer>();
		}



		//spawnTimer = followerSpawnRate; 
	}

	// Update is called once per frame
	void Update () 
	{
		lineRenderer.positionCount = linkPoints.Count; 

		for (int i = 0; i < linkPoints.Count; i++)
		{
			if (linkPoints[i] == null)
				continue; 

			/*
			if (localPositions)
			{
				lineRenderer.SetPosition(i, (Vector3)linkPoints[i]); 
			}
			else
			{
				lineRenderer.SetPosition(i, transform.position + (Vector3)linkPoints[i]); 
			}
			*/
			lineRenderer.SetPosition(i, linkPoints[i].transform.position); 
		}

		if (overrideColor.a == 0)
		{
			lineRenderer.startColor = new Color (ZoomOutUI.linkLineColor.r, ZoomOutUI.linkLineColor.g, ZoomOutUI.linkLineColor.b, ZoomOutUI.GetCanvasGroupAlpha() * ZoomOutUI.linkLineColor.a);
		}
		else
		{
			lineRenderer.startColor = new Color (overrideColor.r, overrideColor.g, overrideColor.b, ZoomOutUI.GetCanvasGroupAlpha() * overrideColor.a);
		}


		lineRenderer.endColor = lineRenderer.startColor; 

		// Spawn follow objects
		/*if (followerSpawnRate != 0)
		{
			spawnTimer -= Time.deltaTime; 
			if (spawnTimer <= 0)
			{
				spawnTimer = followerSpawnRate;
				//LineFollower newFollower = (GameObject.Instantiate(lineFollowerPrefab, transform)).GetComponent<LineFollower>(); 
				//LineFollower newFollower = (GameObject.Instantiate(Resources.Load("UILinkSpark"), transform)).GetComponent<LineFollower>(); 
				GameObject newFollowerObj = Instantiate(Resources.Load("Prefabs/UILinkSpark", typeof(GameObject))) as GameObject;
				newFollowerObj.transform.SetParent(this.transform); 
				LineFollower newFollower = newFollowerObj.GetComponent<LineFollower>(); 
				followers.Add(newFollower); 
				newFollower.Renderer = lineRenderer; 
			}
		}*/
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow; 
		for (int i = 0; i < linkPoints.Count - 1; i++)
		{
			/*
			if (localPositions)
			{
				Gizmos.DrawLine(transform.position + (Vector3)linkPoints[i], transform.position + (Vector3)linkPoints[i + 1]); 
			}
			else
			{
				Gizmos.DrawLine((Vector3)linkPoints[i], (Vector3)linkPoints[i + 1]);
			}
			*/ 
			if (linkPoints[i] == null || linkPoints[i + 1] == null)
				continue;

			Gizmos.DrawLine(linkPoints[i].transform.position, linkPoints[i + 1].transform.position);
		}
	}
}
