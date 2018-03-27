using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransitionBuddy
{
	#region STATIC_VARS

	//singleton instance
	private static TransitionBuddy instance;
	#endregion

	#region INSTANCE_VARS

	#endregion

	#region STATIC_METHODS

	public static TransitionBuddy getInstance()
	{
		if (instance == null)
			instance = new TransitionBuddy ();
		return instance;
	}
	#endregion

	#region INSTANCE_METHODS

	private TransitionBuddy()
	{
		SceneManager.activeSceneChanged += beginNewScene;
		Debug.Log ("[TransitionBuddy] I'm back!"); //DEBUG TB
	}

	~TransitionBuddy()
	{
		Debug.Log ("[TransitionBuddy] I'm going away. Bye!"); //DEBUG TB
	}

	public void endCurrentScene(string nextScene)
	{
		SceneManager.LoadScene (nextScene);
		Debug.Log ("Loaded " + nextScene); //DEBUG
		SceneManager.SetActiveScene (SceneManager.GetSceneByName (nextScene));

		Debug.Log ("[TransitionBuddy] Okay, time to go! New scene, here we come!"); //DEBUG TB
	}

	//Called when a new scene starts
	private void beginNewScene (Scene prev, Scene curr)
	{
		//TODO fade-in effect
		//TODO save this level as the most current level
		if(LevelNameHeader.getMain() != null)
			LevelNameHeader.getMain().SetHeaderState(LevelNameHeader.HeaderState.APPEAR);

		Debug.Log ("[TransitionBuddy] We're in a brand new scene. Shiny!"); //DEBUG TB
	}
	#endregion
}
