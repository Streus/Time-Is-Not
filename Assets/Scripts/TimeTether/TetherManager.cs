using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TetherManager : MonoBehaviour 
{
    public GameObject menu;

	public KeyCode createPointKey;

	// Time tether UI
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

		if (Input.GetKeyDown(createPointKey))
		{
			CreatePoint(); 
		}

		UpdateUI();
    
	}

	void UpdateUI()
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
		}
	}

	public void LoadPoint(int state)
	{
		arrowReachedPointTarget = false; 
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
		Debug.Log("Remove indicators starting at index " + index); 

		for (int i = index; i < timeTetherIndicators.Length; i++)
		{
			if (timeTetherIndicators[i] != null)
			{
				Destroy(timeTetherIndicators[i].gameObject);
			}
		}
	}
}
