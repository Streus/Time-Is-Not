using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class Door : Interactable, IActivatable, ISavable
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
	private bool _playerInRange = false;

	//collider of the door
	private PolygonCollider2D _collider;

	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = false;

	public enum DoorTypes {Manual, Electronic};


	// Use this for initialization
	void Start () 
	{
		_playerInRange = false;
		_collider = gameObject.GetComponent<PolygonCollider2D> ();
		_sprite = gameObject.GetComponent<SpriteRenderer> ();

		GetComponent<RegisteredObject> ().allowResetChanged += ToggleStasis;

	}

	// Update is called once per frame
	void Update () 
	{
		#if UNITY_EDITOR
		_sprite = gameObject.GetComponent<SpriteRenderer> ();
		if(_isOpen && _sprite.sprite != _openSprite)
		{
			_sprite.sprite = _openSprite;
			_collider.enabled = false;
		}
		if(!_isOpen &&  _sprite.sprite != _closedSprite)
		{
			_sprite.sprite = _closedSprite;
			_collider.enabled = true;
		}
		#endif
		getInput ();

		if(!inStasis)
		{
			if(_isOpen && _collider.enabled)
			{
				_sprite.sprite = _openSprite;
				_collider.enabled = false;
			}
			if(!_isOpen && !_collider.enabled)
			{
				_sprite.sprite = _closedSprite;
				_collider.enabled = true;
			}
		}
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

	public void OnDestroy()
	{
		GetComponent<RegisteredObject> ().allowResetChanged -= ToggleStasis;
	}

	/// <summary>
	/// Checks the keyboard for input.
	/// </summary>
	void getInput()
	{
		if(_playerInRange && Input.GetKeyDown(_interactKey) && (_type == DoorTypes.Manual) && isEnabled() && !inStasis && !GameManager.isPaused())
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
		if (_type == DoorTypes.Manual) {
			Debug.Log("NOTE: This door cannot be opened remotely, please change it to an electronic door.");
			return _isOpen;
		}
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
		if (_type == DoorTypes.Manual || inStasis)
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
	}

	void Close()
	{
		_isOpen = false;
	}


	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

		seed.isOpen = _isOpen;

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		if (s == null || inStasis)
			return;
		
		Seed seed = (Seed)s;

		if (seed.isOpen)
			Open ();
		else
			Close ();
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the door open?
		public bool isOpen;
	}


	//****Stasisable Object Functions****

	/// <summary>
	/// Toggles if the object is in stasis.
	/// </summary>
	/// <param name="turnOn">If set to <c>true</c> turn on.</param>
	private void ToggleStasis(bool turnOn)
	{
		inStasis = turnOn;
		if (inStasis)
			_sprite.color = Color.yellow;
		else
			_sprite.color = Color.white;
	}

	/// <summary>
	/// shows if the object is in stasis
	/// </summary>
	/// <returns><c>true</c>, if stasis is active, <c>false</c> otherwise.</returns>
	public bool InStasis()
	{
		return inStasis; 
	}
}
