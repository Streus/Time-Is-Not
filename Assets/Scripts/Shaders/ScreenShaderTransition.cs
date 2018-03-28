using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenShaderTransition : MonoBehaviour 
{
	// Static directory of all active SSTs
	private static Dictionary<string, ScreenShaderTransition> directory;
	static ScreenShaderTransition()
	{
		directory = new Dictionary<string, ScreenShaderTransition> ();
	}

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

	// Other scripts can check transitionInProgress to determine if the transitionState == FADE_IN or FADE_OUT
	bool m_transitionInProgress; 
	public bool transitionInProgress
	{
		get{
			return m_transitionInProgress; 
		}
	}

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

	// Directory management
	public void Awake()
	{
		directory.Add (transitionName, this);
	}
	public void OnDestroy()
	{
		directory.Remove (transitionName);
	}

	public static ScreenShaderTransition getInstance(string name)
	{
		ScreenShaderTransition sst;
		if (directory.TryGetValue (name, out sst))
			return sst;
		return null;
	}

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
				if (curFade <= 0f)
				{
					curFade = 0f; 
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
				if (curFade >= 1f)
				{
					curFade = 1f; 
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
				m_transitionInProgress = true; 
				break; 
			case TransitionState.ON:
				curFade = 1;
				m_transitionInProgress = false; 
				break; 
			case TransitionState.FADE_OUT:
				curFade = 0;
				m_transitionInProgress = true; 
				break; 
			case TransitionState.OFF:
				curFade = 0;
				m_transitionInProgress = false; 
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
