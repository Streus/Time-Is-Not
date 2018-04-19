using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ScreenshotManager : Singleton<ScreenshotManager> 
{
	public Camera screenCam; 

	[SerializeField] List<RenderTexture> screenshots; 

	public int shotWidth; 
	public int shotHeight;
	public int shotDepth; 

	// Events
	public event ScreenshotEvent takeScreenshot;

	// Delegates
	public delegate void ScreenshotEvent(bool startShot);

	// UI screenshot
	[Header("Screenshot settings")]
	public RawImage[] screenshotImages;
	[HideInInspector] public bool isHoveringOverButton;
	int hoverButton;
	bool revealScreenshot;
	public float screenshotFadeInSpeed = 1;
	public float screenshotFadeOutSpeed = 1;
	int curScreenshotIndex; 

	void Awake()
	{
		for (int i = 0; i < LevelStateManager.maxNumStates; i++)
		{
			screenshots.Add(new RenderTexture (shotWidth, shotHeight, shotDepth)); 
		}
	}

	void Start()
	{
		screenCam.enabled = false; 
		//screenshot.gameObject.SetActive(true);

		for (int i = 0; i < screenshotImages.Length; i++)
		{
			screenshotImages[i].gameObject.SetActive(true); 
		}
	}

	// Temporary
	void Update()
	{
		//CameraManager.instance.fitToBounds(inst.screenCam.transform, inst.screenCam); 
		UpdateScreenshotState(); 
	}
		

	public static void createScreenshot(int stateNum)
	{
		if (inst == null)
			return; 

		inst.screenCam.targetTexture = inst.screenshots[stateNum]; 

		// Enable the camera to take a screenshot
		inst.screenCam.enabled = true; 

		// Apply any effects only the screenshot camera can see, using an event
		if (inst.takeScreenshot != null)
		{
			inst.takeScreenshot(true); 
		}

		// Realign the screenshot camera to use bounds correctly
		//CameraManager.instance.fitToBounds(inst.screenCam.transform); 
		CameraManager.instance.fitToBounds(inst.screenCam.transform, inst.screenCam); 

		// Take the screenshot
		inst.screenCam.Render();

		// Unapply any effects only the screenshot camera can see, using an event
		if (inst.takeScreenshot != null)
		{
			inst.takeScreenshot(false); 
		}

		// Disable the camera after taking the screenshot
		inst.screenCam.enabled = false; 
	}

	public static RenderTexture getScreenshot(int stateNum)
	{
		if (stateNum < 0 || stateNum >= inst.screenshots.Count)
		{
			Debug.LogError("Invalid state num; cannot return screenshot from list"); 
			return null;
		}

		return inst.screenshots[stateNum]; 
	}

	public static void removeScreenshotAt(int index)
	{
		// Error cases
		if (index < 0 || index >= inst.screenshots.Count)
		{
			Debug.LogError("Invalid index of " + index + " for screenshot to remove"); 
			return; 
		}
		if (inst.screenshots[index] == null)
		{
			Debug.LogError("Null screenshot RenderTexture at index " + index); 
			return; 
		}

		inst.screenshots.RemoveAt(index);
		inst.screenshots.Add(new RenderTexture (inst.shotWidth, inst.shotHeight, inst.shotDepth)); 
	}

	// UI code
	public void RevealScreenshot(int state)
	{
		//screenshot.color = new Color (1, 1, 1, 1); 
		revealScreenshot = true;
		curScreenshotIndex = state; 
	}

	public void HideScreenshot()
	{
		//screenshot.color = new Color (1, 1, 1, 0); 
		revealScreenshot = false;
	}

	public void UpdateScreenshotState()
	{
		for (int i = 0; i < screenshotImages.Length; i++)
		{
			// Screenshot fade in
			if (revealScreenshot && i == curScreenshotIndex)
			{
				screenshotImages[i].color = new Color(1, 1, 1, Mathf.Lerp(screenshotImages[i].color.a, 1, screenshotFadeInSpeed * Time.deltaTime));
			}
			// Screenshot fade out
			else
			{
				screenshotImages[i].color = new Color(1, 1, 1, Mathf.Lerp(screenshotImages[i].color.a, 0, screenshotFadeOutSpeed * Time.deltaTime));
			}
		}
	}

	// UI Screenshot pointer stuff

	public static void OnPointerEnter(int state)
	{
		inst.isHoveringOverButton = true;
		inst.hoverButton = state;
		//inst.screenshot.texture = ScreenshotManager.getScreenshot(state);

		inst.curScreenshotIndex = state;
		inst.screenshotImages[state].texture = getScreenshot(state); 

		inst.RevealScreenshot(state);
	}

	public static void OnPointerExit()
	{
		inst.isHoveringOverButton = false;
		inst.hoverButton = -1;
		inst.HideScreenshot();
	}
}
