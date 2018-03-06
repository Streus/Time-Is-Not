using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Player/Disabled")]
public class PlayerDisabled : Action
{
	public override void perform (Controller c)
	{
		Player p = State.cast<Player> (c);

		p.findTarget ();
	}
}
