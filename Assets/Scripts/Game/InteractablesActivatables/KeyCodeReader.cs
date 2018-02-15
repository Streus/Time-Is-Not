using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class KeyCodeReader : Interactable
{

	[Tooltip("Interact button(TEMPORARY)")]
	[SerializeField]
	private KeyCode _interactKey = KeyCode.E;

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

	[Tooltip("The ID of the code that can be used here.")]
	[SerializeField]
	private CodeName _codeName;

	[Tooltip("Sprites for the codes.")]
	[SerializeField]
	private Sprite[] codeSprites; 

	[Tooltip("Button prompt sprite")]
	[SerializeField]
	private GameObject _buttonPrompt;

	[Tooltip("Negative button prompt sprite.")]
	[SerializeField]
	private GameObject _negativePrompt;

	//is the player close enough to use the button?
	private bool _playerInRange = false;

	//the spriterenderer for the object
	SpriteRenderer _sprite; 


	// Use this for initialization
	void Start () 
	{
		_sprite = gameObject.GetComponent<SpriteRenderer> ();
		chooseCodeSprite();
		//TODO: get input button from input module
	}

	// Update is called once per frame
	void Update () 
	{
		getInput ();
		#if UNITY_EDITOR
		if(_sprite == null)
			_sprite = gameObject.GetComponent<SpriteRenderer> ();
		if (!Application.isPlaying)
			chooseCodeSprite(); 
		#endif
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
			//Check if the entity is the player
			Entity entityHit = col.gameObject.GetComponent<Entity> ();
			if (entityHit.getFaction () == Entity.Faction.player) 
			{
				_playerInRange = true;
				//show button prompts
				if(isEnabled() && GameManager.HasCode(_codeName))
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

	/// <summary>
	/// Checks the keyboard for input.
	/// </summary>
	void getInput()
	{
		if(_playerInRange && Input.GetKeyDown(_interactKey) && GameManager.HasCode(_codeName) && isEnabled())
		{
			onInteract ();
		}
	}

	/// <summary>
	/// Trigger the Interactable.
	/// </summary>
	public override void onInteract ()
	{
		foreach(GameObject activatable in _activatables)
		{
			if(activatable.GetComponent<IActivatable>() != null)
				activatable.GetComponent<IActivatable>().onActivate ();
		}
	}

	//Select the correct sprite
	public void chooseCodeSprite()
	{
		if (_codeName == CodeName.CODE_1)
			_sprite.sprite = codeSprites[0]; 
		else if (_codeName == CodeName.CODE_2)
			_sprite.sprite = codeSprites[1]; 
		else if (_codeName == CodeName.CODE_3)
			_sprite.sprite = codeSprites[2]; 
		else if (_codeName == CodeName.CODE_4)
			_sprite.sprite = codeSprites[3]; 
		else if (_codeName == CodeName.CODE_5)
			_sprite.sprite = codeSprites[4]; 
		else if (_codeName == CodeName.CODE_6)
			_sprite.sprite = codeSprites[5]; 
		else if (_codeName == CodeName.CODE_7)
			_sprite.sprite = codeSprites[6]; 
		else if (_codeName == CodeName.CODE_8)
			_sprite.sprite = codeSprites[7]; 
	}

}
