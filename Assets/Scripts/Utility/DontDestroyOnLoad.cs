using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour 
{
	public static GameObject inst; 

	void Awake() 
	{
		if (inst == null)
		{
			inst = this.gameObject; 
			DontDestroyOnLoad(transform.gameObject);
		}
		else if (this.gameObject != inst)
		{
			Destroy(this.gameObject); 
		}
	}
}
