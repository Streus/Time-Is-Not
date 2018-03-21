using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/bird/HBSightCone")]
public class HBSightCone : Fork
{
	public override bool check (Controller c)
	{
		Hummingbird bird = State.cast<Hummingbird> (c);

		RaycastHit2D[] hits;
		hits = Physics2D.CircleCastAll (c.transform.position, bird.getSightRange (), Vector2.zero, 0f, bird.getTargetMask());
		for (int i = 0; i < hits.Length; i++)
		{
			Entity e = hits [i].collider.GetComponent<Entity> ();
			if (e != null && e.getFaction () == Entity.Faction.player)
			{
				Vector3 dir = (hits [i].collider.transform.position + (Vector3)hits [i].collider.offset) - c.transform.position;

				if (Vector3.Angle (dir, c.transform.up) < bird.getFOV () / 2f)
				{
					RaycastHit2D wallCheck;
					float dist = Vector2.Distance (c.transform.position, hits [i].collider.transform.position + (Vector3)hits [i].collider.offset);

					bool saveQHT = Physics2D.queriesHitTriggers;
					Physics2D.queriesHitTriggers = false;
					wallCheck = Physics2D.Raycast (c.transform.position, dir, dist, bird.getObstMask());
					Physics2D.queriesHitTriggers = saveQHT;

					if (wallCheck.collider == null || wallCheck.collider.isTrigger)
					{
						bird.setPursuitTarget (hits [i].collider.transform);
						bird.setInPursuit (true);
						return true;
					}
				}

				Vector3 centerDir = hits [i].collider.transform.position - c.transform.position;

				if (Vector3.Angle (centerDir, c.transform.up) < bird.getFOV () / 2f)
				{
					RaycastHit2D wallCheck;
					float dist = Vector2.Distance (c.transform.position, hits [i].collider.transform.position);

					bool saveQHT = Physics2D.queriesHitTriggers;
					Physics2D.queriesHitTriggers = false;
					wallCheck = Physics2D.Raycast (c.transform.position, centerDir, dist, bird.getObstMask());
					Physics2D.queriesHitTriggers = saveQHT;

					if (wallCheck.collider == null || wallCheck.collider.isTrigger)
					{
						bird.setPursuitTarget (hits [i].collider.transform);
						bird.setInPursuit (true);
						return true;
					}
				}
			}
		}

		bird.setInPursuit (false);
		return false;
	}
}
