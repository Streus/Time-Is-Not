using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LevelNameHeader : MonoBehaviour 
{
	// Enum State Machine
	enum HeaderState
	{
		WAIT_TO_APPEAR,
		APPEAR,
		VISIBLE,
		DISAPPREAR,
		INACTIVE
	}; 

	[Header("Current state (read only)")] 
	[SerializeField] HeaderState headerState; 
	
	[Header("Object refs")] 
	[SerializeField] Text levelNameText; 

	CanvasGroup levelHeaderGroup; 

	// Transition settings
	// For now, use a fade
	[Header("Transition settings")] 
	[SerializeField] float fadeInSpeed = 1; 
	[SerializeField] float fadeOutSpeed = 1; 

	// Timing
	float waitTimer; 

	// waitToAppear should probably be temporary; instead it should switch to appearing based on a level fade in finish function
	[SerializeField] float waitToAppearLength = 1; 
	[SerializeField] float stayVisibleLength = 1; 


	// Use this for initialization
	void Start () 
	{
		levelHeaderGroup = GetComponent<CanvasGroup>(); 

		if (SceneSetup.inst != null)
		{
			levelNameText.text = SceneSetup.inst.levelHeader; 

			if (SceneSetup.inst.useLevelHeader)
			{
				SetHeaderState(HeaderState.WAIT_TO_APPEAR); 
				//SetHeaderState(HeaderState.APPEAR); 
			}
			else
			{
				SetHeaderState(HeaderState.INACTIVE); 
			}
		}
		else
		{
			SetHeaderState(HeaderState.INACTIVE); 
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Appearing
		if (headerState == HeaderState.APPEAR)
		{
			levelHeaderGroup.alpha = Mathf.Lerp(levelHeaderGroup.alpha, 1, fadeInSpeed * Time.deltaTime); 

			if (levelHeaderGroup.alpha > 0.99f)
			{
				SetHeaderState(HeaderState.VISIBLE); 
			}
		}
		// Disappearing
		else if (headerState == HeaderState.DISAPPREAR)
		{
			levelHeaderGroup.alpha = Mathf.Lerp(levelHeaderGroup.alpha, 0, fadeOutSpeed * Time.deltaTime);

			if (levelHeaderGroup.alpha < 0.01f)
			{
				SetHeaderState(HeaderState.INACTIVE); 
			}
		}
		// Visible
		else if (headerState == HeaderState.VISIBLE)
		{
			waitTimer -= Time.deltaTime; 

			if (waitTimer <= 0)
			{
				SetHeaderState(HeaderState.DISAPPREAR); 
			}
		}
		else if (headerState == HeaderState.WAIT_TO_APPEAR)
		{
			// TODO
			// Triggr the appear state when the screen has finished fading

			// For now, use a temp timer
			waitTimer -= Time.deltaTime;

			if (waitTimer <= 0)
			{
				SetHeaderState(HeaderState.APPEAR); 
			}
		}
	}

	void SetHeaderState(HeaderState newState)
	{
		headerState = newState;

		switch (newState)
		{
			case HeaderState.WAIT_TO_APPEAR:
				levelHeaderGroup.alpha = 0;
				waitTimer = waitToAppearLength;
				break; 
			case HeaderState.APPEAR:
				levelHeaderGroup.alpha = 0;
				break; 
			case HeaderState.VISIBLE:
				levelHeaderGroup.alpha = 1;
				waitTimer = stayVisibleLength;
				break; 
			case HeaderState.DISAPPREAR:
				levelHeaderGroup.alpha = 1;
				break; 
			case HeaderState.INACTIVE:
				levelHeaderGroup.alpha = 0;
				break; 
		}
	}
}
