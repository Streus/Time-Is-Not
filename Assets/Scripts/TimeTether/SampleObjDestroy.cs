using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleObjDestroy : MonoBehaviour 
{
	public KeyCode destroyKey; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(destroyKey))
		{
			//RegisteredObject.destroy(this.gameObject);
			Destroy(this.gameObject); 
		}
	}
}
