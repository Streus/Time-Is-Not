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
	private CutsceneObject[] slides;

	[SerializeField]
	private float timer;

	[SerializeField]
	private Vector3 scale;

	[SerializeField]
	private Vector3 position;

	[SerializeField]
	private float scaleSpeed;

	[SerializeField]
	private float moveSpeed;

	// Use this for initialization
	void Awake () 
	{
		currentScene = 0;
		MakeSceneActive (currentScene);
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;
		slides[currentScene].Object.transform.localScale = Vector3.MoveTowards (slides[currentScene].Object.transform.localScale, scale, scaleSpeed);
		slides[currentScene].Object.transform.position = Vector3.MoveTowards (slides[currentScene].Object.transform.position, position, moveSpeed);

		if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0) || timer <= 0)
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
			slides [i].Object.SetActive (false);
		}
		slides [scene].Object.SetActive (true);
		timer = slides [scene].Time;
		scale = slides [scene].EndScale;
		position = slides [scene].EndPosition;
		scaleSpeed = Vector3.Distance(scale, slides[currentScene].Object.transform.localScale) / timer * Time.deltaTime; 
		moveSpeed = Vector3.Distance (position, slides [currentScene].Object.transform.position) / timer * Time.deltaTime;	; 
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
