using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterPad : MonoBehaviour 
{
	//[SerializeField] ParticleSystem idleParticles; 
	[SerializeField] ParticleSystem teleportParticles; 

	public void OnStartTeleport()
	{
		Debug.Log("Start teleport"); 
		teleportParticles.Play(); 
	}
}
