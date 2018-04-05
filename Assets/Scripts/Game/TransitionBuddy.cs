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
		//save
		SaveManager.level = SceneManager.GetActiveScene ().name;
		if(SaveManager.level != "Main Menu")
			SaveManager.Save ();

		//start cutscene pause
		if(GameManager.inst != null)
			GameManager.inst.EnterPauseState (PauseType.CUTSCENE);

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

		Debug.Log ("[TransitionBuddy] We're in a brand new scene. Shiny!"); //DEBUG TB
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
		p.setPlaceAnchorAnim ();
	}

	private void finishSceneSetup()
	{
		GameManager.GetPlayer ().GetComponent<Player> ().tetherAnchorEnded -= finishSceneSetup;

		//start up header display
		if (LevelNameHeader.getMain () != null)
			LevelNameHeader.getMain ().SetHeaderState (LevelNameHeader.HeaderState.APPEAR);
		else
			Debug.LogWarning ("No main LevelNameHeader!");

		Debug.Log ("[TransitionBuddy] All done setting up. Have fun!");
	}
	#endregion
}
