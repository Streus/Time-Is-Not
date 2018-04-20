using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonClicks : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;

    private void Awake()
    {
        source = this.gameObject.GetComponent<AudioSource>();
        source.clip = clip;
    }

    public void PlayMenuButton()
    {  
        source.Play();
    }
}
