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

	private float timer;

	private Vector3 scale;

	private Vector3 position;

	private float scaleSpeed;

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
		if(timer != Mathf.Infinity)
		{
			slides[currentScene].Object.transform.localScale = Vector3.MoveTowards (slides[currentScene].Object.transform.localScale, scale, scaleSpeed);
			slides[currentScene].Object.transform.position = Vector3.MoveTowards (slides[currentScene].Object.transform.position, position, moveSpeed);

		}
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) || timer <= 0)
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
		scale = slides [scene].Object.transform.localScale;
		position = slides [scene].Object.transform.position;
		scaleSpeed = Vector3.Distance(scale, slides[scene].EndScale) / timer * Time.deltaTime; 
		moveSpeed = Vector3.Distance(position, slides[scene].EndPosition) / timer * Time.deltaTime; 
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
