using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class TetherTransition : MonoBehaviour
{
	public enum TransitionState {FADE_IN, FADE_OUT, BLACK_SCREEN_REST, TRANSPARENT_SCREEN_REST};

	public TransitionState curState; 
	[SerializeField] float fadeInSpeed; 
	[SerializeField] float fadeOutSpeed; 

	bool m_transitionActive; 

	// Actions
	public event TransitionFinished FadeInFinished; 
	public event TransitionFinished FadeOutFinished; 

	// Delegates
	public delegate void TransitionFinished ();

	public bool transitionActive
	{
		get{
			return m_transitionActive; 
		}
	}

	[SerializeField] Image fadeImage; 

	// Use this for initialization
	void Start () 
	{
		fadeImage.enabled = true; 

		switch (curState)
		{
		case TransitionState.FADE_IN: 
			SetFadeIn(); 
			break;
		case TransitionState.FADE_OUT:
			SetFadeOut(); 
			break;
		case TransitionState.BLACK_SCREEN_REST:
			fadeImage.color = new Color (0, 0, 0, 1);
			break;
		case TransitionState.TRANSPARENT_SCREEN_REST:
			fadeImage.color = new Color (0, 0, 0, 0);
			break;
		default:
			Debug.LogError("Bad transition state (how'd you do this?)");
			break;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (curState == TransitionState.FADE_IN)
		{
			fadeImage.color = new Color (0, 0, 0, fadeImage.color.a - Time.unscaledDeltaTime * fadeInSpeed);
			if (fadeImage.color.a <= 0)
			{
				fadeImage.color = new Color (0, 0, 0, 0);
				curState = TransitionState.TRANSPARENT_SCREEN_REST; 
				OnFadeInCompletion(); 
			}
		}
		else if (curState == TransitionState.FADE_OUT)
		{
			fadeImage.color = new Color (0, 0, 0, fadeImage.color.a + Time.unscaledDeltaTime * fadeOutSpeed);
			if (fadeImage.color.a >= 1)
			{
				fadeImage.color = new Color (0, 0, 0, 1);
				curState = TransitionState.BLACK_SCREEN_REST; 
				OnFadeOutCompletion(); 
			}
		}
	}

	public void SetFadeIn()
	{
		curState = TransitionState.FADE_IN; 
		fadeImage.color = new Color (0, 0, 0, 1); 
		m_transitionActive = true; 
	}

	public void SetFadeOut()
	{
		curState = TransitionState.FADE_OUT;
		fadeImage.color = new Color (0, 0, 0, 0);
		m_transitionActive = true; 
	}

	public bool TransitionInProgress()
	{
		if (curState == TransitionState.TRANSPARENT_SCREEN_REST || curState == TransitionState.BLACK_SCREEN_REST)
		{
			return false; 
		}
		return true; 
	}
		

	// Fade completion functions

	void OnFadeInCompletion()
	{
		m_transitionActive = false; 

		// Send out an event
		//Events.Trigger("FadeInFinished"); 
		if (FadeInFinished != null)
		{
			FadeInFinished(); 
		}
	}
		
	void OnFadeOutCompletion()
	{
		m_transitionActive = false; 

		// Send out an event
		//Events.Trigger("FadeOutFinished");
		if (FadeOutFinished != null)
		{
			FadeOutFinished(); 
		}
	}
}
