using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable 
{
	[Tooltip("Interact button(TEMPORARY)")]
	[SerializeField]
	private KeyCode _interactKey = KeyCode.E;

	//is the player close enough to use the button?
	private bool _playerInRange = false;


	// Use this for initialization
	void Start () 
	{
		//TODO: get input button from input module
	}
	
	// Update is called once per frame
	void Update () 
	{
		getInput ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.GetComponent<Entity> () != null) 
		{
			//Check if 
			Entity entityHit = col.gameObject.GetComponent<Entity> ();
			if (entityHit.getFaction () == Entity.Faction.player) 
			{
				_playerInRange = true;
				//TODO: display button prompt
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.GetComponent<Entity> () != null) 
		{
			Entity entityHit = col.gameObject.GetComponent<Entity> ();
			if (entityHit.getFaction () == Entity.Faction.player) 
			{
				_playerInRange = false;
				//TODO: hide button prompt
			}
		}
	}

	/// <summary>
	/// Checks the keyboard for input.
	/// </summary>
	void getInput()
	{
		if(_playerInRange && Input.GetKeyDown(_interactKey))
		{
			onInteract ();
		}
	}

	public override void onInteract ()
	{
		toggleActivatables ();
	}
}
