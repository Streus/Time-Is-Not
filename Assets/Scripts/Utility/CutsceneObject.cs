﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneObject 
{
	[SerializeField]
	private GameObject _obj;

	[SerializeField]
	private Vector3 _endPosition = new Vector3();

	[SerializeField]
	private Vector3 _endScale = new Vector3(1,1, 1);

	[SerializeField]
	private float _time = Mathf.Infinity;

	public CutsceneObject()
	{
		_obj = null;
		_endPosition = new Vector3 ();
		_endScale = new Vector3 (1,1, 1);
		_time = Mathf.Infinity;
	}

	public GameObject Object
	{
		get{return _obj;}
		set{_obj = value;}
	}

	public Vector2 EndPosition
	{
		get{return _endPosition;}
		set{_endPosition = value;}
	}

	public Vector2 EndScale
	{
		get{return _endScale;}
		set{_endScale = value;}
	}

	public float Time
	{
		get{return _time;}
		set{_time = value;}
	}
}
