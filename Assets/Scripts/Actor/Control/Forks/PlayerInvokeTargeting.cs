using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Player/Targeting")]
public class PlayerInvokeTargeting : Fork
{
    public override bool check(Controller c)
    {
        if ((Input.GetKeyDown(PlayerControlManager.RH_Dash)
            || Input.GetKeyDown(PlayerControlManager.LH_Dash)) &&
            GameManager.inst.canUseDash)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
