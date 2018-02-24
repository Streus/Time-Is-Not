using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Player/StopDashing")]
public class PlayerReachJumpTarget : Fork
{
	public float threshold = 0.5f;

    public override bool check(Controller c)
    {
        Player p = State.cast<Player>(c);
		Collider2D col = p.GetComponent<Collider2D> ();

		p.getSelf ().getAbility (1).active = false;

		if (Vector2.Distance (p.transform.position + (Vector3)col.offset, p.getJumpTargetPos) < threshold)
		{
			p.gameObject.layer = LayerMask.NameToLayer("GroundEnts");
			p.getSelf ().getAbility (1).active = true;
			return true;
		}
        
		return false;
    }
}
