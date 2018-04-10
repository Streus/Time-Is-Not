using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScroller : MonoBehaviour 
{
	private LineRenderer _line;
	private Material _mat;
	[SerializeField]
	private float _speed;

	// Use this for initialization
	void Start () 
	{
		_line = gameObject.GetComponent<LineRenderer> ();
		_mat = new Material (_line.material);
		_line.material = _mat;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		_line.material.mainTextureOffset += new Vector2 (Time.deltaTime * _speed, 0); 
		
	}
}
