using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TimeTetherTest : MonoBehaviour 
{
	public KeyCode createPointKey;
	public Text pointText; 

	public GameObject timeTetherIndicatorPrefab; 
	public List<GameObject> timeTetherIndicators; 

	public KeyCode createStasisKey; 
	public KeyCode removeStasisKey; 
	public Text stasisText; 

	public GameObject stasisBubblePrefab; 



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Time tether
		if (Input.GetKeyDown(createPointKey))
		{
			if (LevelStateManager.canCreateTetherPoint())
			{
				Debug.Log("Create tether point"); 
				LevelStateManager.createTetherPoint(); 
				CreateTimeTetherIndicator(new Vector3 (LevelStateManager.curState, 0, 0));
			}
			else
			{
				Debug.Log("Can't create tether point right now"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			if (LevelStateManager.canLoadTetherPoint(0) && LevelStateManager.loadTetherPoint(0))
			{
				Debug.Log("Successfully loaded state 0"); 
				RemoveTimeTetherIndicator(0); 
			}
			else
			{
				Debug.Log("Could not load state 0"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (LevelStateManager.canLoadTetherPoint(1) && LevelStateManager.loadTetherPoint(1))
			{
				Debug.Log("Successfully loaded state 1"); 
				RemoveTimeTetherIndicator(1); 
			}
			else
			{
				Debug.Log("Could not load state 1"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (LevelStateManager.canLoadTetherPoint(2) && LevelStateManager.loadTetherPoint(2))
			{
				Debug.Log("Successfully loaded state 2"); 
				RemoveTimeTetherIndicator(2); 
			}
			else
			{
				Debug.Log("Could not load state 2"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			if (LevelStateManager.canLoadTetherPoint(3) && LevelStateManager.loadTetherPoint(3))
			{
				Debug.Log("Successfully loaded state 3"); 
				RemoveTimeTetherIndicator(3); 
			}
			else
			{
				Debug.Log("Could not load state 3"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			if (LevelStateManager.canLoadTetherPoint(4) && LevelStateManager.loadTetherPoint(4))
			{
				Debug.Log("Successfully loaded state 4"); 
				RemoveTimeTetherIndicator(4); 
			}
			else
			{
				Debug.Log("Could not load state 4"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			if (LevelStateManager.canLoadTetherPoint(5) && LevelStateManager.loadTetherPoint(5))
			{
				Debug.Log("Successfully loaded state 5"); 
				RemoveTimeTetherIndicator(5); 
			}
			else
			{
				Debug.Log("Could not load state 5"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			if (LevelStateManager.canLoadTetherPoint(6) && LevelStateManager.loadTetherPoint(6))
			{
				Debug.Log("Successfully loaded state 6");
				RemoveTimeTetherIndicator(6); 
			}
			else
			{
				Debug.Log("Could not load state 6"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			if (LevelStateManager.canLoadTetherPoint(7) && LevelStateManager.loadTetherPoint(7))
			{
				Debug.Log("Successfully loaded state 7"); 
				RemoveTimeTetherIndicator(7); 
			}
			else
			{
				Debug.Log("Could not load state 7"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			if (LevelStateManager.canLoadTetherPoint(8) && LevelStateManager.loadTetherPoint(8))
			{
				Debug.Log("Successfully loaded state 8"); 
				RemoveTimeTetherIndicator(8); 
			}
			else
			{
				Debug.Log("Could not load state 8"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			if (LevelStateManager.canLoadTetherPoint(9) && LevelStateManager.loadTetherPoint(9))
			{
				Debug.Log("Successfully loaded state 9"); 
				RemoveTimeTetherIndicator(9); 
			}
			else
			{
				Debug.Log("Could not load state 9"); 
			}
		}

		// Stasis Keys
		if (Input.GetKeyDown(createStasisKey) && LevelStateManager.canAddStasisBubble())
		{
			Vector3 spawnPos = new Vector3 (transform.position.x + Random.Range(-2.0f, 2.0f), transform.position.y + Random.Range(-2.0f, 2.0f), transform.position.z); 
			StasisBubble newStasis = ((GameObject)Instantiate(stasisBubblePrefab, spawnPos, transform.rotation)).GetComponent<StasisBubble>(); 
			LevelStateManager.addStasisBubble(newStasis); 
		}

		if (Input.GetKeyDown(removeStasisKey) && LevelStateManager.canRemoveStasisBubble())
		{
			LevelStateManager.removeLastStasisBubble(); 
		}

		// Sample tether UI
		if (pointText != null)
		{
			pointText.text = LevelStateManager.curState + " / " + (LevelStateManager.maxNumStates - 1); 
		}

		// Sample stasis UI
		if (stasisText != null)
		{
			stasisText.text = LevelStateManager.numStasisLeft + " / " + LevelStateManager.maxNumStasis; 
		}
	}

	void CreateTimeTetherIndicator(Vector3 pos)
	{
		GameObject indicator = Instantiate(timeTetherIndicatorPrefab, pos, Quaternion.identity, this.transform); 
		timeTetherIndicators.Add(indicator); 
	}

	void RemoveTimeTetherIndicator(int index)
	{
		if (index < timeTetherIndicators.Count)
		{
			Destroy(timeTetherIndicators[index].gameObject); 
			timeTetherIndicators.RemoveAt(index); 
		}
	}
}
