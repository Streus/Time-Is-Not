using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("This code has been moved directly into the Player")]
[CreateAssetMenu(menuName = "AI/Actions/Player/Targeting")]
public class PlayerTargeting : Action
{
    public override void perform(Controller c)
    {
		/*
        //HOW TO GET COLLIDER OFFSET
//        Collider2D hitbox = c.gameObject.GetComponent<Collider2D>();
//        Vector2 trueCenter = (Vector2)c.transform.position + hitbox.offset;



        Player p = State.cast<Player>(c);
		BoxCollider2D collider = p.GetComponent<BoxCollider2D> ();
        Vector2 colliderSize = new Vector2(collider.size.x * p.transform.localScale.x, collider.size.y * p.transform.localScale.y) * .5f;
		Vector2 colliderOffset = collider.offset;
		Vector3 pospoff = p.transform.position + (Vector3)colliderOffset;

        float jumpDistance = p.getMaxJumpDist;
		Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pospoff;

        //do a boxcast from the player to the furthest possible jump position
		RaycastHit2D[] pathCheck = Physics2D.BoxCastAll(pospoff, colliderSize, 0.0f, dir, jumpDistance, p.moveMask.value);

        if (pathCheck != null)
        {
            //find the nearest collision returned by the circlecast, ifex
            RaycastHit2D nearest = default(RaycastHit2D);
            foreach (RaycastHit2D hit in pathCheck)
            {
				if (hit.collider.isTrigger || hit.collider == c.GetComponent<Collider2D>())
					continue;

                if (nearest == default(RaycastHit2D))
                    nearest = hit;
				else if (hit.distance < nearest.distance)
                    nearest = hit;
            }

            //set the jump distance to the distance to the nearest collision, ifex
            if (nearest != default(RaycastHit2D) && nearest.distance < jumpDistance)
                jumpDistance = nearest.distance;
        }

		Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float dist = Vector2.Distance(mp, pospoff);
        if (dist > jumpDistance) //if the distance to the mouse is greater than our max jump distance, limit the target position to our max jump distance
			p.setJumpTargetPos((Vector2)p.transform.position + colliderOffset + (mp - (Vector2)p.transform.position + colliderOffset).normalized * jumpDistance);
        else if (dist < p.getMinJumpDist) // if there's no concept of a minimum distance, cut this line out as well as the line with the if statement
			p.setJumpTargetPos((Vector2)p.transform.position + colliderOffset + (mp - (Vector2)p.transform.position + colliderOffset).normalized * Mathf.Min(jumpDistance, p.getMinJumpDist));
        else //mouse is within the max distance, no need to limit
            p.setJumpTargetPos(mp);

		//TODO figure out how to get this working
//		if(Input.GetKeyUp(PlayerControlManager.LH_Dash) || Input.GetKeyUp(PlayerControlManager.RH_Dash))
			p.getSelf().getAbility(1).use(p.getSelf(), p.getJumpTargetPos);
		*/
    }
}
