using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class TransitionTrigger : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The scene to which the game should transition.")]
	private string nextScene = "";

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
				sst.SetFadeOut ();
				sst.fadeOutDone += performTransition;
				activated = false;
			}
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
//		activated = true;
	}
		
	#region DON'T LOOK PLS THANKS
	private IEnumerator temp_delay()
	{
		yield return new WaitForSecondsRealtime (1f);
		activated = true;
	}
	#endregion

	// Move to the next level
	private void performTransition()
	{
		sst.fadeOutDone -= performTransition;
		TransitionBuddy.getInstance().endCurrentScene(nextScene);
	}
}
