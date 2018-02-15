using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrientationCorrector : MonoBehaviour 
{
	[Tooltip("The rotation to orient the object to")]
	[SerializeField]
	private Vector3 _worldSpaceRotation = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.eulerAngles = _worldSpaceRotation;
	}
}
