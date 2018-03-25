using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleDeath : MonoBehaviour 
{
	private ParticleSystem effect;

	void Start()
	{
		effect = gameObject.GetComponent<ParticleSystem> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (effect == null || effect.isStopped)
			Destroy (gameObject);
	}



}
