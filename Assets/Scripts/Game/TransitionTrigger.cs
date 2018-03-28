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
		if (activated && TetherManager.inst.EndLevelAllTetherPointsCollected())
		{
			//start transition
			sst = ScreenShaderTransition.getInstance("LevelChangeTransition");
			if (sst != null)
			{
				sst.SetFadeOut ();
				sst.fadeOutDone += performTransition;
			}
		}
	}

	// Trigger end of level clean-up
	public void OnTriggerEnter2D(Collider2D col)
	{
		GameManager.inst.EnterPauseState (PauseType.CUTSCENE);
		TetherManager.inst.EndLevelRemoveAllTetherPoints ();

		activated = true;
	}

	// Move to the next level
	private void performTransition()
	{
		sst.fadeOutDone -= performTransition;
		TransitionBuddy.getInstance().endCurrentScene(nextScene);
	}
}
