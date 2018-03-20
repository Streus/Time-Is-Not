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

	public void SetParticlePosition(Vector2 location)
	{
		transform.position = (Vector3)location;
	}

	public void SetParticleRotation(Vector2 rot)
	{
		//transform.rotation.Set(rot.x, rot.y,);
	}
}
