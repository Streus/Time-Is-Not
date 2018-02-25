using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskChangingScript : MonoBehaviour {

	private SpriteRenderer _spriteRenderer;
	private SpriteMask _spriteMask;

	void Start ()
	{
		_spriteRenderer = GetComponent <SpriteRenderer> ();
		_spriteMask = GetComponent <SpriteMask> ();
	}

	void Update ()
	{
		if (_spriteRenderer.sprite != _spriteMask.sprite)
		{
			_spriteMask.sprite = _spriteRenderer.sprite;
		}
	}
}
