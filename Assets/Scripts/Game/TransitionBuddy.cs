﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransitionBuddy
{
	#region STATIC_VARS

	//singleton instance
	private static TransitionBuddy instance;
	#endregion

	#region INSTANCE_VARS

	private ScreenShaderTransition transitionEffect;
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
		transitionEffect = null;
		Debug.Log ("[TransitionBuddy] I'm back!"); //DEBUG TB
	}

	~TransitionBuddy()
	{
		Debug.Log ("[TransitionBuddy] I'm going away. Bye!"); //DEBUG TB
	}

	public void endCurrentScene(string nextScene)
	{
		SceneManager.LoadScene (nextScene);
		SceneManager.SetActiveScene (SceneManager.GetSceneByName (nextScene));

		Debug.Log ("[TransitionBuddy] Okay, time to go! New scene, here we come!"); //DEBUG TB
	}

	//Called when a new scene starts, after awake
	private void beginNewScene (Scene prev, Scene curr)
	{
		Debug.Log ("[TransitionBuddy] We're in a brand new scene. Shiny!"); //DEBUG TB

		//save
		SaveManager.level = SceneManager.GetActiveScene ().name;
		if(SaveManager.level != "Main Menu" && SaveManager.level != "Credits" && SaveManager.level != "Secret Level")
			SaveManager.Save ();

		//start cutscene pause
		if (GameManager.inst != null)
			GameManager.inst.EnterPauseState (PauseType.CUTSCENE);
		else
			Debug.LogWarning ("No GameManager! Could not pause for transition in.");

		//start fade effect
		transitionEffect = ScreenShaderTransition.getInstance("LevelChangeTransition");
		if (transitionEffect != null)
		{
			transitionEffect.SetFadeIn ();
			transitionEffect.fadeInDone += startPlayerCutscene;
		}
		else
		{
			Debug.LogWarning ("Fade In not set up!");
			finishSceneSetup ();
		}
	}

	private void startPlayerCutscene()
	{
		if (transitionEffect != null)
		{
			transitionEffect.fadeInDone -= finishSceneSetup;
			transitionEffect = null;
		}

		//listen to player tetherAnchorEnded event
		Player p = GameManager.GetPlayer().GetComponent<Player>();
		p.tetherAnchorEnded += finishSceneSetup;
		//p.setPlaceAnchorAnim ();
		//TetherManager.inst.CreatePoint(); 
		TetherManager.inst.ManualStart(); 
	}

	private void finishSceneSetup()
	{
		GameManager.GetPlayer ().GetComponent<Player> ().tetherAnchorEnded -= finishSceneSetup;
		GameManager.inst.ExitPauseState ();

		//start up header display
		LevelNameHeader lnh = LevelNameHeader.getMain();
		if (lnh != null)
		{
			if (SceneSetup.inst == null || SceneSetup.inst.useLevelHeader)
				LevelNameHeader.getMain ().SetHeaderState (LevelNameHeader.HeaderState.WAIT_TO_APPEAR);
		}
		else
			Debug.LogWarning ("No main LevelNameHeader!");

		Debug.Log ("[TransitionBuddy] All done setting up. Have fun!");
	}
	#endregion
}
