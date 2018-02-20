using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum TetherUIState
{
	GAMEPLAY,
	TETHER_MENU,
	TETHER_ANIMATION
}; 

/// <summary>
/// Manages interaction with time tether systems in LevelStateManager and the time tether UI systems
/// </summary>
public class TetherManager : Singleton<TetherManager> 
{
	[Header("Tether UI references")]
	[Tooltip("(Drag In) The prefab for the object dropped at the player position each time a tether point is created.")]
	public GameObject timeTetherIndicatorPrefab; 

	[Tooltip("(Drag In) The arrow that moves along the timeline")]
	public RectTransform curTimeArrow; 
	public RectTransform[] tetherPoints; 

	// A normalized value between 0 and 1 that tells the arrow how close it should be to its target position
	float arrowMoveProgress; 

	// The target position the arrow is lerping towards
	float arrowXTarget;

	[Header("Misc settings/refs")]
	[Tooltip("How fast the arrow lerps.")]
	public float arrowLerpSpeed; 

	// Which tetherPoint is the arrow lerping to
	int arrowLerpStateIndex; 

	// If true, the arrow lerps to a position in front of the arrowLerpStateIndex (halfway to the next tether point)
	bool arrowLerpBetween; 

	// If true, once the arrow reaches a position directly above a tether point, it will continue to the next position between the current and next point
	bool continueToLerpBetween;

	// An array holding all the timeTetherIndicators spawned in the scene. 
	GameObject[] timeTetherIndicators;  

	[SerializeField] TetherUIState tetherUIState; 

	[SerializeField] TempPlayerScript tempTetherScript; 

	[SerializeField] TetherTransition tetherTransition; 

	[Header("Fast tether back settings")]
	[Tooltip("If true, the player can tap the tether menu key to instantly go back to the last tether point.")]
	[SerializeField] bool allowFastTether; 
	[Tooltip("How long does the tether menu key have to be held for the menu to come up, rather than doing fast tether back on release")]
	[SerializeField] float fastTetherKeyTime; 
	// The timer that keeps track of how long the tether menu key is held
	float fastTetherKeyTimer; 

	[Header("Tether menu fading")]
	[Tooltip("The CanvasGroup for the pop up tether menu. Used to fade the entire group in/out")]
	[SerializeField] CanvasGroup tetherMenuGroup; 
	[SerializeField] float tetherMenuFadeInSpeed = 1; 
	[SerializeField] float tetherMenuFadeOutSpeed = 1; 

	[Header("Fade image settings")]
	[Tooltip("(Drag In) The background image that darkens the background when the tether UI is up")]
	[SerializeField] Image fadeImage; 
	[SerializeField] float fadeImageMaxAlpha;
	[SerializeField] float fadeImageFadeInSpeed; 
	[SerializeField] float fadeImageFadeOutSpeed; 

	// UI screenshot
	[Header("Screenshot settings")]
	public RawImage screenshot; 
	[HideInInspector] public bool isHoveringOverButton; 
	int hoverButton; 
	bool revealScreenshot; 
	public float screenshotFadeInSpeed = 1; 
	public float screenshotFadeOutSpeed = 1; 

	// Fade out left corner UI when zooming out
	[Header("Corner HUD fading")]
	[SerializeField] CanvasGroup tetherHUDGroup; 
	[SerializeField] float tetherHUDFadeInSpeed; 
	[SerializeField] float tetherHUDFadeOutSpeed; 


	// Use this for initialization
	void Start () 
	{
		timeTetherIndicators = new GameObject[LevelStateManager.maxNumStates]; 
		CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, 0); 

		arrowLerpBetween = true; 
		tetherMenuGroup.alpha = 0; 
		fadeImage.gameObject.SetActive(true); 
		fadeImage.color = new Color (fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
		screenshot.gameObject.SetActive(true); 
	}

	void Update()
	{
		/*
		 * Create tether point functionality
		 */ 

		// Creating a tether point
		if (tetherUIState == TetherUIState.GAMEPLAY && !GameManager.isPlayerDead())
		{
			if (Input.GetKeyDown(PlayerControlManager.RH_DropTether) || Input.GetKeyDown(PlayerControlManager.LH_DropTether))
			{ 
				CreatePoint();
			}
		}


		/*
		 * Fast tethering functionality 
		 */ 

		// Keep track of the fast tether back key
		if (allowFastTether)
		{
			// These conditions determine whether tethering is allowed
			if (tetherUIState == TetherUIState.GAMEPLAY && !GameManager.isPlayerDead() && !GameManager.CameraIsZoomedOut())
			{
				// If the player presses the menu key, start the timer
				if (Input.GetKeyDown(PlayerControlManager.LH_TetherMenu) || Input.GetKeyDown(PlayerControlManager.RH_TetherMenu))
				{
					fastTetherKeyTimer = fastTetherKeyTime; 
				}

				// If the menu button is released before the timer has reached zero, call a fast tether back
				if (Input.GetKeyUp(PlayerControlManager.LH_TetherMenu) || Input.GetKeyUp(PlayerControlManager.RH_TetherMenu))
				{
					//Debug.Log("fast tether back (TODO)"); 
					//LoadTetherPoint(LevelStateManager.curState); 
					OnFastTetherStart(LevelStateManager.curState); 
				}
			}
		}
		else
		{
			fastTetherKeyTimer = 0; 
		}

		// Update fastTetherKeyTimer
		if (fastTetherKeyTimer > 0)
		{
			fastTetherKeyTimer -= Time.deltaTime;
			if (fastTetherKeyTimer < 0)
				fastTetherKeyTimer = 0; 
		}


		/*
		 * Tether menu functionality 
		 */ 

		// Bringing up the tether menu
		if (tetherUIState == TetherUIState.GAMEPLAY || tetherUIState == TetherUIState.TETHER_MENU)
		{
			// Test for bringing up the menu, while alive
			if (!GameManager.isPlayerDead())
			{
				// If the tether menu key is held down
				if ((Input.GetKey(PlayerControlManager.RH_TetherMenu) || Input.GetKey(PlayerControlManager.LH_TetherMenu)) && !GameManager.CameraIsZoomedOut() && fastTetherKeyTimer <= 0)
				{
					tetherUIState = TetherUIState.TETHER_MENU; 
					GameManager.setPause(true); 
					GameManager.inst.lockCursorType = true; 
					GameManager.inst.cursorType = CursorType.UI_HOVER; 
					ShowTetherMenu();
				}
				else
				{
					tetherUIState = TetherUIState.GAMEPLAY; 
					GameManager.setPause(false); 
					GameManager.inst.lockCursorType = false; 
					GameManager.inst.OnCursorBoundsUpdated(); 
					HideTetherMenu(); 
				}

			}
			// If the player is dead, force the menu to come up
			else
			{
				tetherUIState = TetherUIState.TETHER_MENU; 
				GameManager.setPause(true); 
				GameManager.inst.lockCursorType = true; 
				GameManager.inst.cursorType = CursorType.UI_HOVER; 
				ShowTetherMenu();
			}
		}

		UpdateTimeArrowPos(); 
		UpdateScreenshotState(); 
	}

	/// <summary>
	/// Called continuously to make sure the tether menu is revealed
	/// Returns true if the menu is (nearly) completely visible
	/// </summary>
	bool ShowTetherMenu()
	{
		tetherMenuGroup.alpha = Mathf.Lerp(tetherMenuGroup.alpha, 1, tetherMenuFadeInSpeed * Time.deltaTime); 
		fadeImage.color = new Color (fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(fadeImage.color.a, fadeImageMaxAlpha, fadeImageFadeInSpeed * Time.deltaTime)); 

		if (tetherMenuGroup.alpha > 0.99f && fadeImage.color.a >= fadeImageMaxAlpha - 0.01f)
		{
			tetherMenuGroup.alpha = 1;
			fadeImage.color = new Color (fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImageMaxAlpha); 
			return true;
		}
		return false; 
	}

	/// <summary>
	/// Called continuously to make sure the tether menu is hidden
	/// Returns true if the menu is (nearly) hidden
	/// </summary>
	bool HideTetherMenu()
	{
		tetherMenuGroup.alpha = Mathf.Lerp(tetherMenuGroup.alpha, 0, tetherMenuFadeOutSpeed * Time.deltaTime);
		fadeImage.color = new Color (fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(fadeImage.color.a, 0, fadeImageFadeOutSpeed * Time.deltaTime)); 

		if (tetherMenuGroup.alpha < 0.01f && fadeImage.color.a < 0.01f)
		{
			tetherMenuGroup.alpha = 0;
			fadeImage.color = new Color (fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0); 
			return true;
		}
		return false; 
	}

	/// <summary>
	/// Called continuously to choose where the time arrow should lerp
	/// </summary>
	void UpdateTimeArrowPos()
	{
		// Error checking
		if (arrowLerpStateIndex >= tetherPoints.Length || arrowLerpStateIndex < 0)
		{
			Debug.LogError("Invalid arrowLerpStateIndex of " + arrowLerpStateIndex + ". tetherPoints.Length = " + tetherPoints.Length); 
			return; 
		}

		if (tetherPoints.Length < 2 || tetherPoints[0] == null || tetherPoints[1] == null )
		{
			Debug.LogError("tetherPoints has null values or is an incorrect length. tetherPoints.Length = " + tetherPoints.Length); 
			return; 
		}

		arrowXTarget = tetherPoints[arrowLerpStateIndex].anchoredPosition.x;

		// If the arrow should move between tether points, add an x offset from calculating the distance between the first two tether points 
		//	 (the distance should be constant between all points)
		if (arrowLerpBetween)
		{
			arrowXTarget += Mathf.Abs(tetherPoints[0].anchoredPosition.x - tetherPoints[1].anchoredPosition.x) / 2; 
		}

		// Lerp the arrow to its target anchored position
		// Set the RectTransform's position, lerping to the target
		float xLerp = Mathf.Lerp(curTimeArrow.anchoredPosition.x, arrowXTarget, arrowMoveProgress); 
		curTimeArrow.anchoredPosition = new Vector2 (xLerp, curTimeArrow.anchoredPosition.y);

		// Lerp the arrowMoveprogress
		if (arrowMoveProgress < 1)
		{
			arrowMoveProgress = Mathf.Lerp(arrowMoveProgress, 1, arrowLerpSpeed * Time.deltaTime); 

			if (ArrowReachedTarget())
			{
				arrowMoveProgress = 1; 
			}
		}
		else if (continueToLerpBetween)
		{
			continueToLerpBetween = false; 
			SetArrowTarget(arrowLerpStateIndex, true, false); 
		}
	}

	/// <summary>
	/// You should explicitly call this function to set a new arrow target. It takes care of resetting all the arrow properties
	/// _continueToLerpBetween should be false UNLESS _arrowLerpBetween = false AND the arrow should move on to the middle once it reaches the target tether point pos
	/// </summary>
	void SetArrowTarget(int _arrowLerpStateIndex, bool _arrowLerpBetween, bool _continueToLerpBetween)
	{
		arrowLerpStateIndex = _arrowLerpStateIndex; 
		arrowLerpBetween = _arrowLerpBetween; 
		arrowMoveProgress = 0; 

		// Redundant b/c of same line in UpdateTimeArrowPos(), but used to prevent a order of operations error with ArrowReachedTarget
		arrowXTarget = tetherPoints[arrowLerpStateIndex].anchoredPosition.x;

		if (_arrowLerpBetween == true && continueToLerpBetween == true)
		{
			Debug.LogWarning("Set arrow target parameters issue: You shouldn't set _continueToLerpBetween = true if _arrowLerpBetween is also true"); 
		}
		continueToLerpBetween = _continueToLerpBetween; 
	}
		

	/// <summary>
	/// Returns true if the timeline arrow has approximately reached the target x position.
	/// </summary>
	bool ArrowReachedTarget()
	{
		if (Mathf.Abs(curTimeArrow.anchoredPosition.x - arrowXTarget) < 0.01f)
		{
			return true; 
		}
		return false; 
	}

	/// <summary>
	/// Tether Button click for tether buttons in the bottom left tether UI
	/// </summary>
	/// <param name="stateToLoad">LevelStateManager state to load, starting at 0.</param>
	[System.Obsolete("Not used anymore b/c you can no longer click the buttons on the bottom left TimeTetherUI")]
	public void OnTetherGameplayButtonClick(int stateToLoad)
	{
		if (tetherUIState != TetherUIState.GAMEPLAY)
		{
			Debug.LogError("Shouldn't be able to click gameplay tether button when not in TetherUIState.GAMEPLAY. Currently in TetherUIState." + tetherUIState); 
			return; 
		}
			
		tetherUIState = TetherUIState.TETHER_ANIMATION; 
		GameManager.setPause(true);
		GameManager.inst.lockCursorType = true; 
		GameManager.inst.cursorType = CursorType.DEACTIVATED; 

		// Start the animation coroutine that jumps directly into the tether animation code
		StartCoroutine("TetherBackAnimation", stateToLoad); 
	}

	/// <summary>
	/// Begins the tether back sequence, skipping the tether menu animation part of the coroutine
	/// Essentially the same as OnTetherGameplayButtonClick(int stateToLoad), but repurposed for tapping the tether menu key instead of pressing a tether button in the bottom left UI
	/// </summary>
	void OnFastTetherStart(int stateToLoad)
	{
		if (tetherUIState != TetherUIState.GAMEPLAY)
		{
			Debug.LogError("Shouldn't be able to fast load tether point when not in TetherUIState.GAMEPLAY. Currently in TetherUIState." + tetherUIState); 
			return; 
		}

		tetherUIState = TetherUIState.TETHER_ANIMATION; 
		GameManager.setPause(true);
		GameManager.inst.lockCursorType = true; 
		GameManager.inst.cursorType = CursorType.DEACTIVATED; 

		// Start the animation coroutine that jumps directly into the tether animation code
		StartCoroutine("TetherBackAnimation", stateToLoad); 
	}

	/// <summary>
	/// Tether Button click for tether buttons in pop up menu
	/// </summary>
	/// <param name="stateToLoad">LevelStateManager state to load, starting at 0.</param>
	public void OnTetherMenuButtonClick(int stateToLoad)
	{
		if (tetherUIState != TetherUIState.TETHER_MENU)
		{
			Debug.LogError("Shouldn't be able to click menu tether button when not in TetherUIState.TETHER_MENU. Currently in TetherUIState." + tetherUIState); 
			return; 
		}

		tetherUIState = TetherUIState.TETHER_ANIMATION; 
		GameManager.setPause(true); 
		GameManager.inst.lockCursorType = true; 
		GameManager.inst.cursorType = CursorType.DEACTIVATED; 

		// Start the animation coroutine that begins with the hide menu code
		StartCoroutine("TetherBackAnimation_HideMenu", stateToLoad); 
	}

	IEnumerator TetherBackAnimation_HideMenu(int stateToLoad)
	{
		//Debug.Log("TetherBackAnimation_HideMenu coroutine started");

		// If clicking the button from the tether menu, we first need to take care of making the menu disappear

		// Wait for the menu to finish disappearing
		while (!HideTetherMenu())
		{
			yield return null; 
		}

		// Once the tether menu is hidden, go to the second part of the tether animation
		yield return StartCoroutine("TetherBackAnimation", stateToLoad); 
	}

	IEnumerator TetherBackAnimation(int stateToLoad)
	{
		//Debug.Log("TetherBackAnimation coroutine started"); 

		// The tether menu should be gone by this point

		// Make Margot play her tether animation
		// start animation
		tempTetherScript.PlayTetherAnimation();
        AudioLibrary.PlayTetherRewindSound();

		// Temporary way of delaying; should eventually have the animation controller tell this script that Margot's animation has finished
		yield return new WaitForSeconds(0.5f); 

		/*
		while (false)
		{
			// 	Wait for Margot's animation to finish
			yield return null; 
		}
		*/ 

		// Next, start two simultaneous actions
		// 	(1) Make the screen transition play
		tetherTransition.SetFadeOut(); 

		//	(2) Make the timeline arrow move directly above the previous tether point
		SetArrowTarget(stateToLoad, false, false); 

		while (!ArrowReachedTarget() || tetherTransition.TransitionInProgress())
		{
			// Wait for both conditions to finish
			yield return null; 
		}

		// Load the desired state via LevelStateManager
		LoadTetherPoint(stateToLoad); 
		//RemoveTimeTetherIndicator(stateToLoad + 1); 
		//LevelStateManager.loadTetherPoint(stateToLoad);

		// How long to wait on the black screen
		yield return new WaitForSeconds (0.3f);

		// Now that the state has been loaded, make the transition fade back in
		tempTetherScript.PlayReappearAnimation();
		yield return new WaitForSeconds (0.1f);
		tetherTransition.SetFadeIn(); 

		while (tetherTransition.TransitionInProgress())
		{
			// Wait for the transition in to finish
			yield return null; 
		}
			
		// When the transition is done, start two simultaneous actions
		// 	(1) Make Margot play her appear animation

		yield return new WaitForSeconds(0.2f); 

		//	(2) Move the timeline arrow to the middle position in front of the tether point we just reverted to
		SetArrowTarget(stateToLoad, true, false);

		// TODO need compound boolean
		while (!ArrowReachedTarget())
		{
			// Wait for both conditions to finish
			yield return null; 
		}

		yield return new WaitForSeconds(0.5f);

		// Now that the process has finished, restore control to the player
		// TODO
		tetherUIState = TetherUIState.GAMEPLAY; 
		GameManager.setPause(false); 
		GameManager.inst.lockCursorType = false; 
		GameManager.inst.OnCursorBoundsUpdated();
	}



	public void CreatePoint()
	{
		if (LevelStateManager.canCreateTetherPoint())
		{
			Debug.Log("Create tether point"); 
			LevelStateManager.createTetherPoint(); 
			CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, LevelStateManager.curState);
            AudioLibrary.PlayTetherPlacementSound();
            // Timeline arrow
            SetArrowTarget(LevelStateManager.curState, false, true); 
		}
		else
		{
            AudioLibrary.PlayTetherErrorSound();
			Debug.Log("Can't create tether point right now"); 
		}
	}

	/// <summary>
	/// Called via UI buttons in the timeline to load a state
	/// </summary>
	/// <param name="state">State, as determined in the button</param>
	/*
	public void LoadPoint(int state)
	{
		RemoveTimeTetherIndicator(state + 1); 
		LevelStateManager.loadTetherPoint(state);

		// Temporary fix!!!
		GameManager.setPause(false);
	}
	*/ 

	void LoadTetherPoint(int state)
	{
		RemoveTimeTetherIndicator(state + 1); 
		LevelStateManager.loadTetherPoint(state);
	}

	void CreateTimeTetherIndicator(Vector3 pos, int state)
	{
		GameObject indicator = Instantiate(timeTetherIndicatorPrefab, pos, Quaternion.identity, this.transform); 
		//timeTetherIndicators.Add(indicator); 
		timeTetherIndicators[state] = indicator; 
	}

	// Removes all tether indicators from timeTetherIndicators[index] to timeTetherIndicators[Length-1]
	void RemoveTimeTetherIndicator(int index)
	{
		//Debug.Log("Remove indicators starting at index " + index); 

		for (int i = index; i < timeTetherIndicators.Length; i++)
		{
			if (timeTetherIndicators[i] != null)
			{
				Destroy(timeTetherIndicators[i].gameObject);
			}
		}
	}

	// UI Screenshot stuff

	public static void OnPointerEnter(int state)
	{
		inst.isHoveringOverButton = true;
		inst.hoverButton = state; 
		inst.screenshot.texture = ScreenshotManager.getScreenshot(state); 
		inst.RevealScreenshot();
	}

	public static void OnPointerExit()
	{
		inst.isHoveringOverButton = false; 
		inst.hoverButton = -1; 
		inst.HideScreenshot(); 
	}

	void RevealScreenshot()
	{
		//screenshot.color = new Color (1, 1, 1, 1); 
		revealScreenshot = true; 
	}

	void HideScreenshot()
	{
		//screenshot.color = new Color (1, 1, 1, 0); 
		revealScreenshot = false; 
	}

	void UpdateScreenshotState()
	{
		// Screenshot fade in
		if (revealScreenshot)
		{
			screenshot.color = new Color (1, 1, 1, Mathf.Lerp(screenshot.color.a, 1, screenshotFadeInSpeed * Time.deltaTime)); 
		}
		// Screenshot fade out
		else
		{
			screenshot.color = new Color (1, 1, 1, Mathf.Lerp(screenshot.color.a, 0, screenshotFadeOutSpeed * Time.deltaTime)); 
		}
	}
		
	/// <summary>
	/// The camera calls this function to check that zoom out is allowed. This will return false if the TetherManager is currently in the menu or an animation
	/// </summary>
	public static bool ZoomOutAllowed()
	{
		if (inst.tetherUIState == TetherUIState.GAMEPLAY)
		{
			return true; 
		}

		return false; 
	}




}
