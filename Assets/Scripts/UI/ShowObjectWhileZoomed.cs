using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectWhileZoomed : MonoBehaviour 
{
	private int defaultLayer = 1;
	private SpriteRenderer rend;

	[Tooltip("If this is affecting a particle renderer, drag in the gameObjec to get the renderer")]
	[SerializeField] ParticleSystemRenderer prend;
	private bool zoomHelper = false;

	// Use this for initialization
	void Awake () 
	{
		if (GetComponent<SpriteRenderer>() != null)
		{
			rend = gameObject.GetComponent<SpriteRenderer>();
			defaultLayer = rend.sortingLayerID;
		}

		if (prend != null)
		{
			defaultLayer = prend.sortingLayerID;
		}

		// Not working... thus the serialized field instead
		//prend = gameObject.GetComponent<ParticleSystemRenderer>();


	}
	
	// Update is called once per frame
	void Update () 
	{
		if(GameManager.CameraIsZoomedOut() && !zoomHelper)
		{
			if(rend != null)
				rend.sortingLayerName = "ScreenFade";
			if (prend != null)
			{
				prend.sortingLayerName = "ScreenFade";
				Debug.Log("Set to screen fade"); 
			}
			else
			{
				Debug.Log("prend is null"); 
			}
				 
			zoomHelper = true;
		}
		if(!GameManager.CameraIsZoomedOut() && zoomHelper)
		{
			if (rend != null)
				rend.sortingLayerID = defaultLayer;
			if (prend != null)
				prend.sortingLayerID = defaultLayer;
			zoomHelper = false;
		}
		
	}
}
