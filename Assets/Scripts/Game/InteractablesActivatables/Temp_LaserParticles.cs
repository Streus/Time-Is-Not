using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_LaserParticles : MonoBehaviour 
{
	ParticleSystem laserParticles; 

	// Use this for initialization
	void Start () 
	{
		laserParticles = GetComponent<ParticleSystem>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameManager.isPaused() && !laserParticles.isPaused)
		{
			laserParticles.Pause();
		}
		else if (!GameManager.isPaused() && laserParticles.isPaused)
		{
			laserParticles.Play(); 
		}
	}
}
