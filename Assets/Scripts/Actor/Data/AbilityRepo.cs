using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Ability
{
	static Ability()
	{
		repo = new Dictionary<string, Ability> ();

		put (new Ability (
			"Place Stasis",
			"Place a stasis bubble in the world.",
			null,
			1f,
			0,
			placeStasisBubble));
        put(new Ability (
            "Dash",
            "Dashes to mouse description",
            null,
            1,
            0,
            isDashing));
	}

	#region PLAYER_ABILITIES

	private static bool placeStasisBubble(Entity e, Vector2 tarPos)
	{
		if (!LevelStateManager.canAddStasisBubble ())
			return false;

		float angle = Vector2.Angle (Vector2.up, tarPos - (Vector2)e.transform.position);
		float sign = e.transform.position.x < tarPos.x ? -1f : 1f;
		Quaternion rot = Quaternion.Euler(0f, 0f, angle * sign);

		StasisBullet s = StasisBullet.create (e.transform.position, rot, tarPos);
		Physics2D.IgnoreCollision (s.GetComponent<Collider2D> (), e.GetComponent<Collider2D> ());

		return true;
	}
    private static bool isDashing(Entity e, Vector2 tarPos)
    {
        e.GetComponent<Player>().enterDashState();
        return true;
    }
	#endregion
}


