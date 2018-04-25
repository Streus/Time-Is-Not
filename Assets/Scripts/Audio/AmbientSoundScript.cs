using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundScript : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip ambientSound;
    private float time;
    private float timer = 0;
    private bool played;
    public bool background;

	// Use this for initialization
	void Start ()
    {
        source = this.gameObject.GetComponent<AudioSource>();
        if (background)
        {
            time = 0;
        }
        else
        {
            time = Random.Range(0.0f, 2.0f);
        }
        played = false;
    }
    private void Update()
    {
        if (!played)
        {
            timer += Time.deltaTime;
            if (ambientSound != null && timer >= time)
            {
                source.clip = ambientSound;
                source.Play();
                played = true;
                Debug.Log("Played " + source.clip.name);
            }
        }

        if(source.isPlaying)
        {
            Debug.Log("Playing " + source.clip.name);
        }
    }
}
