using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Player/Dashing")]
public class PlayerDashing : Action
{
	public float dashSpeed = 1f;

    public override void perform(Controller c)
    {
        Player p = State.cast<Player>(c);
		c.gameObject.layer = LayerMask.NameToLayer("MPPassenger");
		c.transform.position = Vector2.Lerp(c.transform.position, p.getJumpTargetPos, dashSpeed * Time.deltaTime);
    }
}
