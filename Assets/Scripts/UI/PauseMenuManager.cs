using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PauseMenuManager : Singleton<PauseMenuManager> 
{
	[Header("Vertical Slice Setting")]
	[Tooltip("(For vertical slice) If true, the pause menu cannot be brought up")]
	[SerializeField] bool disablePauseMenu;

	// true if the menu should be visible/appearing; false otherwise
	bool m_pauseMenuActive; 
	public static bool pauseMenuActive
	{
		get{
			return inst.m_pauseMenuActive; 
		}
	}

	[Header("Fade speeds")] 
	[Tooltip("How quickly the pause menu fades in once the pause button is pressed")] 
	[SerializeField] float menuFadeInSpeed = 1; 
	[Tooltip("How quickly the pause menu fades out")] 
	[SerializeField] float menuFadeOutSpeed = 1; 

	[Header("Object references")] 
	[SerializeField] CanvasGroup pauseCanvasGroup;  
	[SerializeField] GameObject pausePanelParent; 

	void Start () 
	{
		pauseCanvasGroup.alpha = 0; 
		pauseCanvasGroup.blocksRaycasts = false; 
		pausePanelParent.SetActive(true); 
	}

	void Update () 
	{
		// Check for key input for bring up/hiding pause menu
		if (PlayerControlManager.GetKeyDown(ControlInput.PAUSE_MENU) && TetherManager.PauseMenuAllowed() && !disablePauseMenu)
		{
			if (!m_pauseMenuActive)
			{
				EnablePauseMenu(); 
			}
			else
			{
				DisablePauseMenu(); 
			}
		}

		if (m_pauseMenuActive)
		{
			// Continuously fade menu in
			pauseCanvasGroup.alpha = Mathf.Lerp(pauseCanvasGroup.alpha, 1, menuFadeInSpeed * Time.deltaTime); 
		}
		else
		{
			// Continuously fade menu out
			pauseCanvasGroup.alpha = Mathf.Lerp(pauseCanvasGroup.alpha, 0, menuFadeOutSpeed * Time.deltaTime); 
		}
	}

	/// <summary>
	/// When the continue button is pressed to hide the pause menu
	/// </summary>
	public void OnContinueButton()
	{
		if (disablePauseMenu)
			return; 

		if (!m_pauseMenuActive)
		{
			Debug.LogError("Shouldn't be able to press the continue button while pauseMenuActive == false"); 
			return; 
		}

		DisablePauseMenu(); 
	}

	/// <summary>
	/// Called in the frame that the pause menu should be enabled
	/// </summary>
	void EnablePauseMenu()
	{
		// TODO play menu appear sound here 

		m_pauseMenuActive = true; 
		pauseCanvasGroup.blocksRaycasts = true; 
		GameManager.setPause(true); 
		GameManager.setPauseLock(true); 
		CursorManager.inst.lockCursorType = true;
		CursorManager.inst.cursorState = CursorState.MENU;
	}

	/// <summary>
	/// Called in the frame that the pause menu should be disabled
	/// </summary>
	void DisablePauseMenu()
	{
		// TODO play menu disappear sound here

		m_pauseMenuActive = false; 
		pauseCanvasGroup.blocksRaycasts = false; 
		GameManager.setPause(false); 
		GameManager.setPauseLock(false); 
		CursorManager.inst.lockCursorType = false;
	}


}
