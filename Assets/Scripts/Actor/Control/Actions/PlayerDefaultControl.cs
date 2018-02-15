using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Player/Default")]
public class PlayerDefaultControl : Action
{
    public override void perform (Controller c)
	{
		Player p = State.cast<Player> (c);

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));

		// Use Stasis Placement ablility
		if ((Input.GetKeyDown (PlayerControlManager.RH_FireStasis) ||
			Input.GetKeyDown(PlayerControlManager.LH_FireStasis)) && 
			GameManager.inst.canUseStasis)
			p.getSelf ().getAbility (0).use (p.getSelf (), mousePos);

        p.move();
	}
}
