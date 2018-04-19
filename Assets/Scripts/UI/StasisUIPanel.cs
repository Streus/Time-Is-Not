using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisUIPanel : Singleton<StasisUIPanel> 
{
	[Tooltip("(Drag In) The stasis UI piece of the time tether that displays stasis UI")]
	public GameObject stasisPanel; 

	RectTransform panelRT; 
	CanvasGroup stasisCanvasGroup; 

	public ParticleSystem stasisFitParticles; 

	public ParticleSystem[] stasisLightParticles; 

	int m_transitionState; 
	public int transitionState
	{
		get{
			return m_transitionState; 
		}
	}

	[Header("Transition settings")]
	public Vector2 transitionStartPos;
	public Vector2 transitionEndPos; 
	public Vector3 transitionStartScale;
	public Vector3 transitionEndScale; 
	public float transitionSpeed = 1; 
	float transitionProgress; 

	// The first timer that plays before the lights begin playing in succession
	public float startLightWaitLength; 
	float startLightWaitTimer; 
	// The time in between lights playing in succession
	public float lightWaitLength;
	float lightWaitTimer; 

	/*
	public float waitForRotationLength;
	float waitForRotationTimer; 

	public float startRotZ; 
	public float endRotZ; 
	public float spinSpeed; 
	*/ 

	public bool transitionActive
	{
		get{
			return m_transitionState == 0 ? false : true;  
		}
	}

	// Use this for initialization
	void Start () 
	{
		UpdateStasisPanelActive(false); 
		panelRT = stasisPanel.GetComponent<RectTransform>(); 
		stasisCanvasGroup = stasisPanel.GetComponent<CanvasGroup>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Lerp in
		if (m_transitionState == 1)
		{
			transitionProgress += transitionSpeed * Time.deltaTime; 
			if (transitionProgress >= 1)
			{
				transitionProgress = 1; 
				m_transitionState = 2; 
				//waitForRotationTimer = waitForRotationLength; 
				startLightWaitTimer = startLightWaitLength; 
				lightWaitTimer = 0; 
				AudioLibrary.PlayPressurePlateOn(); 
			}

			if (transitionProgress > 0.8f && !stasisFitParticles.isPlaying)
			{
				stasisFitParticles.Play(); 
			}

			panelRT.anchoredPosition = Vector2.Lerp(transitionStartPos, transitionEndPos, transitionProgress); 
			panelRT.localScale = Vector2.Lerp(transitionStartScale, transitionEndScale, transitionProgress); 
			stasisCanvasGroup.alpha = transitionProgress; 
		}
		else if (m_transitionState >= 2)
		{
			if (startLightWaitTimer > 0)
			{
				startLightWaitTimer -= Time.deltaTime; 
				return; 
			}

			lightWaitTimer -= Time.deltaTime; 

			if (lightWaitTimer <= 0)
			{
				stasisLightParticles[m_transitionState - 2].Play(); 
				AudioLibrary.PlayStasisBubbleRemove();  
				m_transitionState++; 

				if (m_transitionState - 2 < stasisLightParticles.Length)
				{
					lightWaitTimer = lightWaitLength; 
				}
				else
				{
					m_transitionState = 0; 
				}
			}
		}


			/*
			lightWaitTimer -= Time.deltaTime; 

			if (lightWaitTimer <= 0)
			{
				Debug.Log("lightWaitTimer reached zero. TransitionState = " + m_transitionState); 

				stasisLightParticles[m_transitionState - 2].Play(); 

				if (m_transitionState - 1 < stasisLightParticles.Length)
				{
					lightWaitTimer = lightWaitLength; 
					m_transitionState++; 
				}
				else
				{
					m_transitionState = 0; 
				}
			}
		}*/

		// Wait for rotation
		/*
		else if (transitionState == 2)
		{
			waitForRotationTimer -= Time.deltaTime;
			if (waitForRotationTimer <= 0)
			{
				transitionState = 3; 
			}
		}
		// Rotate
		else if (transitionState == 3)
		{
			panelRT.localEulerAngles = new Vector3 (0, 0, Mathf.Lerp(panelRT.localEulerAngles.z, endRotZ, spinSpeed * Time.deltaTime));

			if (Mathf.Abs(panelRT.localEulerAngles.z - endRotZ) < 0.01f)
			{
				panelRT.localEulerAngles = new Vector3 (0, 0, endRotZ); 

				// TODO
				// Play particle effect here

				transitionState = 0; 
			}
		}
		*/ 
	}

	public void UpdateStasisPanelActive(bool useTransition)
	{
		if (GameManager.inst.canUseStasis)
		{
			stasisPanel.SetActive(true); 

			if (useTransition)
				StartTransition(); 
		}
		else
		{
			stasisPanel.SetActive(false); 
		}
	}

	void StartTransition()
	{
		m_transitionState = 1; 
		panelRT.anchoredPosition = transitionStartPos; 
		panelRT.localScale = transitionStartScale; 
		stasisCanvasGroup.alpha = 0; 
		transitionProgress = 0; 
		//panelRT.localEulerAngles = new Vector3 (0, 0, startRotZ); 
	}
}
