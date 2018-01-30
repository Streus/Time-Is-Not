using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour 
{
	/// <summary>
	/// applies the pickup to the entity.
	/// </summary>
	public void apply ()
	{
		
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<Entity> () != null) 
		{
			Entity e = col.GetComponent<Entity> ();
			if(e.getFaction() == Entity.Faction.player)
				apply ();
		}
	}
}
