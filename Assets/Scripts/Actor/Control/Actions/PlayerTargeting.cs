using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Player/Targeting")]
public class PlayerTargeting : Action
{
    public override void perform(Controller c)
    {
        Player p = State.cast<Player>(c);
        float jumpDistance = p.getMaxJumpDist;
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - p.transform.position;

		BoxCollider2D collider = p.GetComponent<BoxCollider2D> ();
        Vector2 colliderSize = collider.size;
		Vector2 colliderOffset = collider.offset;

        //do a boxcast from the player to the furthest possible jump position
		RaycastHit2D[] pathCheck = Physics2D.BoxCastAll(p.transform.position, colliderSize, 0.0f, dir, jumpDistance, p.moveMask.value);

        if (pathCheck != null)
        {
            //find the nearest collision returned by the circlecast, ifex
            RaycastHit2D nearest = default(RaycastHit2D);
            foreach (RaycastHit2D hit in pathCheck)
            {
				if (hit.collider.isTrigger)
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
		float dist = Vector2.Distance(mp, p.transform.position);
        if (dist > jumpDistance) //if the distance to the mouse is greater than our max jump distance, limit the target position to our max jump distance
            p.setJumpTargetPos((Vector2)p.transform.position + (mp - (Vector2)p.transform.position).normalized * jumpDistance);
        else if (dist < p.getMinJumpDist) // if there's no concept of a minimum distance, cut this line out as well as the line with the if statement
            p.setJumpTargetPos((Vector2)p.transform.position + (mp - (Vector2)p.transform.position).normalized * Mathf.Min(jumpDistance, p.getMinJumpDist));
        else //mouse is within the max distance, no need to limit
            p.setJumpTargetPos(mp);

		//TODO figure out how to get this working
//		if(Input.GetKeyUp(PlayerControlManager.LH_Dash) || Input.GetKeyUp(PlayerControlManager.RH_Dash))
			p.getSelf().getAbility(1).use(p.getSelf(), p.getJumpTargetPos);            
    }
}
