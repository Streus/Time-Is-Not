using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Player/StopDashing")]
public class PlayerReachJumpTarget : Fork
{
	public float threshold = 0.05f;

    public override bool check(Controller c)
    {
        Player p = State.cast<Player>(c);
		Collider2D col = p.GetComponent<Collider2D> ();

		p.getSelf ().getAbility (1).active = false;

        bool isColliding = false;

        Collider2D[] colsHit = Physics2D.OverlapBoxAll((Vector2)p.transform.position + col.offset, ((BoxCollider2D)col).size, 0f, p.moveMask);
        for (int i = 0; i < colsHit.Length; i++)
        {
			if (colsHit[i] != col && !colsHit[i].isTrigger)
                isColliding = true;
        }
       
		if (Vector2.Distance ((Vector2)p.transform.position + col.offset, p.getJumpTargetPos()) < threshold || isColliding)
		{
//			Debug.Log("Target pos: " + p.getJumpTargetPos().x + ", " + p.getJumpTargetPos().y);
//            Debug.Log("Player pos: " + p.transform.position.x + ", " + p.transform.position.y);
//            Debug.Log("True Player pos: " + (p.transform.position.x + col.offset.x) + ", " + (p.transform.position.y + col.offset.y));
//			Debug.Log("Distance: " + Vector2.Distance((Vector2)p.transform.position, p.getJumpTargetPos()));
//			Debug.Log("True Distance: " + Vector2.Distance((Vector2)p.transform.position + col.offset, p.getJumpTargetPos()));

            p.gameObject.layer = LayerMask.NameToLayer("GroundEnts");
			p.getSelf ().getAbility (1).active = true;
			return true;
		}
        
		return false;
    }
}
