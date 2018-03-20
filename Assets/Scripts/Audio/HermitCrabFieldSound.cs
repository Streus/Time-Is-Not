using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitCrabFieldSound : MonoBehaviour
{
    SpriteRenderer fieldRenderer;
    AudioSource source;
    AudioClip hermitField;
    bool fieldOn = false;
	// Use this for initialization
	void Start ()
    {
        fieldRenderer = this.GetComponent<SpriteRenderer>();
        source = this.gameObject.GetComponent<AudioSource>();
        hermitField = AudioLibrary.inst.hermitCrabField;
        source.clip = hermitField;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(fieldRenderer.enabled && fieldOn)
        {
            source.Play();
            Debug.Log("ON");
            fieldOn = false;
        }
        else if (!fieldRenderer.enabled && !fieldOn)
        {
            source.Stop();
            Debug.Log("OFF");
            fieldOn = true;
        }
		
	}
}
