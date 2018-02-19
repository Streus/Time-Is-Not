using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ZoomOutUI : Singleton<ZoomOutUI> 
{
	CameraManager cameraManager; 

	CanvasGroup canvasGroup; 

	[Tooltip("The speed, multiplied by Time.deltaTime, that the zoom out UI fades in")]
	public float fadeInSpeed; 
	[Tooltip("The speed, multiplied by Time.deltaTime, that the zoom out UI fades out")]
	public float fadeOutSpeed;

	[Tooltip("The speed, multiplied by Time.deltaTime, that the edge of screen arrows fade in/out when the camera reaches level bounds")]
	public float arrowFadeSpeed; 

	CanvasGroup leftArrow;
	CanvasGroup rightArrow; 
	CanvasGroup upArrow; 
	CanvasGroup downArrow; 

	[SerializeField] Color m_linkLineColor; 
	public static Color linkLineColor
	{
		get{
			return inst.m_linkLineColor; 
		}
	}


	// Use this for initialization
	void Start () 
	{
		canvasGroup = GetComponent<CanvasGroup>(); 
		canvasGroup.alpha = 0; 

		cameraManager = GameManager.GetCameraManager(); 

		leftArrow = transform.Find("Arrows/LeftArrow").GetComponent<CanvasGroup>(); 
		rightArrow = transform.Find("Arrows/RightArrow").GetComponent<CanvasGroup>(); 
		upArrow = transform.Find("Arrows/UpArrow").GetComponent<CanvasGroup>(); 
		downArrow = transform.Find("Arrows/DownArrow").GetComponent<CanvasGroup>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Zoom out UI is active
		if (GameManager.CameraIsZoomedOut() == true)
		{
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, fadeInSpeed * Time.deltaTime); 
		}
		// Zoom out UI is not active
		else
		{
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, fadeOutSpeed * Time.deltaTime); 
		}

		// Update each arrow's visibility, depending on whether the player can keep panning in that direction
		UpdateArrowVisibility(!cameraManager.atLeftBound, leftArrow); 
		UpdateArrowVisibility(!cameraManager.atRightBound, rightArrow); 
		UpdateArrowVisibility(!cameraManager.atTopBound, upArrow); 
		UpdateArrowVisibility(!cameraManager.atBottomBound, downArrow); 
	}

	void UpdateArrowVisibility(bool isOn, CanvasGroup arrow)
	{
		if (isOn)
		{
			arrow.alpha = Mathf.Lerp(arrow.alpha, 1, arrowFadeSpeed * Time.deltaTime);  
		}
		else
		{
			arrow.alpha = Mathf.Lerp(arrow.alpha, 0, arrowFadeSpeed * Time.deltaTime); 
		}
	}

	public static float GetCanvasGroupAlpha()
	{
		return inst.canvasGroup.alpha; 
	}
}
