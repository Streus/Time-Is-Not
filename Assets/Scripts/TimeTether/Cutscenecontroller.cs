using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscenecontroller : MonoBehaviour 
{
	private int currentScene = 0;

	[SerializeField]
	private string _levelToLoad;

	[SerializeField]
	private GameObject[] slides;

	// Use this for initialization
	void Awake () 
	{
		currentScene = 0;
		MakeSceneActive (currentScene);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
		{
			currentScene++;
			MakeSceneActive (currentScene);
		}
	}

	void MakeSceneActive(int scene)
	{
		if (scene >= slides.Length) 
		{
			ChangeScene ();
			return;
		}
		for(int i = 0; i < slides.Length; i++)
		{
			slides [i].SetActive (false);
		}
		slides [scene].SetActive (true);
	}

	public bool ChangeScene()
	{
		Scene scene = SceneManager.GetSceneByName (_levelToLoad);
		if (scene == null) {
			Debug.LogError ("Error, no such scene exists.");
			return false;
		}
		else 
		{
			SceneManager.LoadScene (_levelToLoad);
			return true;
		}

	}
}
