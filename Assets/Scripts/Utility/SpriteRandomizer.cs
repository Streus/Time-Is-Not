using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour 
{
	[SerializeField]
	private List<WeightedSprite> _spriteList;

	[SerializeField]
	private string _tag;

	public void RandomizeAll()
	{
		GameObject[] floorTiles = GameObject.FindGameObjectsWithTag(_tag);
		for(int i = 0; i < floorTiles.Length; i++)
		{
			if (floorTiles [i].GetComponent<SpriteRenderer> () != null)
				RandomizeMe (floorTiles [i].GetComponent<SpriteRenderer> ());
		}
	}

	public void RandomizeMe(SpriteRenderer rend)
	{
		rend.sprite = Generate ();
	}

	public Sprite Generate()
	{
		//calculate total value
		int range = 0;
		for (int i = 0; i < _spriteList.Count; i++) 
		{
			range += _spriteList [i].Weight;
		}


		//generate a number
		int rand = Random.Range(0, range);
		int top = 0;
		//find the item
		for (int i = 0; i < _spriteList.Count; i++) 
		{
			top += _spriteList[i].Weight;
			if (rand < top)
				return _spriteList[i].Sprite;
		}
		return _spriteList[0].Sprite;
	}
}
