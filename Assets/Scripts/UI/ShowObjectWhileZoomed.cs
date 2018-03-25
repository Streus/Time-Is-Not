using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectWhileZoomed : MonoBehaviour 
{
	private int defaultLayer;
	private SpriteRenderer rend;
	private bool zoomHelper = false;

	// Use this for initialization
	void Awake () 
	{
		rend = gameObject.GetComponent<SpriteRenderer> ();
		defaultLayer = rend.sortingLayerID;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(GameManager.CameraIsZoomedOut() && !zoomHelper)
		{
			rend.sortingLayerName = "UI";
			zoomHelper = true;
		}
		if(!GameManager.CameraIsZoomedOut() && zoomHelper)
		{
			rend.sortingLayerID = defaultLayer;
			zoomHelper = false;
		}
		
	}
}
