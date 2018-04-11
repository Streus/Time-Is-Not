using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertFlash : Singleton<AlertFlash> 
{
	ParticleSystem particles; 
	ParticleSystem.MainModule particlesMain; 
	float defaultSimulationSpeed; 

	public bool disable; 

	void Awake()
	{
		particles = GetComponent<ParticleSystem>(); 
		particlesMain = particles.main; 

		defaultSimulationSpeed = particles.main.simulationSpeed; 
	}

	public void PlayAlertFlash()
	{
		if (particles.isPlaying)
			return; 

		if (disable)
			return; 

		particlesMain.simulationSpeed = defaultSimulationSpeed; 
		particles.Play(); 
	}

	public void PlayAlertFlash(float simulationSpeed)
	{
		if (particles.isPlaying)
			return;

		if (disable)
			return; 

		particlesMain.simulationSpeed = simulationSpeed; 
		particles.Play(); 
	}
}
