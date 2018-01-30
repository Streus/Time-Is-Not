using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualDoor : Interactable 
{
	[Tooltip("Interact key (TEMPORARY)")]
	[SerializeField]
	private KeyCode _interactKey = KeyCode.E;

	[Tooltip("Shows if the door is open or not.")]
	[SerializeField]
	public bool _isOpen = false;

	//Shows if the player is close enough to open the door
	private bool _playerInRange = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
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
		_isOpen = !_isOpen;
		toggleActivatables ();
	}
}
