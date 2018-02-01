using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// Manages interaction with time tether systems in LevelStateManager and the time tether UI systems
/// </summary>
public class TetherManager : MonoBehaviour 
{
	[Tooltip("The key used to create a tether point.")]
	public KeyCode createPointKey;
	[Tooltip("The key that needs to be held to bring up the load tether point selection UI")]
	public KeyCode bringUpTetherUIKey; 

	// Time tether UI
	[Tooltip("(Drag In) The GameObject with the tether UI that zooms between the corner and the screen center")]
	public GameObject tetherUIParent; 

	[Tooltip("The position of the tetherUIParent when at the bottom corner of the screen.")]
	public Vector3 tetherUIPos0;

	[Tooltip("The position of the tetherUIParent when zoomed in at the screen center")]
	public Vector3 tetherUIPos1; 

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


	// Use this for initialization
	void Start () 
	{
		fadeImage.gameObject.SetActive(true); 
		timeTetherIndicators = new GameObject[LevelStateManager.maxNumStates]; 
		CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, 0); 
	}

    // Update is called once per frame
    void Update() 
	{
		UpdateUIZoomState(); 

		// This is where the game detects when the player wants to create a tether point
		// Restrictions: player can't be dead, arrow can't be moving between points, and timeline must be in corner
		// TODO: can't restrict while game is paused, but need to find another way to prevent creation when a pause menu is up
		if (!GameManager.isPlayerDead() && Input.GetKeyDown(createPointKey) && arrowReachedPointTarget && tetherUINotZoomed)
		{
			CreatePoint(); 
		}

		UpdateTetherTimelineUI();
    
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
			if ((Input.GetKey(bringUpTetherUIKey) && !tetherZoomKeyLock) || arrowMovingBack)
			{
				GameManager.setPause(true);
				tetherUIParent.transform.localPosition = Vector3.Lerp(tetherUIParent.transform.localPosition, tetherUIPos1, tetherUIZoomUpSpeed); 
				tetherUIParent.transform.localScale = Vector3.Lerp(tetherUIParent.transform.localScale, tetherUIScale1, tetherUIZoomUpSpeed); 
				fadeImage.CrossFadeAlpha(fadeImageMaxAlpha, 0.1f, true);
			}
			// Make the timeline zoom and move back to the corner
			else
			{
				GameManager.setPause(false);
				tetherUIParent.transform.localPosition = Vector3.Lerp(tetherUIParent.transform.localPosition, tetherUIPos0, tetherUIZoomDownSpeed); 
				tetherUIParent.transform.localScale = Vector3.Lerp(tetherUIParent.transform.localScale, tetherUIScale0, tetherUIZoomDownSpeed); 
				fadeImage.CrossFadeAlpha(0, 0.1f, true);
			}
		}
		// If the player has just died, force the timeline to come up and don't let players exit out of it
		else
		{
			GameManager.setPause(true);
			tetherUIParent.transform.localPosition = Vector3.Lerp(tetherUIParent.transform.localPosition, tetherUIPos1, tetherUIZoomUpSpeed); 
			tetherUIParent.transform.localScale = Vector3.Lerp(tetherUIParent.transform.localScale, tetherUIScale1, tetherUIZoomUpSpeed); 
			fadeImage.CrossFadeAlpha(fadeImageMaxAlpha, 0.15f, true);
		}

		// When the UI starts to minimize back to the corner, don't allow it to maximize again until the bringUpTetherUIKey has been released
		if (!arrowMovingBack && arrowReachedPointTarget && tetherZoomKeyLock && !Input.GetKey(bringUpTetherUIKey))
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
			xTarget = -11 + (1.5f * LevelStateManager.curState);
		}
		// Arrow position is in between nodes
		else
		{
			// Initial x position is -10.25, with a spacing of 1.5 between each node
			xTarget = -10.25f + (1.5f * LevelStateManager.curState); 
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
}
