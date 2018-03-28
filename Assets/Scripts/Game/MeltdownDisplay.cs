using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltdownDisplay : MonoBehaviour 
{
	[SerializeField]
	private Sprite[] _sprites;

	private SpriteRenderer _rend;

	[SerializeField]
	private MeltdownController _controller;


	// Use this for initialization
	void Awake () 
	{
		_rend = gameObject.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_controller == null)
		{
			Debug.LogError ("Error: no controller.");
			return;
		}
		if(_rend == null)
		{
			Debug.LogError ("Error: no sprite renderer on display object.");
			return;
		}
		if(_controller.State >= _sprites.Length || _controller.State < 0)
		{
			Debug.LogError ("Error: controller state out of bounds.");
			return;
		}
		_rend.sprite = _sprites [_controller.State];
	}
}
