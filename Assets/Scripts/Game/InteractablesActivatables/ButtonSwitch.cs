using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitch : Interactable 
{
	[Tooltip("Time before the button triggers again (if 0 or infinity, will have no timer.")]
	[SerializeField]
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

	//spriteRenderer for the object
	private SpriteRenderer _sprite;


	//is the player close enough to use the button?
	private bool _playerInRange = false;


	// Use this for initialization
	void Start () 
	{
		_sprite = gameObject.GetComponent<SpriteRenderer> ();
		//TODO: get input button from input module
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
		if(_playerInRange && Input.GetKeyDown(_interactKey) && !GameManager.isPaused())
		{
			onInteract ();
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
		foreach(GameObject activatable in _activatables)
		{
			if(activatable.GetComponent<IActivatable>() != null)
				activatable.GetComponent<IActivatable>().onActivate ();
		}
		_sprite.sprite = _pressedSprite;
		StartCoroutine (spriteTimer (0.4f));
	}

	IEnumerator spriteTimer(float seconds)
	{
		yield return new WaitForSeconds (seconds);
		_sprite.sprite = _unpressedSprite;
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
		_playerInRange = false;
	}
}
