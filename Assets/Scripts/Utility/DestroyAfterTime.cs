using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour 
{
	public float destroyTimer = 3; 
	
	// Update is called once per frame
	void Update () 
	{
		destroyTimer -= Time.deltaTime;
		if (destroyTimer <= 0)
		{
			Destroy(this.gameObject); 
		}
	}
}
