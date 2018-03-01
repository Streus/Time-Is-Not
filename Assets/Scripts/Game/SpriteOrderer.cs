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
			sprites [i].sortingOrder =  (int)Mathf.Round (sprites [i].transform.position.y * 100) * -1;
		}
	}

	public int OrderMe(Transform trans)
	{
		int layer = -1 * (int)Mathf.Round (trans.position.y * 100);
			
		return layer;
	}
}
