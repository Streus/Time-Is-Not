using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Player/Dashing")]
public class PlayerReachJumpTarget : Fork
{
    public override bool check(Controller c)
    {
        Player p = State.cast<Player>(c);

        if (Vector2.Distance(p.transform.position, p.getJumpTargetPos) < 0.25f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
