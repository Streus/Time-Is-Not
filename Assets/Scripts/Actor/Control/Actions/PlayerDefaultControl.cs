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

		bool inSBBounds = LevelStateManager.StasisBubbleAtPos (mousePos);

		// Use Stasis Placement ablility
		if (Input.GetKeyDown (PlayerControlManager.RH_FireStasis) ||
			Input.GetKeyDown(PlayerControlManager.LH_FireStasis))
		{
			if (GameManager.inst.canUseStasis &&
			   !inSBBounds &&
			   CursorManager.CursorInGameplayState ())
				c.getSelf ().getAbility (0).use (c.getSelf (), mousePos);

			if(!LevelStateManager.canAddStasisBubble() && !GlobalAudio.ClipIsPlaying(AudioLibrary.inst.stasisError))
				AudioLibrary.PlayStasisErrorSound();
		}

		// Use Dash ability
		if (Input.GetKeyDown (PlayerControlManager.RH_Dash) ||
		   Input.GetKeyDown (PlayerControlManager.LH_Dash))
		{
			if (GameManager.inst.canUseDash && CursorManager.CursorInGameplayState ())
				c.getSelf ().getAbility (1).use (c.getSelf (), p.getJumpTargetPos ());
		}

        p.move();
		p.findTarget ();
	}
}
