using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HummingBirdFlappingSounds : MonoBehaviour {
    AudioSource source;
    // Use this for initialization
    void Start () {
        source = this.GetComponentInParent<AudioSource>();
    }


    public void PlayHummingBirdFlapUpSoud()
    {
        if (source != null)
        {
            source.clip = AudioLibrary.inst.hummingBirdFlapUp;
            source.Play();
        }
    }

    public void PlayHummingBirdFlapDownSoud()
    {
        if (source != null)
        {
            source.clip = AudioLibrary.inst.hummingBirdFlapDown;
            source.Play();
        }
    }
}
