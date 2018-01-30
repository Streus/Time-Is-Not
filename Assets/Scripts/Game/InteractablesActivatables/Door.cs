using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable, IActivatable
{
	[Tooltip("Interact key (TEMPORARY)")]
	[SerializeField]
	private KeyCode _interactKey = KeyCode.E;

	[Tooltip("Shows if the door is open or not.")]
	[SerializeField]
	public bool _isOpen = false;

	[Tooltip("Is the door openable by hand or controled via interactable")]
	[SerializeField]
	public DoorTypes _type = DoorTypes.Electronic;

	//the door's sprite
	private Sprite sprite;

	//Shows if the player is close enough to open the door
	private bool _playerInRange = true;

	public enum DoorTypes {Manual, Electronic};


	// Use this for initialization
	void Start () 
	{
		sprite = gameObject.GetComponent<Sprite> ();

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
				if(_type == DoorTypes.Manual)
				{
					//TODO: display button prompt
				}
				else
				{
					//TODO: display negative prompt
				}

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
		if(_playerInRange && Input.GetKeyDown(_interactKey) && (_type == DoorTypes.Manual))
		{
			onInteract ();
		}
	}

	public override void onInteract ()
	{
		_isOpen = !_isOpen;

		if(_isOpen)
		{
			//TODO: play open animation
		}
		else
		{
			//TODO: Player close animation
		}
	}

	/// <summary>
	/// Toggle the object's state.
	/// </summary>
	public bool onActivate()
	{
		if (_type == DoorTypes.Manual)
			return;
		_isOpen = !_isOpen;
		if(_isOpen)
		{
			//TODO: play open animation
		}
		else
		{
			//TODO: Player close animation
		}
	}

	/// <summary>
	/// Set the object's state.
	/// </summary>
	public bool onActivate (bool state)
	{
		if (_type == DoorTypes.Manual)
			return;
		_isOpen = state;
		if(_isOpen)
		{
			//TODO: play open animation
		}
		else
		{
			//TODO: Player close animation
		}
	}

	void Open()
	{
		_isOpen = true;
	}

	void Close()
	{
		_isOpen = false;
	}
}
