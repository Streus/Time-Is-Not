using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class TransitionTrigger : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The scene to which the game should transition.")]
	private string nextScene = "";

	private bool activated = false;

	public void Update()
	{
		if (activated && IndicatorReturnObject.NoInstancesExist ())
		{
			//start transition
			//TODO delay this by fade-out
			TransitionBuddy.getInstance().endCurrentScene(nextScene);
		}
	}

	// Trigger end of level clean-up
	public void OnTriggerEnter2D(Collider2D col)
	{
		GameManager.inst.EnterPauseState (PauseType.CUTSCENE);
		TetherManager.inst.EndLevelRemoveAllTetherPoints ();

		activated = true;
	}
}
