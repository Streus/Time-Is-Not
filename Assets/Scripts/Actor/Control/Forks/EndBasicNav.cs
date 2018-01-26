using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/EndBasicNav")]
public class EndBasicNav : Fork
{
	public override bool check (Controller c)
	{
		Vector3 pos;
		return c.currentPosition (out pos);
	}
}
