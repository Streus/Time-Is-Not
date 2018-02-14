using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Player/StopDashing")]
public class PlayerReachJumpTarget : Fork
{
    public override bool check(Controller c)
    {
        Player p = State.cast<Player>(c);

		if (Vector2.Distance (p.transform.position, p.getJumpTargetPos) < 0.1f)
		{
			p.gameObject.layer = 8;
			return true;
		}
		return false;
    }
}
