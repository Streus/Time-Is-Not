using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleDeath : MonoBehaviour 
{
	private ParticleSystem effect;
    AudioSource source;

	void Start()
	{
		effect = gameObject.GetComponent<ParticleSystem> ();
        source = this.GetComponent<AudioSource>();
        if (source != null)
        {
            source.clip = AudioLibrary.inst.stasisRemoval;
            source.Play();
        }
    }

	// Update is called once per frame
	void Update () 
	{
		if (effect == null || effect.isStopped)
			Destroy (gameObject);
	}



}
