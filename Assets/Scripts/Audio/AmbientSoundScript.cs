using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundScript : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip ambientSound;

	// Use this for initialization
	void Start ()
    {
        source = this.gameObject.GetComponent<AudioSource>();
        if (ambientSound != null)
        {
            source.clip = ambientSound;
            source.Play();
        }
    }
	
}
