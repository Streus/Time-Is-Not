using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TetherManager : MonoBehaviour 
{
    public GameObject menu;

	public KeyCode createPointKey;
	public KeyCode bringUpTetherUIKey; 

	// Time tether UI
	public GameObject tetherUIParent; 
	public Vector3 tetherUIPos0;
	public Vector3 tetherUIPos1; 
	public Vector3 tetherUIScale0;
	public Vector3 tetherUIScale1; 

	public float tetherUIZoomUpSpeed; 
	public float tetherUIZoomDownSpeed; 

	public Image fadeImage; 
	public float fadeImageMaxAlpha; 

	bool arrowMovingBack;  
	bool tetherZoomKeyLock; 
	bool tetherUINotZoomed; 

	public Image[] ui_tetherPoints; 
	public Color ui_pointActiveColor; 
	public Color ui_pointInactiveColor; 
	public GameObject curTimeArrow; 
	[SerializeField] bool arrowReachedPointTarget; 
	float xTarget; 

	public GameObject timeTetherIndicatorPrefab; 
	[HideInInspector] GameObject[] timeTetherIndicators; 


	// Use this for initialization
	void Start () 
	{
        menu.SetActive(false);
		fadeImage.gameObject.SetActive(true); 
		timeTetherIndicators = new GameObject[LevelStateManager.maxNumStates]; 
		CreateTimeTetherIndicator(GameManager.GetPlayer().transform.position, 0); 
	}

    // Update is called once per frame
    void Update() 
	{
		/* The panel has been removed for now
        if (Input.GetKey(KeyCode.Space))
        {
            menu.SetActive(true);
        }
        else
        {
            menu.SetActive(false);
        }
        */ 

		UpdateUIZoomState(); 

		if (!GameManager.isPlayerDead() && Input.GetKeyDown(createPointKey) && arrowReachedPointTarget && tetherUINotZoomed)
		{
			CreatePoint(); 
		}

		UpdateTetherTimelineUI();
    
	}

	void UpdateUIZoomState()
	{
		if (!GameManager.isPlayerDead())
		{
			if ((Input.GetKey(bringUpTetherUIKey) && !tetherZoomKeyLock) || arrowMovingBack)
			{
				GameManager.setPause(true);
				tetherUIParent.transform.localPosition = Vector3.Lerp(tetherUIParent.transform.localPosition, tetherUIPos1, tetherUIZoomUpSpeed); 
				tetherUIParent.transform.localScale = Vector3.Lerp(tetherUIParent.transform.localScale, tetherUIScale1, tetherUIZoomUpSpeed); 
				fadeImage.CrossFadeAlpha(fadeImageMaxAlpha, 0.1f, true);
			}
			else
			{
				GameManager.setPause(false);
				tetherUIParent.transform.localPosition = Vector3.Lerp(tetherUIParent.transform.localPosition, tetherUIPos0, tetherUIZoomDownSpeed); 
				tetherUIParent.transform.localScale = Vector3.Lerp(tetherUIParent.transform.localScale, tetherUIScale0, tetherUIZoomDownSpeed); 
				fadeImage.CrossFadeAlpha(0, 0.1f, true);
			}
		}
		else
		{
			GameManager.setPause(true);
			tetherUIParent.transform.localPosition = Vector3.Lerp(tetherUIParent.transform.localPosition, tetherUIPos1, tetherUIZoomUpSpeed); 
			tetherUIParent.transform.localScale = Vector3.Lerp(tetherUIParent.transform.localScale, tetherUIScale1, tetherUIZoomUpSpeed); 
			fadeImage.CrossFadeAlpha(fadeImageMaxAlpha, 0.15f, true);
		}

		if (!arrowMovingBack && arrowReachedPointTarget && tetherZoomKeyLock && !Input.GetKey(bringUpTetherUIKey))
		{
			tetherZoomKeyLock = false; 
		}

		if (Vector3.Distance(tetherUIParent.transform.localPosition, tetherUIPos0) < 2f)
		{
			tetherUINotZoomed = true; 
		}
		else
		{
			tetherUINotZoomed = false; 
		}
	}

	void UpdateTetherTimelineUI()
	{
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

		if (!arrowReachedPointTarget)
		{
			xTarget = -11 + (1.5f * LevelStateManager.curState);
		}
		else
		{
			xTarget = -10.25f + (1.5f * LevelStateManager.curState); 
		}

		float xLerp = Mathf.Lerp(rp.anchoredPosition.x, xTarget, 0.2f); 
		rp.anchoredPosition = new Vector2 (xLerp, rp.anchoredPosition.y); 

		if (!arrowReachedPointTarget && Mathf.Abs(rp.anchoredPosition.x - xTarget) < 0.01f)
		{
			arrowReachedPointTarget = true; 

			if (arrowMovingBack)
			{
				arrowMovingBack = false;

				tetherZoomKeyLock = true; 
			}
		}
	}

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
