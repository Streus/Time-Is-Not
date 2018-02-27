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
        Collider2D hitBox = p.GetComponent<Collider2D>();
		p.gameObject.layer = LayerMask.NameToLayer("DashingPlayer");
		p.transform.position = Vector2.Lerp(p.transform.position, (Vector2)p.getJumpTargetPos() - hitBox.offset, dashSpeed * Time.deltaTime);
    }
}
