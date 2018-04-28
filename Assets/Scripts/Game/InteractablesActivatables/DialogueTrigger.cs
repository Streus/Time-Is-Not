using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IActivatable, ISavable
{
	[SerializeField]
	private DialogueObject[] dialogueChain;

	[SerializeField]
	private bool freezesPlayer;

	int id;

	[SerializeField]
	private bool clearsViaDistance = false;
	[SerializeField]
	private float distance = 20f;

	public float curDist;

	float ignoreTimer = 0;

	bool distHelper;

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

	void Update()
	{
		if (ignoreTimer > 0)
			ignoreTimer -= Time.deltaTime;
		if(clearsViaDistance)
		{
			Player player = GameObject.FindObjectOfType<Player> ();
			if(player != null)
			{
				float dist = Vector3.Distance (player.transform.position, transform.position);
				curDist = dist;
				if (dist > distance && distHelper) {
					onActivate (false);
					distHelper = false;
				}
			}
		}
	}

	/// <summary>
	/// Display Dialogue.
	/// </summary>
	public bool onActivate()
	{
		//if (dialogueChain.Length == 0)
		if (dialogueChain.Length == 0 || ignoreTimer > 0)
			return false;

		distHelper = true;

		id = DialogueManager.inst.CreateBox (dialogueChain [0]);
		return true;
	}

	/// <summary>
	/// Display Dialogue.
	/// </summary>
	public bool onActivate (bool state)
	{
		//if (dialogueChain.Length == 0)
		if (dialogueChain.Length == 0 || ignoreTimer > 0)
			return false;
		if (!state)
		{
			distHelper = false;
			DialogueManager.inst.RemoveMyDialogue (id);
			return true;
		}
		distHelper = true;
		id = DialogueManager.inst.CreateBox (dialogueChain [0]);
		dialogueChain [0].FreezePlayer = false;
		return true;
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		SeedBase seed = new SeedBase ();

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		if (s == null)
			return;
		ignoreTimer = 1f;
	}
}
