﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Player/Targeting")]
public class PlayerInvokeTargeting : Fork
{
    public override bool check(Controller c)
    {
        Player p = State.cast<Player>(c);
        if ((Input.GetKeyDown(PlayerControlManager.RH_Dash) ||
			Input.GetKeyDown(PlayerControlManager.LH_Dash)) && 
			p.getSelf().getAbility(1).isReady() &&
			GameManager.CursorInGameplayState())
        {
            return true;
        }
		return false;
    }
}
