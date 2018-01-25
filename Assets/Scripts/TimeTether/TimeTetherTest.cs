using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TimeTetherTest : MonoBehaviour 
{
	public KeyCode createPointKey;
	public Text pointText; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(createPointKey))
		{
			if (LevelStateManager.canCreateTetherPoint())
			{
				Debug.Log("Create tether point"); 
				LevelStateManager.createTetherPoint(); 
			}
			else
			{
				Debug.Log("Can't create tether point right now"); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			if (LevelStateManager.curState >= 0)
			{
				LevelStateManager.loadTetherPoint(0); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (LevelStateManager.curState >= 1)
			{
				LevelStateManager.loadTetherPoint(1); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (LevelStateManager.curState >= 2)
			{
				LevelStateManager.loadTetherPoint(2); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			if (LevelStateManager.curState >= 3)
			{
				LevelStateManager.loadTetherPoint(3); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			if (LevelStateManager.curState >= 4)
			{
				LevelStateManager.loadTetherPoint(4); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			if (LevelStateManager.curState >= 5)
			{
				LevelStateManager.loadTetherPoint(5); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			if (LevelStateManager.curState >= 6)
			{
				LevelStateManager.loadTetherPoint(6); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			if (LevelStateManager.curState >= 7)
			{
				LevelStateManager.loadTetherPoint(7); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			if (LevelStateManager.curState >= 8)
			{
				LevelStateManager.loadTetherPoint(8); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			if (LevelStateManager.curState >= 9)
			{
				LevelStateManager.loadTetherPoint(9); 
			}
		}

		if (pointText != null)
		{
			pointText.text = LevelStateManager.curState + " / " + LevelStateManager.maxNumStates; 
		}
	}
}
