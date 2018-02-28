using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("This functionality has been moved into PlayerDefaultControl")]
[CreateAssetMenu(menuName = "AI/Forks/Player/Targeting")]
public class PlayerInvokeTargeting : Fork
{
    public override bool check(Controller c)
    {
        Player p = State.cast<Player>(c);
        if ((Input.GetKeyDown(PlayerControlManager.RH_Dash) ||
            Input.GetKeyDown(PlayerControlManager.LH_Dash)) &&
			CursorManager.CursorInGameplayState())
        {
            if(p.getSelf().getAbility(1).isReady())
            {
                AudioLibrary.PlayDashForwardSound();
                return true;
            }
            else
            {
                if (!GlobalAudio.ClipIsPlaying(AudioLibrary.inst.dashError))
                {
                    AudioLibrary.PlayDashErrorSound();
                }
                return false;
            }
        }
        return false;
    }
}
            