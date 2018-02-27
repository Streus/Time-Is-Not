using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : StatusComponent
{
	#region STATIC_VARS
	#endregion

	#region INSTANCE_VARS

	#endregion

	#region INSTANCE_METHODS
	public override void onApply (Entity subject)
	{
		for (int i = 0; i < subject.abilityCount; i++)
		{
			subject.getAbility (i).available = false;
		}
	}

	public override void onRevert (Entity subject)
	{
		for (int i = 0; i < subject.abilityCount; i++)
		{
			subject.getAbility (i).available = true;
		}
	}
	#endregion
}
