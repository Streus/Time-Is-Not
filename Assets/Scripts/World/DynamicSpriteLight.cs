using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpriteLight : MonoBehaviour 
{
	SpriteRenderer rend; 
	[Range(0, 1)] float alphaCeiling; 

	float random;

	[Header("Flicker Effect")] 
	public bool flicker_enabled; 
	public float flicker_intensity = 0.1f; 
	public float flicker_scrollSpeed = 1;

	[Range(0, 1)] float flicker_amount; 
	float flicker_theta; 

	[Header("Pulse Effect")] 
	public bool pulse_enabled; 
	public float pulse_intensity = 0.1f; 
	public float pulse_scrollSpeed = 1;

	[Range(0, 1)] float pulse_amount; 
	float pulse_theta; 

	[Header("On/Off Effect")] 
	public bool onOff_enabled; 

	[System.Serializable]
	public struct OnOffState
	{
		[Range(0, 1)] public float alphaMultiplier; 
		public float stayTime; 
		public float transitionLerpSpeed; 
	}

	public int onOff_curIndex; 
	public OnOffState[] onOff_states; 
	[Range(0, 1)] float onOff_finalMultiplier; 

	float onOff_timer; 

	// Use this for initialization
	void Start () 
	{
		rend = GetComponent<SpriteRenderer>(); 
		alphaCeiling = rend.color.a; 
		random = Random.Range(0.0f, 65535.0f);
		SetOnOff_CurIndex(onOff_curIndex); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		if (!Application.isPlaying && !previewInEditMode)
		{
			return; 
		}
		*/ 

		float finalAlpha = alphaCeiling; 

		/*
		 * Flicker effect
		 */ 
		flicker_amount = 0;
		if (flicker_enabled)
		{
			flicker_theta += Time.deltaTime * flicker_scrollSpeed;
			if (flicker_theta > Mathf.PI * 2)
				flicker_theta -= Mathf.PI * 2;
			else if (flicker_theta < -Mathf.PI * 2)
				flicker_theta += Mathf.PI * 2; 

			flicker_amount = Mathf.PerlinNoise(random, Mathf.Sin(flicker_theta)) * ScaleIntensity(flicker_intensity);
			Debug.Log("flicker_amount: " + flicker_amount); 
			finalAlpha -= flicker_amount; 
		}

		/*
		 * Pulse effect
		 */
		pulse_amount = 0; 
		if (pulse_enabled)
		{
			pulse_theta += Time.deltaTime * pulse_scrollSpeed; 
			if (pulse_theta > Mathf.PI * 2)
				pulse_theta -= Mathf.PI * 2;
			else if (pulse_theta < -Mathf.PI * 2)
				pulse_theta += Mathf.PI * 2;

			pulse_amount = ((Mathf.Sin(pulse_theta) / 2) + 0.5f) * ScaleIntensity(pulse_intensity); 
			Debug.Log("pulse pulse_amount: " + pulse_amount); 
			finalAlpha -= pulse_amount; 
		}

		/*
		 * On/off effect
		 */ 

		if (onOff_enabled && onOff_states.Length > 0)
		{
			onOff_timer -= Time.deltaTime; 

			if (onOff_timer <= 0)
			{
				SetOnOff_CurIndex(onOff_curIndex + 1); 
			}

			onOff_finalMultiplier = Mathf.Lerp(onOff_finalMultiplier, onOff_states[onOff_curIndex].alphaMultiplier, onOff_states[onOff_curIndex].transitionLerpSpeed * Time.deltaTime); 
			finalAlpha *= onOff_finalMultiplier; 
		}

		rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, finalAlpha); 

	}

	void SetOnOff_CurIndex(int newIndex)
	{
		if (onOff_states == null || onOff_states.Length == 0)
		{
			return; 
		}

		if (newIndex < onOff_states.Length)
		{
			onOff_curIndex = newIndex; 
		}
		else
		{
			onOff_curIndex = 0; 
		}
		onOff_timer = onOff_states[onOff_curIndex].stayTime;
	}

	float ScaleIntensity(float initIntensity)
	{
		return initIntensity * alphaCeiling; 
	}
}
