using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/HermitCrab/AtHome")]
public class HCAtHome : Fork
{
	public override bool check (Controller c)
	{
		HermitCrab hc = State.cast<HermitCrab> (c);

		if (c.getMap () == null)
		{
			//not sure how you would get here without an Atlas, but okay. ナイスジョブ
			Debug.LogError (hc.name + " needs an Atlas!");
			return false;
		}
		if (Vector2.Distance (c.transform.position, hc.getHome ()) <= c.getSelf ().getMovespeed () * Time.deltaTime)
		{
			c.transform.position = hc.getHome ();
			return true;
		}
		return false;
	}
}
