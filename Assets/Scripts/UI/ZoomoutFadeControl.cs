using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ZoomoutFadeControl : MonoBehaviour 
{
	SpriteRenderer rend;
	[Range(0, 1)]
	public float screenFadePercent = 0.1f;
	public float fadeRate = 1f;
	[Tooltip("If this is true, the scene will always be darkened, used for testing")]
	public bool screenFadeOverride = false;

	// Use this for initialization
	void Awake () 
	{
		rend = gameObject.GetComponent<SpriteRenderer> ();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!Application.isPlaying && !screenFadeOverride) 
		{
			rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0);
			return;
		}
		if(GameManager.CameraIsZoomedOut() || screenFadeOverride)
		{
			if(rend.color.a < screenFadePercent)
			{
				rend.color = new Color (rend.color.r, rend.color.g, rend.color.b, rend.color.a + (fadeRate * Time.deltaTime));
			}
			else
			{
				rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, screenFadePercent);
			}
		}
		else
		{
			if(rend.color.a > 0)
			{
				rend.color = new Color (rend.color.r, rend.color.g, rend.color.b, rend.color.a - (fadeRate * Time.deltaTime));
			}
			else
			{
				rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0);
			}
		}
	}
}
