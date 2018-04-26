using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IActivatable
{
	[SerializeField]
	private DialogueObject[] dialogueChain;

	[SerializeField]
	private bool freezesPlayer;

	int id;

	void Start()
	{
		for(int i = 0; i < dialogueChain.Length; i++)
		{
			dialogueChain [i].FreezePlayer = freezesPlayer;
			dialogueChain [i].Timer = dialogueChain[i].Lifetime;
		}
		for(int i = 0; i < dialogueChain.Length -1; i++)
		{
			dialogueChain [i].Next = dialogueChain [i + 1];
		}
	}

	/// <summary>
	/// Display Dialogue.
	/// </summary>
	public bool onActivate()
	{
		if (dialogueChain.Length == 0)
			return false;
		id = DialogueManager.inst.CreateBox (dialogueChain [0]);
		return true;
	}

	/// <summary>
	/// Display Dialogue.
	/// </summary>
	public bool onActivate (bool state)
	{
		if (dialogueChain.Length == 0)
			return false;
		if (!state)
		{
			DialogueManager.inst.RemoveMyDialogue (id);
			return true;
		}
		id = DialogueManager.inst.CreateBox (dialogueChain [0]);
		dialogueChain [0].FreezePlayer = false;
		return true;
	}
}
