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

	[SerializeField] ParticleSystem[] tetherUICreateParticles; 
	[SerializeField] ParticleSystem[] tetherUIRemoveParticles;

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
	[SerializeField] List<TetherIndicator> timeTetherIndicators;

    [SerializeField] TetherUIState tetherUIState;

	[SerializeField] Player playerScript;

    [SerializeField] TetherTransition tetherTransition;
	[SerializeField] ScreenShaderTransition tetherShaderTransition; 

    [Header("Fast tether back settings")]
    [Tooltip("If true, the player can tap the tether menu key to instantly go back to the last tether point.")]
    [SerializeField]
    bool allowFastTether;
    [Tooltip("How long does the tether menu key have to be held for the menu to come up, rather than doing fast tether back on release")]
    [SerializeField]
    float fastTetherKeyTime;
    // The timer that keeps track of how long the tether menu key is held
    float fastTetherKeyTimer;

    [Header("Tether menu fading")]
    [Tooltip("The CanvasGroup for the pop up tether menu. Used to fade the entire group in/out")]
    [SerializeField]
    CanvasGroup tetherMenuGroup;
    [SerializeField] float tetherMenuFadeInSpeed = 1;
    [SerializeField] float tetherMenuFadeOutSpeed = 1;

	// If > 0, counts down and prevents the tether menu from being brought up
	public float tetherMenuDisableTimer; 

	[SerializeField] Image menuLight; 
	[SerializeField] Sprite menuLightOn; 
	[SerializeField] Sprite menuLightOff; 

    [Header("Fade image settings")]
    [Tooltip("(Drag In) The background image that darkens the background when the tether UI is up")]
    [SerializeField]
    Image fadeImage;
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
    [SerializeField]
    CanvasGroup tetherHUDGroup;
    [SerializeField] float tetherHUDFadeInSpeed;
    [SerializeField] float tetherHUDFadeOutSpeed;

	// TODO change this to false at the start of the scene and reenable once the player gets control after Amelia finishes her first placing animation
	bool tetherHUDVisible = true; 

	// Set true when the level ends and all tether indicator return objects are flying to the player. Set false again when all are collected
	bool endLevelWaitForCollection; 
	// Set true once EndLevelRemoveAllTetherPoints is called
	bool levelIsEnding; 

    bool playTMenuOpen = true;

    // Use this for initialization
    void Start()
    {
        //timeTetherIndicators = new GameObject[LevelStateManager.maxNumStates];
		timeTetherIndicators = new List<TetherIndicator>();

        CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, 0);

        arrowLerpBetween = true;
        tetherMenuGroup.alpha = 0;
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        screenshot.gameObject.SetActive(true);
    }

    void Update()
    {
        /*
		 * Create tether point functionality
		 */

        // Creating a tether point
		if (tetherUIState == TetherUIState.GAMEPLAY && !GameManager.isPlayerDead() && !GameManager.CameraIsZoomedOut() && GameManager.inst.pauseType == PauseType.NONE && !GameManager.isPlayerDashing())
        {
			if (PlayerControlManager.GetKeyDown(ControlInput.DROP_TETHER))
            {
                //CreatePoint();
				//GameManager.GetPlayer().GetComponent<Player>().setPlaceAnchorAnim();

				// Order flipped so the tether point exists for the screenshot
				// Test!
				GameManager.GetPlayer().GetComponent<Player>().setPlaceAnchorAnim();
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
			if (tetherUIState == TetherUIState.GAMEPLAY && !GameManager.isPlayerDead() && !GameManager.CameraIsZoomedOut() && !GameManager.isPlayerDashing() && !GameManager.isPaused())
            {
                // If the player presses the menu key, start the timer
                //if (Input.GetKeyDown(PlayerControlManager.LH_TetherMenu) || Input.GetKeyDown(PlayerControlManager.RH_TetherMenu))
                if (PlayerControlManager.GetKeyDown(ControlInput.TETHER_MENU))
                {
                    fastTetherKeyTimer = fastTetherKeyTime;
                }

                // If the menu button is released before the timer has reached zero, call a fast tether back
                //if (Input.GetKeyUp(PlayerControlManager.LH_TetherMenu) || Input.GetKeyUp(PlayerControlManager.RH_TetherMenu))
                if (PlayerControlManager.GetKeyUp(ControlInput.TETHER_MENU))
                {
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
		// Adding the pause menu active check prevents the cursor from getting reverted to the gameplay cursor while the pause menu is up
		if ((tetherUIState == TetherUIState.GAMEPLAY || tetherUIState == TetherUIState.TETHER_MENU) && !PauseMenuManager.pauseMenuActive)
        {
            // Test for bringing up the menu, while alive
			if (!GameManager.isPlayerDead())
            {
                // If the tether menu key is held down
				//if (PlayerControlManager.GetKey(ControlInput.TETHER_MENU) && !GameManager.CameraIsZoomedOut() && fastTetherKeyTimer <= 0 && !GameManager.isPlayerDashing())
				if (PlayerControlManager.GetKey(ControlInput.TETHER_MENU) && fastTetherKeyTimer <= 0 && tetherMenuDisableTimer == 0 && !GameManager.isPlayerDashing() && (GameManager.inst.pauseType == PauseType.NONE || GameManager.inst.pauseType == PauseType.TETHER_MENU)) 
				{
                    tetherUIState = TetherUIState.TETHER_MENU;
                    //GameManager.setPause(true);
					if (GameManager.inst.pauseType == PauseType.NONE)
					{
						GameManager.inst.EnterPauseState(PauseType.TETHER_MENU); 
					}

                    CursorManager.inst.lockCursorType = true;
                    CursorManager.inst.cursorState = CursorState.MENU;
                    ShowTetherMenu();
                }
				else
                {
                    tetherUIState = TetherUIState.GAMEPLAY;

                    //GameManager.setPause(false);
					if (GameManager.inst.pauseType == PauseType.TETHER_MENU)
					{
						GameManager.inst.ExitPauseState(); 
						//Debug.Log("ExitPauseState()"); 
					}

                    CursorManager.inst.lockCursorType = false;
                    CursorManager.inst.OnCursorBoundsUpdated();
                    HideTetherMenu();
                }

            }
            // If the player is dead, force the menu to come up
            else
            {
                tetherUIState = TetherUIState.TETHER_MENU;
                //GameManager.setPause(true);
				GameManager.inst.EnterPauseState(PauseType.TETHER_MENU); 
                CursorManager.inst.lockCursorType = true;
                CursorManager.inst.cursorState = CursorState.MENU;
                ShowTetherMenu();
            }
        }

		/*
		 * Tether Corner HUD visibility
		 */ 

		if (tetherHUDVisible)
		{
			tetherHUDGroup.alpha = Mathf.Lerp(tetherHUDGroup.alpha, 1, tetherHUDFadeInSpeed * Time.deltaTime);
		}
		else
		{
			tetherHUDGroup.alpha = Mathf.Lerp(tetherHUDGroup.alpha, 0, tetherHUDFadeOutSpeed * Time.deltaTime);
		}

        /*
		 * Tether Corner HUD fading when zooming out
		 */

        // When zooming/zoomed out, hide the bottom left time tether HUD
		// JK let's not do this
		/*
        if (GameManager.CameraIsZoomedOut())
        {
            tetherHUDGroup.alpha = Mathf.Lerp(tetherHUDGroup.alpha, 0, tetherHUDFadeOutSpeed * Time.deltaTime);
        }
        // When zooming back in or not zoomed, reveal the bottom left time tether HUD
        else
        {
            tetherHUDGroup.alpha = Mathf.Lerp(tetherHUDGroup.alpha, 1, tetherHUDFadeInSpeed * Time.deltaTime);
        }
        */ 


        /*
		 * Other Update Stuff
		 */

        UpdateTimeArrowPos();
        UpdateScreenshotState();

		if (tetherUIState == TetherUIState.TETHER_MENU)
		{
			if (menuLight != null && menuLightOn != null)
				menuLight.sprite = menuLightOn; 
		}
		else
		{
			if (menuLight != null && menuLightOff != null)
				menuLight.sprite = menuLightOff; 
		}


		// End level tether point collection
		// Once all indicator return objects have returned to the player and been destroyed, this will allow EndLevelAllTetherPointdCollected to return true
		if (endLevelWaitForCollection)
		{
			if (IndicatorReturnObject.NoInstancesExist())
			{
				endLevelWaitForCollection = false; 
			}
		}

		if (tetherMenuDisableTimer > 0)
		{
			tetherMenuDisableTimer -= Time.deltaTime; 
			if (tetherMenuDisableTimer < 0)
			{
				// Don't allow the timer to reach 0 until the tether menu key has been released
				if (PlayerControlManager.GetKey(ControlInput.TETHER_MENU))
				{
					tetherMenuDisableTimer = 0.1f; 
				}
				else
				{
					tetherMenuDisableTimer = 0f; 
				}
			}
		}
    }

    /// <summary>
    /// Called continuously to make sure the tether menu is revealed
    /// Returns true if the menu is (nearly) completely visible
    /// </summary>
    bool ShowTetherMenu()
    {
        // Only allow clicking on tether menu buttons when it's faded in
        tetherMenuGroup.blocksRaycasts = true;

        tetherMenuGroup.alpha = Mathf.Lerp(tetherMenuGroup.alpha, 1, tetherMenuFadeInSpeed * Time.deltaTime);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(fadeImage.color.a, fadeImageMaxAlpha, fadeImageFadeInSpeed * Time.deltaTime));
        /*if (playTMenuOpen)
        {
            AudioLibrary.PlayTetherMenuOpen();
            playTMenuOpen = false;
        }*/
        if (tetherMenuGroup.alpha > 0.99f && fadeImage.color.a >= fadeImageMaxAlpha - 0.01f)
        {
            tetherMenuGroup.alpha = 1;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImageMaxAlpha);
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
        // Only allow clicking on tether menu buttons when it's faded in
        tetherMenuGroup.blocksRaycasts = false;

        tetherMenuGroup.alpha = Mathf.Lerp(tetherMenuGroup.alpha, 0, tetherMenuFadeOutSpeed * Time.deltaTime);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(fadeImage.color.a, 0, fadeImageFadeOutSpeed * Time.deltaTime));
        /*if (!playTMenuOpen)
        {
            AudioLibrary.PlayTetherMenuClose();
            playTMenuOpen = true;
        }*/
        if (tetherMenuGroup.alpha < 0.01f && fadeImage.color.a < 0.01f)
        {
            tetherMenuGroup.alpha = 0;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
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

        if (tetherPoints.Length < 2 || tetherPoints[0] == null || tetherPoints[1] == null)
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
        curTimeArrow.anchoredPosition = new Vector2(xLerp, curTimeArrow.anchoredPosition.y);

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

		if (GameManager.inst.pauseType == PauseType.GAME)
		{
			return; 
		}

        tetherUIState = TetherUIState.TETHER_ANIMATION;
        //GameManager.setPause(true);
		GameManager.inst.ExitPauseState(); 
		GameManager.inst.EnterPauseState(PauseType.TETHER_TRANSITION); 

        CursorManager.inst.lockCursorType = true;
        CursorManager.inst.cursorState = CursorState.DEACTIVATED;

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
        //GameManager.setPause(true);
		GameManager.inst.ExitPauseState(); 
		GameManager.inst.EnterPauseState(PauseType.TETHER_TRANSITION); 
        CursorManager.inst.lockCursorType = true;
        CursorManager.inst.cursorState = CursorState.DEACTIVATED;

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
        AudioLibrary.PlayTetherSelect();
        tetherUIState = TetherUIState.TETHER_ANIMATION;
        //GameManager.setPause(true);
		GameManager.inst.ExitPauseState(); 
		GameManager.inst.EnterPauseState(PauseType.TETHER_TRANSITION); 
        CursorManager.inst.lockCursorType = true;
        CursorManager.inst.cursorState = CursorState.DEACTIVATED;

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
        playerScript.PlayTetherAnimation();
        AudioLibrary.PlayTetherRewindSound();

		// Play ripple out particle effect
		ParticleSystem ripples = Instantiate(Resources.Load("Prefabs/ParticlesRippleOut") as GameObject, GameManager.GetPlayer().transform).GetComponent<ParticleSystem>();
		ripples.transform.position = GameManager.GetPlayer().transform.position;
		ripples.Play(); 

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
		tetherShaderTransition.SetFadeOut(); 

        //	(2) Make the timeline arrow move directly above the previous tether point
        SetArrowTarget(stateToLoad, false, false);

        while (!ArrowReachedTarget() || tetherTransition.TransitionInProgress())
        {
            // Wait for both conditions to finish
            yield return null;
        }

		Destroy(ripples.gameObject); 

        // Load the desired state via LevelStateManager
        LoadTetherPoint(stateToLoad);
        //RemoveTimeTetherIndicator(stateToLoad + 1); 
        //LevelStateManager.loadTetherPoint(stateToLoad);

        // How long to wait on the black screen
        yield return new WaitForSeconds(0.3f);

        // Now that the state has been loaded, make the transition fade back in
        playerScript.PlayReappearAnimation();
        yield return new WaitForSeconds(0.1f);
        tetherTransition.SetFadeIn();
		tetherShaderTransition.SetFadeIn(); 

		// Play ripple in particle effect
		ripples = Instantiate(Resources.Load("Prefabs/ParticlesRippleIn") as GameObject, GameManager.GetPlayer().transform).GetComponent<ParticleSystem>();
		ripples.transform.position = GameManager.GetPlayer().transform.position; 
		ripples.Play(); 

        while (tetherTransition.TransitionInProgress())
        {
            // Wait for the transition in to finish
            yield return null;
        }

        // When the transition is done, start these simultaneous actions
        // 	(1) Make Margot play her appear animation

		ripples.enableEmission = false; 

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

		Destroy(ripples.gameObject);

        // Now that the process has finished, restore control to the player
        // TODO
        tetherUIState = TetherUIState.GAMEPLAY;
        //GameManager.setPause(false);
		GameManager.inst.ExitPauseState(); 
        CursorManager.inst.lockCursorType = false;
        CursorManager.inst.OnCursorBoundsUpdated();
    }



    public void CreatePoint()
    {
        if (LevelStateManager.canCreateTetherPoint())
        {
            //Debug.Log("Create tether point");
			CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, LevelStateManager.curState + 1);
            LevelStateManager.createTetherPoint();
            
            AudioLibrary.PlayTetherPlacementSound();
            // Timeline arrow
            SetArrowTarget(LevelStateManager.curState, false, true);

			if (LevelStateManager.curState < tetherUICreateParticles.Length && tetherUICreateParticles[LevelStateManager.curState] != null)
			{
				tetherUICreateParticles[LevelStateManager.curState].Play(); 
			}
        }
        else
        {
            if (!GlobalAudio.ClipIsPlaying(AudioLibrary.inst.tetherError))
            {
                AudioLibrary.PlayTetherErrorSound();
            }
            Debug.Log("Can't create tether point right now");
        }
    }

	/// <summary>
	/// Called when removing a specific tether point. 
	/// </summary>
	public void RemoveTetherPoint(int tetherIndex)
	{
		StartCoroutine("RemoveTetherProcess", tetherIndex); 
	}

	IEnumerator RemoveTetherProcess(int tetherIndex)
	{
		if (false)
			yield return new WaitForSeconds(1); 

		// Remove the time tether indicator gameObject
		RemoveTimeTetherIndicator(tetherIndex, false); 

		// Update the internal level state data in LevelStateManager
		//LevelStateManager.removeTetherPointAt(tetherIndex);
		if (LevelStateManager.removeTetherPointAt(tetherIndex))
		{
			Debug.Log("Removal succesful for point " + tetherIndex); 
		}
		else
		{
			Debug.Log("Could not remove point " + tetherIndex); 
		} 

		// Update the screenshots
		ScreenshotManager.removeScreenshotAt(tetherIndex); 

		// Set a new target for the timeline arrow
		SetArrowTarget(LevelStateManager.curState, true, false);
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
        RemoveTimeTetherIndicator(state + 1, true);
        LevelStateManager.loadTetherPoint(state);
    }

    void CreateTimeTetherIndicator(Vector3 pos, int state)
    {
		//Debug.Log("Create time tether indicator. State = " + state); 

        GameObject indicator = Instantiate(timeTetherIndicatorPrefab, pos, Quaternion.identity, this.transform);
		timeTetherIndicators.Add(indicator.GetComponent<TetherIndicator>()); 
		TetherIndicator newIndicator = indicator.GetComponent<TetherIndicator>();
		newIndicator.tetherIndex = state; 
		newIndicator.UpdateTetherSprite(); 
    }

    // Removes all tether indicators from timeTetherIndicators[index] to timeTetherIndicators[Length-1]
	/// <summary>
	/// Removes time tether indicators. If removeAll == true, removes all tether indicators from timeTetherIndicators[index] to timeTetherIndicators[Length-1]
	/// </summary>
	/// <param name="index">Index.</param>
	/// <param name="removeAll">If set to <c>true</c> remove all.</param>
	void RemoveTimeTetherIndicator(int index, bool removeAll)
    {
        Debug.Log("Remove indicators starting at index " + index);

		// First, test to make sure the index is in range
		if (index > timeTetherIndicators.Count - 1)
		{
			return; 
		}

		// Option 1: Only remove the indicator at the index; the others are effectively shifted
		if (!removeAll)
		{
			if (timeTetherIndicators[index] != null)
			{
				timeTetherIndicators[index].StartDestroy(); 
				//Destroy(timeTetherIndicators[index].gameObject);
				timeTetherIndicators.RemoveAt(index); 

				if (index < tetherUIRemoveParticles.Length && tetherUIRemoveParticles[index] != null)
				{
					tetherUIRemoveParticles[index].Play(); 
				}
			}
		}
		// Option 2: Remove every indicator from the index and after
		else
		{
			for (int i = timeTetherIndicators.Count - 1; i >= index; i--)
			{
				if (timeTetherIndicators[i] != null)
				{
					timeTetherIndicators[i].StartDestroy(); 
					//Destroy(timeTetherIndicators[i].gameObject);
					timeTetherIndicators.RemoveAt(i); 
				}

				if (i < tetherUIRemoveParticles.Length && tetherUIRemoveParticles[i] != null)
				{
					tetherUIRemoveParticles[i].Play(); 
				}
			}
		}

		// Iterate through each indicator to update its internal index
		for (int i = 0; i < timeTetherIndicators.Count; i++)
		{
			if (timeTetherIndicators[i] != null)
			{
				timeTetherIndicators[i].tetherIndex = i; 
			}
		}
    }

	/// <summary>
	/// Causes all tether points, including the first, to enter their destroy animation
	/// </summary>
	public void EndLevelRemoveAllTetherPoints()
	{
		tetherHUDVisible = false; 
		RemoveTimeTetherIndicator(0, true);
		endLevelWaitForCollection = true; 
		levelIsEnding = true; 
	}

	public bool EndLevelAllTetherPointsCollected()
	{
		if (levelIsEnding && endLevelWaitForCollection == false)
		{
			return true; 
		}
		return false; 
	}

	/// <summary>
	/// Returns the position of the specified time tether indicator point. Necessary for setting player position
	/// </summary>
	/// <returns>The time tether indicator position.</returns>
	/// <param name="index">Index of the timeTetherIndicators array</param>
	public Vector3 GetTimeTetherIndicatorPos(int index)
	{
		if (index < timeTetherIndicators.Count && timeTetherIndicators[index] != null)
		{
			return timeTetherIndicators[index].transform.position; 
		}
		Debug.LogError("Could not retrieve timeTetherIndicators at index " + index + ". Vector3.zero has been returned instead."); 
		return Vector3.zero; 
	}

	/// <summary>
	/// Forces all positions of time tether indicators to update
	/// This needs to be used during the state load process to make sure tether positions update as if their moveParent position changes
	/// </summary>
	public void UpdateTetherIndicatorPositions()
	{
		for (int i = 0; i < timeTetherIndicators.Count; i++)
		{
			if (timeTetherIndicators[i] != null)
			{
				timeTetherIndicators[i].UpdatePosition(); 
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
            screenshot.color = new Color(1, 1, 1, Mathf.Lerp(screenshot.color.a, 1, screenshotFadeInSpeed * Time.deltaTime));
        }
        // Screenshot fade out
        else
        {
            screenshot.color = new Color(1, 1, 1, Mathf.Lerp(screenshot.color.a, 0, screenshotFadeOutSpeed * Time.deltaTime));
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

	/// <summary>
	/// The PauseMenuManager calls this function to check that bringing up the pause menu is allowed. 
	/// </summary>
	public static bool PauseMenuAllowed()
	{
		if (inst.tetherUIState == TetherUIState.GAMEPLAY)
		{
			return true; 
		}

		return false; 
	}

	/// <summary>
	/// Returns true if the cursor is over one of the player's stasis bubbles
	/// </summary>
	/// <returns><c>true</c>, if over a bubble, <c>false</c> otherwise.</returns>
	public static bool CursorIsOverATetherPoint()
	{
		for (int i = 0; i < inst.timeTetherIndicators.Count; i++)
		{
			if (inst.timeTetherIndicators[i].MouseIsOver() && inst.timeTetherIndicators[i].allowKeyRemoval)
			{
				return true; 
			}
		}
		return false; 
	}



}
