using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnAwake : MonoBehaviour 
{
	public bool disabled; 

	public GameObject affectedObject; 

	void Awake()
	{
		if (!disabled && affectedObject != null)
			affectedObject.SetActive(true); 
	}
}
