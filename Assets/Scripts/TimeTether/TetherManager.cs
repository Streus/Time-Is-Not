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
	[Tooltip("(Drag In) The prefab for the object dropped at the player position each time a tether point is created.")]
	public GameObject timeTetherIndicatorPrefab; 

	[Tooltip("(Drag In) The arrow that moves along the timeline")]
	public RectTransform curTimeArrow; 
	public RectTransform[] tetherPoints; 

	// A normalized value between 0 and 1 that tells the arrow how close it should be to its target position
	float arrowMoveProgress; 

	// The target position the arrow is lerping towards
	float arrowXTarget;

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

	[Tooltip("The CanvasGroup for the pop up tether menu. Used to fade the entire group in/out")]
	[SerializeField] CanvasGroup tetherMenuGroup; 
	[SerializeField] float tetherMenuFadeInSpeed = 1; 
	[SerializeField] float tetherMenuFadeOutSpeed = 1; 

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

	// Use this for initialization
	void Start () 
	{
		timeTetherIndicators = new GameObject[LevelStateManager.maxNumStates]; 
		CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, 0); 

		arrowLerpBetween = true; 
		tetherMenuGroup.alpha = 0; 
		fadeImage.gameObject.SetActive(true); 
		fadeImage.color = new Color (fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0); 
	}

	void Update()
	{
		// Creating a tether point
		if (tetherUIState == TetherUIState.GAMEPLAY && !GameManager.isPlayerDead())
		{
			if (Input.GetKeyDown(PlayerControlManager.RH_DropTether) || Input.GetKeyDown(PlayerControlManager.LH_DropTether))
			{ 
				CreatePoint(); 
			}
		}

		// Bringing up the tether menu
		if (tetherUIState == TetherUIState.GAMEPLAY || tetherUIState == TetherUIState.TETHER_MENU)
		{
			// Test for bringing up the menu, while alive
			if (!GameManager.isPlayerDead())
			{
			
				if (Input.GetKey(PlayerControlManager.RH_TetherMenu) || Input.GetKey(PlayerControlManager.LH_TetherMenu))
				{
					tetherUIState = TetherUIState.TETHER_MENU; 
					GameManager.setPause(true); 
					ShowTetherMenu();
				}
				else
				{
					tetherUIState = TetherUIState.GAMEPLAY; 
					GameManager.setPause(false); 
					HideTetherMenu(); 
				}

			}
			// If the player is dead, force the menu to come up
			else
			{
				tetherUIState = TetherUIState.TETHER_MENU; 
				GameManager.setPause(true); 
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
	public void OnTetherGameplayButtonClick(int stateToLoad)
	{
		if (tetherUIState != TetherUIState.GAMEPLAY)
		{
			Debug.LogError("Shouldn't be able to click gameplay tether button when not in TetherUIState.GAMEPLAY. Currently in TetherUIState." + tetherUIState); 
			return; 
		}
			
		tetherUIState = TetherUIState.TETHER_ANIMATION; 
		GameManager.setPause(true); 

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
		RemoveTimeTetherIndicator(stateToLoad + 1); 
		LevelStateManager.loadTetherPoint(stateToLoad);

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

		yield return new WaitForSeconds(1.2f);

		// Now that the process has finished, restore control to the player
		// TODO
		tetherUIState = TetherUIState.GAMEPLAY; 
		GameManager.setPause(false); 
	}



	public void CreatePoint()
	{
		if (LevelStateManager.canCreateTetherPoint())
		{
			Debug.Log("Create tether point"); 
			LevelStateManager.createTetherPoint(); 
			CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, LevelStateManager.curState);

			// Timeline arrow
			SetArrowTarget(LevelStateManager.curState, false, true); 
		}
		else
		{
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
		













	/*
	[Header("UI Settings - better not to touch")]
	// Time tether UI
	[Tooltip("(Drag In) The GameObject with the tether UI that zooms between the corner and the screen center")]
	public RectTransform tetherUIParent; 

	[Tooltip("The position of the tetherUIParent when at the bottom corner of the screen.")]
	public Vector2 tetherUIPos0;

	[Tooltip("The position of the tetherUIParent when zoomed in at the screen center")]
	public Vector2 tetherUIPos1; 

	[Tooltip("The localScale of the tetherUIParent when at the bottom corner of the screen")]
	public Vector3 tetherUIScale0;

	[Tooltip("The localScale of the tetherUIParent when zoomed in at the screen center")]
	public Vector3 tetherUIScale1; 

	[Tooltip("The speed that the tether UI moves/scales to the center of the screen")]
	public float tetherUIZoomUpSpeed = 1; 

	[Tooltip("The speed that the tether UI moves/scales down from the center of the screen")]
	public float tetherUIZoomDownSpeed = 1; 

	[Tooltip("(Drag In) The background image that darkens the background when the tether UI is up")]
	public Image fadeImage; 

	[Tooltip("The alpha value of fadeImage when fully faded in (0 - 1)")]
	[Range(0,1)] public float fadeImageMaxAlpha; 

	// UI state booleans

	// When the arrow is moving back after clicking to load a state
	bool arrowMovingBack;  

	// Set to true after loading a state forces the UI to minimize; 
	// prevents the UI from immediately popping back up while bringUpTetherUIKey is still held
	bool tetherZoomKeyLock; 

	// True if the tether UI is in the lower left corner
	bool tetherUINotZoomed; 

	// Used in UpdateTetherTimeline to determine whether the arrow is lerping above or in-between nodes
	bool arrowReachedPointTarget;

	// Used in UpdateTetherTimeline to hold the target position the arrow is lerping to
	float xTarget;

	[Tooltip("(Drag In) The nodes (also buttons) that represent each state in the UI tether timeline")]
	public Image[] ui_tetherPoints; 

	[Tooltip("The color of ui_tetherPoints when holding a state")]
	public Color ui_pointActiveColor; 

	[Tooltip("The color of ui_tetherPoints when not holding a state")]
	public Color ui_pointInactiveColor; 

	[Tooltip("(Drag In) The arrow that moves along the timeline")]
	public GameObject curTimeArrow; 

	[Tooltip("(Drag In) The prefab for the object dropped at the player position each time a tether point is created.")]
	public GameObject timeTetherIndicatorPrefab; 

	// An array holding all the timeTetherIndicators spawned in the scene. 
	GameObject[] timeTetherIndicators; 

	// UI screenshot
	public GameObject screenshotParent; 
	public GameObject screenshot; 
	RawImage screenshotImage; 

	public bool isHoveringOverButton; 
	int hoverButton; 

	[Header("Keycode tray")]
	[Tooltip("(Drag In) The keycode tray object")]
	public Image keycodeDrawer; 

	[Tooltip("The tray's starting position")]
	public Vector2 keycodeTrayStartPos; 
	[Tooltip("The step, in x rectTransform units, that the tray should move out per keyCode collected")]
	public float trayXPosStep; 

	[Header("Stasis UI")]
	[Tooltip("(Drag In) The stasis UI piece of the time tether that displays stasis UI")]
	public GameObject stasisPieceParent; 

	// Use this for initialization
	void Start () 
	{
		fadeImage.gameObject.SetActive(true); 
		timeTetherIndicators = new GameObject[LevelStateManager.maxNumStates]; 

		if (screenshot != null)
			screenshotImage = screenshot.GetComponent<RawImage>(); 
		HideScreenshotParent();
		HideScreenshot(); 
		CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, 0); 

		UpdateStasisDisplay(); 
	}

    // Update is called once per frame
    void Update() 
	{
		UpdateUIZoomState(); 

		// This is where the game detects when the player wants to create a tether point
		// Restrictions: player can't be dead, arrow can't be moving between points, and timeline must be in corner
		// TODO: can't restrict while game is paused, but need to find another way to prevent creation when a pause menu is up
		if (Input.GetKeyDown(PlayerControlManager.RH_DropTether) || Input.GetKeyDown(PlayerControlManager.LH_DropTether))
		{ 
			if (!GameManager.isPlayerDead() && arrowReachedPointTarget && tetherUINotZoomed)
			{
				CreatePoint(); 
			}
		}

		UpdateTetherTimelineUI();

		if (Input.GetKey(KeyCode.G))
		{
			RevealDrawer(); 
		}
		else
		{
			HideDrawer(); 
		}
	}

	/// <summary>
	/// Updates other aspects of the UI state, including the zoom of the tether UI timeline
	/// </summary>
	void UpdateUIZoomState()
	{
		// If the player isn't dead, they can bring up the tether timeline by holding the bringUpTetherUIKey
		if (!GameManager.isPlayerDead())
		{
			// Make the timeline zoom and move up to the center
			// Can happen in two cases
			// 		(a) The bringUpTetherUIKey is held down and the ability to bring up the UI isn't locked
			//		(b) The arrow is moving back after loading a state, preventing the UI from minimizing in the else statement
			if (((Input.GetKey(PlayerControlManager.RH_TetherMenu) || Input.GetKey(PlayerControlManager.LH_TetherMenu)) && !tetherZoomKeyLock) || arrowMovingBack)
			{
				GameManager.setPause(true);
				//tetherUIParent.transform.localPosition = Vector3.Lerp(tetherUIParent.transform.localPosition, tetherUIPos1, tetherUIZoomUpSpeed); 
				tetherUIParent.anchoredPosition = Vector2.Lerp(tetherUIParent.anchoredPosition, tetherUIPos1, tetherUIZoomUpSpeed); 
				tetherUIParent.transform.localScale = Vector3.Lerp(tetherUIParent.transform.localScale, tetherUIScale1, tetherUIZoomUpSpeed); 
				fadeImage.CrossFadeAlpha(fadeImageMaxAlpha, 0.1f, true);
				RevealScreenshotParent(); 
			}
			// Make the timeline zoom and move back to the corner
			else
			{
				GameManager.setPause(false);
				//tetherUIParent.transform.localPosition = Vector3.Lerp(tetherUIParent.transform.localPosition, tetherUIPos0, tetherUIZoomDownSpeed); 
				tetherUIParent.anchoredPosition = Vector2.Lerp(tetherUIParent.anchoredPosition, tetherUIPos0, tetherUIZoomDownSpeed); 
				tetherUIParent.transform.localScale = Vector3.Lerp(tetherUIParent.transform.localScale, tetherUIScale0, tetherUIZoomDownSpeed); 
				fadeImage.CrossFadeAlpha(0, 0.1f, true);
				HideScreenshotParent(); 
			}
		}
		// If the player has just died, force the timeline to come up and don't let players exit out of it
		else
		{
			GameManager.setPause(true);
			//tetherUIParent.transform.localPosition = Vector3.Lerp(tetherUIParent.transform.localPosition, tetherUIPos1, tetherUIZoomUpSpeed); 
			tetherUIParent.anchoredPosition = Vector2.Lerp(tetherUIParent.anchoredPosition, tetherUIPos1, tetherUIZoomUpSpeed); 
			tetherUIParent.transform.localScale = Vector3.Lerp(tetherUIParent.transform.localScale, tetherUIScale1, tetherUIZoomUpSpeed); 
			fadeImage.CrossFadeAlpha(fadeImageMaxAlpha, 0.15f, true);
			RevealScreenshotParent(); 
		}

		// When the UI starts to minimize back to the corner, don't allow it to maximize again until the bringUpTetherUIKey has been released
		if (!arrowMovingBack && arrowReachedPointTarget && tetherZoomKeyLock && (!Input.GetKey(PlayerControlManager.RH_TetherMenu) && !Input.GetKey(PlayerControlManager.LH_TetherMenu)))
		{
			tetherZoomKeyLock = false; 
		}

		// Set the state of tetherUINotZoomed, based on the position distance
		// This is somewhat imprecise; set the 'lower than' value to determine how close it must lerp before, for example, the player can place more tether points
		if (Vector3.Distance(tetherUIParent.transform.localPosition, tetherUIPos0) < 2f)
		{
			tetherUINotZoomed = true; 
		}
		else
		{
			tetherUINotZoomed = false; 
		}
	}

	/// <summary>
	/// Called via update to set the position of the curTimeArrow and the color of the tether point nodes
	/// </summary>
	void UpdateTetherTimelineUI()
	{
		// Update the color of each tether point node
		for (int i = 1; i < LevelStateManager.maxNumStates; i++)
		{

			if (i <= LevelStateManager.curState)
			{
				ui_tetherPoints[i].color = ui_pointActiveColor; 
			}
			else
			{
				ui_tetherPoints[i].color = ui_pointInactiveColor;
			}

		}

		RectTransform rp = curTimeArrow.GetComponent<RectTransform>(); 

		// Chooses the target position of the arrow, which must either be on top of the node or in between nodes
		// arrowReachedPointTarget is used to control where the arrow is lerping

		// Arrow position on top of node, using hard-coded values
		if (!arrowReachedPointTarget)
		{
			// Initial x position is -11, with a spacing of 1.5 between each node
			//xTarget = -11 + (1.5f * LevelStateManager.curState);
			xTarget = -95 + (45 * LevelStateManager.curState); 
		}
		// Arrow position is in between nodes
		else
		{
			// Initial x position is -10.25, with a spacing of 1.5 between each node
			//xTarget = -10.25f + (1.5f * LevelStateManager.curState); 
			xTarget = -65 + (45 * LevelStateManager.curState); 
		}

		// Set the RectTransform's position, lerping to the target
		float xLerp = Mathf.Lerp(rp.anchoredPosition.x, xTarget, 0.2f); 
		rp.anchoredPosition = new Vector2 (xLerp, rp.anchoredPosition.y); 

		// Handle state changes when the arrow approx reaches its target position
		if (!arrowReachedPointTarget && Mathf.Abs(rp.anchoredPosition.x - xTarget) < 0.01f)
		{
			// Used in conjunction with the zoomed-in tether UI to control its state
			if (arrowMovingBack)
			{
				arrowMovingBack = false;
				tetherZoomKeyLock = true; 
			}
			else
			{
				if (tetherUINotZoomed)
				{
					// Setting arrowReachedTargetPoint causes the xTarget above to change so the arrow moves to its new target in-between nodes
					arrowReachedPointTarget = true; 
				}
			}
		}
	}

	/// <summary>
	/// Called via UI buttons in the timeline to load a state
	/// </summary>
	/// <param name="state">State, as determined in the button</param>
	public void LoadPoint(int state)
	{
		arrowReachedPointTarget = false; 
		arrowMovingBack = true; 
		RemoveTimeTetherIndicator(state + 1); 
		LevelStateManager.loadTetherPoint(state);
	}

	public void CreatePoint()
	{
		if (LevelStateManager.canCreateTetherPoint())
		{
			Debug.Log("Create tether point"); 
			arrowReachedPointTarget = false; 
			LevelStateManager.createTetherPoint(); 
			CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, LevelStateManager.curState);
		}
		else
		{
			Debug.Log("Can't create tether point right now"); 
		}
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
		inst.screenshotImage.texture = ScreenshotManager.getScreenshot(state); 
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
		if (screenshot != null)
			screenshot.SetActive(true); 
	}

	void HideScreenshot()
	{
		if (screenshot != null)
			screenshot.SetActive(false); 
	}

	void RevealScreenshotParent()
	{
		if (screenshotParent != null)
			screenshotParent.SetActive(true); 
	}

	void HideScreenshotParent()
	{
		if (screenshotParent != null)
			screenshotParent.SetActive(false); 
	}

	// UI Drawer 

	void RevealDrawer()
	{
		Debug.Log("Reveal drawer: " + GameManager.HighestCodeFound()); 
		//keycodeDrawer.rectTransform.anchoredPosition = Vector3.Lerp(keycodeDrawer.rectTransform.anchoredPosition, GetDrawerTargetPos(), 5 * Time.deltaTime); 
	}

	void HideDrawer()
	{ 
		//keycodeDrawer.rectTransform.anchoredPosition = Vector3.Lerp(keycodeDrawer.rectTransform.anchoredPosition, keycodeTrayStartPos, 5 * Time.deltaTime); 
	}

	Vector2 GetDrawerTargetPos()
	{
		Debug.Log("target pos: " + new Vector3(keycodeDrawer.rectTransform.anchoredPosition.x + (GameManager.HighestCodeFound() * trayXPosStep), keycodeDrawer.rectTransform.anchoredPosition.y)); 

		return new Vector2(keycodeTrayStartPos.x + (GameManager.HighestCodeFound() * trayXPosStep), keycodeDrawer.rectTransform.anchoredPosition.y); 

	}

	// Stasis display
	void UpdateStasisDisplay()
	{
		if (stasisPieceParent == null)
		{
			return; 
		}

		if (GameManager.inst.canUseStasis)
		{
			stasisPieceParent.SetActive(true); 
		}
		else
		{
			stasisPieceParent.SetActive(false); 
		}
	}
	*/ 
}
