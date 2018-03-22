using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger: MonoBehaviour, IActivatable
{
	[SerializeField]
	private string _levelToLoad;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// Toggle the object's state.
	/// </summary>
	public bool onActivate()
	{
		Scene scene = SceneManager.GetSceneByName (_levelToLoad);
		if (scene == null)
			Debug.LogError ("Error, no such scene exists.");
		else
			SceneManager.LoadScene (_levelToLoad);
	}

	/// <summary>
	/// Set the object's state.
	/// </summary>
	public bool onActivate (bool state)
	{
		Scene scene = SceneManager.GetSceneByName (_levelToLoad);
		if (scene == null)
			Debug.LogError ("Error, no such scene exists.");
		else
			SceneManager.LoadScene (_levelToLoad);
	}
}
