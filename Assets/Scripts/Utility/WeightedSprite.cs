using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedSprite 
{
	[SerializeField]
	private Sprite _sprite;
	[SerializeField]
	private int _weight;

	public WeightedSprite()
	{
		_sprite = null;
		_weight = 0;
	}

	public Sprite Sprite
	{
		get{return _sprite;}
		set{_sprite = value;}
	}

	public int Weight
	{
		get{return _weight;}
		set{_weight = value;}
	}

}
