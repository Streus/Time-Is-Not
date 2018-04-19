using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class TransitionTrigger : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The scene to which the game should transition.")]
	private string nextScene = "";

	[SerializeField]
	[Tooltip ("The time to wait between cleanup and starting the fade out.")]
	private float initDelayTime = 1.5f;

	[SerializeField]
	[Tooltip ("The time to wait before transitoning after all cleanup and fade out is done.")]
	private float finalDelayTime = 0.5f;

	private bool activated = false;

	private ScreenShaderTransition sst;

	public void Update()
	{
		//stay idle waiting for player to finish / cleanup to be done
		if (activated && 
			IndicatorReturnObject.NoInstancesExist() &&
			TetherManager.inst.EndLevelAllTetherPointsCollected())
		{
			//start transition
			sst = ScreenShaderTransition.getInstance("LevelChangeTransition");
			if (sst != null)
			{
				StartCoroutine (initDelay ());
			}
			else
				Debug.LogWarning ("No Screen Shader Transition set up!");
		}
	}

	// Trigger end of level clean-up
	public void OnTriggerEnter2D(Collider2D col)
	{
		if (!col.gameObject.CompareTag ("Player"))
			return;
		GameManager.inst.EnterPauseState (PauseType.CUTSCENE);
		TetherManager.inst.EndLevelRemoveAllTetherPoints ();

		//FIXME temporary solution to waiting for tethers to be destroyed
		StartCoroutine (temp_delay ());

		//this file contains all my sins
		Player p = GameManager.GetPlayer ().GetComponent<Player> ();
		p.enabled = false;
		p.setDashingAnim (false);
		GameManager.GetPlayer ().GetComponent<Animator> ().SetBool ("isMoving", false);
	}
		
	#region DON'T LOOK PLS THANKS
	private IEnumerator temp_delay()
	{
		yield return new WaitForSecondsRealtime (1f);
		activated = true;
	}

	private IEnumerator initDelay()
	{
		activated = false;
		yield return new WaitForSecondsRealtime (initDelayTime);
		sst.SetFadeOut ();
		sst.fadeOutDone += performTransition;
	}

	private IEnumerator endDelay()
	{
		yield return new WaitForSecondsRealtime (finalDelayTime);
		TransitionBuddy.getInstance ().endCurrentScene (nextScene);
	}
	#endregion

	// Move to the next level
	private void performTransition()
	{
		sst.fadeOutDone -= performTransition;
		StartCoroutine (endDelay ());
	}
}
