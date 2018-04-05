using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePauser : MonoBehaviour 
{
	ParticleSystem particles;
	//ParticleSystem.EmissionModule particlesEmission; 

	bool pauseSaved; 

	public bool disablePausing; 

	void OnEnable()
	{
		if (GameManager.inst != null)
			GameManager.inst.pauseTypeToggled += OnPauseTypeToggled; 
	}

	void OnDisable()
	{
		if (GameManager.inst != null)
			GameManager.inst.pauseTypeToggled -= OnPauseTypeToggled;
	}

	// Use this for initialization
	void Start () 
	{
		if (GetComponent<ParticleSystem>() != null)
		{
			particles = GetComponent<ParticleSystem>(); 
			//particlesEmission = particles.emission; 
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnPauseTypeToggled(PauseType type)
	{
		if (particles == null)
		{
			return; 
		}

		Debug.Log("On Pause Type Toggled: " + type); 

		// Pause
		if (!disablePausing && !particles.isPaused)
		{
			Debug.Log("Test"); 

			if (GameManager.inst.IsWorldPauseType(type))
			{
				Debug.Log("Test2"); 

				pauseSaved = true; 
				particles.Pause(); 
				//particlesEmission.enabled = false; 
			}
		}

		// Unpause
		if (pauseSaved)
		{
			if (type == PauseType.NONE || type == PauseType.ZOOM)
			{
				Debug.Log("Test 3"); 

				pauseSaved = false; 
				particles.Play(); 
				//particlesEmission.enabled = true; 
			}
		}
	}
}
