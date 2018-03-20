using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/HermitCrab/ResetReturnTimer")]
public class HCResetReturnTimer : Action
{
    AudioSource source;
	public override void perform (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);
        source = c.GetComponent<AudioSource>();
        
        if (hc.getWasPushed())
        {
            
            hc.setReturnTimerOnStand();
            source.clip = AudioLibrary.inst.hermitCrabMoving;
            source.loop = true;
            source.Play();
        }
        else
            hc.resetReturnTimer();
	}
}
