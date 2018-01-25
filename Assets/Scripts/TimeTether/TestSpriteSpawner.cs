using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpriteSpawner : MonoBehaviour 
{
	string prefabPath = "TestSprite"; 

	// Use this for initialization
	void Start () {
		
	}

	void Awake()
	{
		RegisteredObject.create(prefabPath, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			RegisteredObject.create(prefabPath, transform.position, transform.rotation); 
		}
	}
}
