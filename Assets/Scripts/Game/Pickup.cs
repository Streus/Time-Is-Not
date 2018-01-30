using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour 
{
	/// <summary>
	/// applies the pickup to the entity.
	/// </summary>
	/// <param name="entity">the entity to apply the pickup to.</param>
	public abstract void apply (Entity entity);

	void OnTriggerEnter2D(Collider2D col)
	{
		Entity e = col.GetComponent<Entity> ();
		if (e != null)
			apply (e);
	}
}
