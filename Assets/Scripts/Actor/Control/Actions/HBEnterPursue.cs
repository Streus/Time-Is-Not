using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Hummingbird/HBEnterPursue")]
public class HBEnterPursue : Action
{
    AudioSource source;

    public override void perform(Controller c)
    {
        source = c.GetComponent<AudioSource>();
        source.clip = AudioLibrary.inst.hummingBirdSpotting;
        source.loop = false;
        source.Play();
    }
}
