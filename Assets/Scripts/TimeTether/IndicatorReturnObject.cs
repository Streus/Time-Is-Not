using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorReturnObject : MonoBehaviour 
{
	GameObject target; 

	public float startHomingSpeed = 1; 
	public float maxHomingSpeed = 1; 
	public float increaseHomingSpeedRate = 0.1f; 

	float curHomingSpeed; 

	[Tooltip("The maximum distance this object can be away from the target (player) when a tether state is loaded")]
	[SerializeField] float maxLoadDist = 10; 

	//int pauseStates; 

	void OnEnable()
	{
		LevelStateManager.inst.stateLoaded += OnLoad;
	}

	void OnDisable()
	{
		if (LevelStateManager.inst != null)
			LevelStateManager.inst.stateLoaded -= OnLoad;
	}

	// Use this for initialization
	void Start () 
	{
		target = GameManager.GetPlayer(); 

		if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
		{
			// Don't play sounds or do collected effects here
			Destroy(this.gameObject); 
		}

		curHomingSpeed = startHomingSpeed; 

		//pauseStates = (int)PauseType.CUTSCENE | (int)PauseType.NONE | (int)PauseType.ZOOM; 
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		if (GameManager.inst.CheckPause(pauseStates))
		{
			Debug.Log("return object is active"); 
		}
		else
		{
			Debug.Log("return object NOT active");
		}
		*/

		if (GameManager.inst.pauseType == PauseType.CUTSCENE || GameManager.inst.pauseType == PauseType.NONE || GameManager.inst.pauseType == PauseType.ZOOM || GameManager.inst.pauseType == PauseType.TETHER_TRANSITION)
		{
			HomeToTarget(); 
		}
	}

	void HomeToTarget()
	{
		// Move
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, curHomingSpeed * Time.deltaTime); 

		// Update speed
		//curHomingSpeed = Mathf.Lerp(curHomingSpeed, maxHomingSpeed, increaseHomingSpeedRate * Time.deltaTime); 
		if (curHomingSpeed < maxHomingSpeed)
		{
			curHomingSpeed += increaseHomingSpeedRate * Time.deltaTime; 
			if (curHomingSpeed > maxHomingSpeed)
				curHomingSpeed = maxHomingSpeed; 
		}

		//transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.1f); 

		if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
		{
			// TODO Spawn effect for being recollected
			// TODO Play sound
			Destroy(this.gameObject); 
		}

		if (GameManager.inst.pauseType == PauseType.TETHER_TRANSITION && Vector3.Distance(transform.position, target.transform.position) > maxLoadDist)
		{
			Debug.Log("Move indicator closer to player"); 

			Vector3 dir = (transform.position - target.transform.position).normalized; 

			transform.position = target.transform.position + (maxLoadDist * dir); 
		}
			
	}

	void OnLoad(bool success)
	{
		/*
		if (!success)
			return; 

		if (Vector3.Distance(transform.position, target.transform.position) > maxLoadDist)
		{
			Debug.Log("Move indicator closer to player"); 

			Vector3 dir = (target.transform.position - transform.position).normalized; 

			transform.position = target.transform.position + (maxLoadDist * dir); 
		}
		*/ 
		// Destroy this object if another tether back is called while this one is flying
		Destroy(gameObject); 
	}
}
