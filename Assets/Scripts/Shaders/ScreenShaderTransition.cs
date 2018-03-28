using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenShaderTransition : MonoBehaviour 
{
	// Enum State Machine
	public enum TransitionState
	{
		FADE_IN,
		ON,
		FADE_OUT,
		OFF
	}; 

	[Tooltip("This name is for clarity in the inspector to distuingish between different materials that use this script")] 
	public string transitionName; 

	[Header("Current state (read only)")] 
	[SerializeField] TransitionState transitionState; 

	[Header("Settings")] 
	[SerializeField] bool shaderEnabled;
	[SerializeField] Material transitionMat; 

	[SerializeField] float fadeInSpeed; 
	[SerializeField] float fadeOutSpeed;

	[Range(0, 1)] public float curFade; 

	// TODO
	// Add instance events/delegates

	// Events
	public event TransitionDone fadeInDone;
	public event TransitionDone fadeOutDone;

	// Delegates
	public delegate void TransitionDone();

	// Use this for initialization
	void Start () 
	{
		SetTransitionState(transitionState); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Application.isPlaying)
		{
			if (transitionState == TransitionState.FADE_IN)
			{
				curFade -= fadeInSpeed * Time.deltaTime; 
				if (curFade <= 0)
				{
					curFade = 0; 
					SetTransitionState(TransitionState.OFF); 
					if (fadeInDone != null)
					{
						fadeInDone(); 
					}
				}
			}
			else if (transitionState == TransitionState.FADE_OUT)
			{
				curFade += fadeOutSpeed * Time.deltaTime; 
				if (curFade >= 1)
				{
					curFade = 1; 
					SetTransitionState(TransitionState.ON); 
					if (fadeOutDone != null)
					{
						fadeOutDone(); 
					}
				}
			}

			transitionMat.SetFloat("_Cutoff", curFade);
			Debug.Log("material cutoff: " + transitionMat.GetFloat("_Cutoff"));
		}
	}

	#if UNITY_EDITOR
	void OnValidate()
	{
		SetTransitionState(transitionState); 
	}
	#endif

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (transitionMat != null && shaderEnabled)
			Graphics.Blit(src, dst, transitionMat);
		else
			Graphics.Blit(src, dst); 
	}

	/// <summary>
	/// Sets the transition to the specified state, using ScreenShaderTransition.TransitionState.VALUE
	/// </summary>
	/// <param name="newState">New state.</param>
	public void SetTransitionState(TransitionState newState)
	{
		transitionState = newState;

		switch (newState)
		{
			case TransitionState.FADE_IN:
				curFade = 1;
				break; 
			case TransitionState.ON:
				curFade = 1;
				break; 
			case TransitionState.FADE_OUT:
				curFade = 0;
				break; 
			case TransitionState.OFF:
				curFade = 0;
				break; 
		}
	}

	/// <summary>
	/// Sets the transition to begin fading in (from black to transparent)
	/// </summary>
	public void SetFadeIn()
	{
		SetTransitionState(TransitionState.FADE_IN); 
	}

	/// <summary>
	/// Sets the transition to begin fading out (from transparent to black)
	/// </summary>
	public void SetFadeOut()
	{
		SetTransitionState(TransitionState.FADE_OUT);
	}
}
