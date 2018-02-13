using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Player/Dashing")]
public class PlayerDashing : Action
{
    public override void perform(Controller c)
    {
        Player p = State.cast<Player>(c);
        p.gameObject.layer = 9;
        p.transform.position = Vector2.Lerp(p.transform.position, p.getJumpTargetPos, 1);
    }
}
