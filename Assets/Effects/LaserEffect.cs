using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LaserEffect : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(gameObject.GetComponent<ParticleSystem>() == null)
		{
			gameObject.AddComponent<ParticleSystem> ();
		}
	}


}
