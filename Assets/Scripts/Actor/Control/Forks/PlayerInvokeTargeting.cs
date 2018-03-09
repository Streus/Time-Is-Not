using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Player/Targeting")]
public class PlayerInvokeTargeting : Fork
{
    public override bool check(Controller c)
    {
        Player p = State.cast<Player>(c);
		if (PlayerControlManager.GetKeyDown(ControlInput.DASH) &&
			CursorManager.CursorInGameplayState())
        {
            return true;
        }
        return false;
    }
}
            