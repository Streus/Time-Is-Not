using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/BasicNav")]
public class BasicNav : Action
{
	public override void perform (Controller c)
	{
		Vector3 pos;
		if (c.currentPosition (out pos))
		{
			float dist = Vector3.Distance (c.transform.position, pos);
			if (dist < (float)c.getSelf ().getMovespeed () * Time.deltaTime)
			{
				if (!c.nextPosition (out pos))
					return;
			}

			Vector3 movevec = pos - c.transform.position;
			c.transform.Translate (movevec.normalized * c.getSelf ().getMovespeed () * Time.deltaTime);
		}
	}
}
