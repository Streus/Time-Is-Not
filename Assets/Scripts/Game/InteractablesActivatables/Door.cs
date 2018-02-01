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
	private bool _isOpen = false;

	[Tooltip("Is the door openable by hand or controled via interactable")]
	[SerializeField]
	private DoorTypes _type = DoorTypes.Electronic;

	//the door's sprite
	private SpriteRenderer _sprite;

	[Tooltip("The open sprite")]
	[SerializeField]
	private Sprite _openSprite;

	[Tooltip("The closed sprite")]
	[SerializeField]
	private Sprite _closedSprite;

	[Tooltip("Button prompt sprite")]
	[SerializeField]
	private GameObject _buttonPrompt;

	[Tooltip("Negative button prompt sprite.")]
	[SerializeField]
	private GameObject _negativePrompt;

	//Shows if the player is close enough to open the door
	private bool _playerInRange = true;

	//collider of the door
	private PolygonCollider2D _collider;

	public enum DoorTypes {Manual, Electronic};


	// Use this for initialization
	void Start () 
	{
		_collider = gameObject.GetComponent<PolygonCollider2D> ();
		_sprite = gameObject.GetComponent<SpriteRenderer> ();

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
				if(_type == DoorTypes.Manual && isEnabled())
				{
					_buttonPrompt.SetActive (true);
					_negativePrompt.SetActive (false);
				}
				else
				{
					_negativePrompt.SetActive (true);
					_buttonPrompt.SetActive (false);
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
				_buttonPrompt.SetActive (false);
				_negativePrompt.SetActive (false);
			}
		}
	}

	/// <summary>
	/// Checks the keyboard for input.
	/// </summary>
	void getInput()
	{
		if(_playerInRange && Input.GetKeyDown(_interactKey) && (_type == DoorTypes.Manual) && isEnabled())
		{
			onInteract ();
		}
	}

	public override void onInteract ()
	{
		_isOpen = !_isOpen;

		if(_isOpen)
		{
			Open ();
		}
		else
		{
			Close ();
		}
	}

	/// <summary>
	/// Toggle the object's state.
	/// </summary>
	public bool onActivate()
	{
		if (_type == DoorTypes.Manual)
			return _isOpen;
		if(!_isOpen)
		{
			Open ();
		}
		else
		{
			Close ();
		}
		return _isOpen;
	}

	/// <summary>
	/// Set the object's state.
	/// </summary>
	public bool onActivate (bool state)
	{
		if (_type == DoorTypes.Manual)
			return _isOpen;
		if(state)
		{
			Open ();
		}
		else
		{
			Close ();
		}
		return _isOpen;
	}

	void Open()
	{
		_isOpen = true;
		_sprite.sprite = _openSprite;
		_collider.enabled = false;
	}

	void Close()
	{
		_isOpen = false;
		_sprite.sprite = _closedSprite;
		_collider.enabled = true;
	}
}
