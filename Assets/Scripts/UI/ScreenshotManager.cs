using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotManager : Singleton<ScreenshotManager> 
{
	public Camera screenCam; 

	[SerializeField] List<RenderTexture> screenshots; 

	public int shotWidth; 
	public int shotHeight;
	public int shotDepth; 

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
	}
		

	public static void createScreenshot(int stateNum)
	{
		if (inst == null)
			return; 

		inst.screenCam.targetTexture = inst.screenshots[stateNum]; 

		inst.screenCam.enabled = true; 
		inst.screenCam.Render();
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
}
