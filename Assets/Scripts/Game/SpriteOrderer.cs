using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderer : Singleton<SpriteOrderer> 
{

	public void OrderAll()
	{
		SpriteRenderer[] sprites = GameObject.FindObjectsOfType<SpriteRenderer> ();

		for(int i = 0; i < sprites.Length; i ++)
		{
			float offset = 0;

			if (sprites[i].gameObject.GetComponent<Collider2D> () != null)
				offset = sprites[i].gameObject.GetComponent<Collider2D> ().offset.y;
			
			sprites [i].sortingOrder = (int)Mathf.Round ((sprites [i].transform.position.y + offset) * 100) * -1;
		}
	}

	public int OrderMe(Transform trans)
	{
		float offset = 0;

		if (trans.GetComponent<Collider2D> () != null)
			offset = trans.GetComponent<Collider2D> ().offset.y;

		int layer = -1 * (int)Mathf.Round ((trans.position.y + offset) * 100);
			
		return layer;
	}
}
