using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour 
{
	public string sceneToLoad;
	bool changeAfterTimer = true;
	float timer = 3.45f;

	// Use this for initialization
	void Start () 
	{
		if(changeAfterTimer)
			 StartCoroutine (Load (timer));
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	void OnTriggerEnter2D(Collider2D col) 
	{
		if (col.CompareTag ("Player"))
			StartCoroutine(Load (0.5f));
	}


	public IEnumerator Load(float time)
	{
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene (sceneToLoad);
	}
}
