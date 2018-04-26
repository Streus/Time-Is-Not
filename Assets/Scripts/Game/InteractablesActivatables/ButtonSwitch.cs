using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitch : Interactable, ISavable, IActivatable
{
	/*[Tooltip("Time before the button triggers again (if 0 or infinity, will have no timer.")]
	[SerializeField]*/
	private float _timer = 0;

	[Tooltip("Interact button(TEMPORARY)")]
	[SerializeField]
	private KeyCode _interactKey = KeyCode.E;

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

	[Tooltip("Button prompt sprite")]
	[SerializeField]
	private GameObject _buttonPrompt;

	[Tooltip("Negative button prompt sprite.")]
	[SerializeField]
	private GameObject _negativePrompt;

	[Tooltip("Unpressed state sprite.")]
	[SerializeField]
	private Sprite _unpressedSprite;

	[Tooltip("Pressed state sprite.")]
	[SerializeField]
	private Sprite _pressedSprite;

	[SerializeField]
	private bool _reusable = true;

	//spriteRenderer for the object
	private SpriteRenderer _sprite;


	//is the player close enough to use the button?
	private bool _playerInRange = false;

	private bool isInverted;

	[SerializeField]
	[Tooltip("If true, this button will play an alert flash when it's pressed.")]
	bool alertFlashOnInteract; 


	// Use this for initialization
	void Start () 
	{
		//TODO: fix timed switch functionality
		_timer = 0;
		_sprite = gameObject.GetComponent<SpriteRenderer> ();
		//TODO: get input button from input module
		isInverted = !isEnabled();
	}
	
	// Update is called once per frame
	void Update () 
	{
		getInput ();
	}

	//Draw lines to all linked activatables
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		for(int i = 0; i < _activatables.Length; i++)
		{
			if(_activatables[i] != null)
				Gizmos.DrawLine (transform.position, _activatables[i].transform.position);
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
				//show button prompts
				if(isEnabled())
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
				//hide button prompts
				_buttonPrompt.SetActive (false);
				_negativePrompt.SetActive (false);
			}
		}
	}

	IEnumerator buttonTimer(float seconds)
	{		
		yield return new WaitForSeconds (seconds);
		onInteract ();
	}

	/// <summary>
	/// Checks the keyboard for input.
	/// </summary>
	void getInput()
	{
		//if(_playerInRange && Input.GetKeyDown(_interactKey) && !GameManager.isPaused() && !GameManager.CameraIsZoomedOut())
		if (_playerInRange && Input.GetKeyDown(_interactKey) && GameManager.inst.pauseType == PauseType.NONE && isEnabled() && DialogueManager.inst.ActiveDialogues.Count == 0)
		{
			onInteract ();
			if(!_reusable)
			{
				_negativePrompt.SetActive (true);
				_buttonPrompt.SetActive (false);
				disable ();
				return;
			}
			if(_timer != Mathf.Infinity && _timer > 0)
			{
				StartCoroutine (buttonTimer (_timer));
			}
		}
	}

	/// <summary>
	/// Trigger the Interactable.
	/// </summary>
	public override void onInteract ()
	{
		ToggleSecurityDoors ();
        AudioLibrary.PlayNormalSwitchSound();
		if (alertFlashOnInteract)
		{
			AlertFlash.inst.PlayAlertFlash(); 
		}
		if (_activatables.Length == 0)
			return;
		foreach(GameObject activatable in _activatables)
		{
			if(activatable.GetComponent<IActivatable>() != null)
				activatable.GetComponent<IActivatable>().onActivate ();
		}
		_sprite.sprite = _pressedSprite;
		if(_reusable)
			StartCoroutine (spriteTimer (0.4f));
	}

	IEnumerator spriteTimer(float seconds)
	{
		yield return new WaitForSeconds (seconds);
		_sprite.sprite = _unpressedSprite;
	}

	/// <summary>
	/// Toggle the object's state.
	/// </summary>
	public bool onActivate()
	{
		if(isEnabled())
		{
			_sprite.sprite = _pressedSprite;
			disable ();
		}
		else
		{
			enable ();
		}
		return isEnabled();
	}

	/// <summary>
	/// Set the object's state.
	/// </summary>
	public bool onActivate (bool state)
	{
		//if the door is inverted, a true state closes the door
		if(isInverted)
		{
			if (state)
				enable();
			else {
				_sprite.sprite = _pressedSprite;
				disable ();
			}
		}
		else
		{
			if (state) {
				_sprite.sprite = _pressedSprite;
				disable ();
			}
			else
				enable();
		}
		return isEnabled();
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

		seed.canBePressed = isEnabled ();

		_sprite = gameObject.GetComponent<SpriteRenderer> ();
		seed.isPressed = (_sprite.sprite == _pressedSprite);

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		Seed seed = (Seed)s;

		if(seed.isPressed)
			_sprite.sprite = _pressedSprite;
		else
			_sprite.sprite = _unpressedSprite;

		if (seed.canBePressed)
			enable ();
		else
			disable ();

	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the door open?
		public bool isPressed;

		public bool canBePressed;
	}
}
