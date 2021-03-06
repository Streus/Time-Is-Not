﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interactable
{
	[SerializeField]
	private Vector2 _checkSize = new Vector2(0.3f, 0.3f);

	[SerializeField]
	private Vector2 _offset = new Vector2(0,0);

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

//	[Tooltip("Unpressed state sprite.")]
//	[SerializeField]
//	private Sprite _unpressedSprite;
//
//	[Tooltip("Pressed state sprite.")]
//	[SerializeField]
//	private Sprite _pressedSprite;

	//is the button pressed?
	private bool _pressed = false;

    private bool playSound = true;

	Animator _animationControl;

    AudioSource _source;

    AudioClip pressurePlateOnSound;

    AudioClip pressurePlateOffSound;

    // Use this for initialization
    void Start () 
	{
		_animationControl = GetComponent<Animator> ();

        _source = this.GetComponent<AudioSource>();

        if (_source != null)
        {
            _source.outputAudioMixerGroup = UIManager.inst.mixer.FindMatchingGroups("SFX")[0];
        }

        pressurePlateOnSound = AudioLibrary.inst.pressurePlateOn;
        pressurePlateOffSound = AudioLibrary.inst.pressurePlateOff;

//		if (_pressed)
//			gameObject.GetComponent<SpriteRenderer> ().sprite = _pressedSprite;
//		else
//			gameObject.GetComponent<SpriteRenderer> ().sprite = _unpressedSprite;
		LevelStateManager.inst.stateLoaded += OnLoad;
	}
	
	// Update is called once per frame
	void Update () 
	{
		_animationControl.SetBool ("pressed", _pressed);

		//Check the area for push blocks or a player
		bool checkTest = CircleCheck ();
		//if it does not match the state, update the state and trigger activatables and update sprite
		if(checkTest != _pressed)
		{
			_pressed = checkTest;
			onInteract ();
//			if (_pressed)
//				gameObject.GetComponent<SpriteRenderer> ().sprite = _pressedSprite;
//			else
//				gameObject.GetComponent<SpriteRenderer> ().sprite = _unpressedSprite;
		}

        if(_pressed && playSound)
        {
            if (_source != null)
            {
                _source.clip = pressurePlateOnSound;
                _source.Play();
            }
            playSound = false;
        }
        if(!_pressed && !playSound)
        {
            if (_source != null)
            {
                _source.clip = pressurePlateOffSound;
                _source.Play();
            }
            playSound = true;
        }
	}

	void OnDestroy()
	{
		if(LevelStateManager.inst != null)
			LevelStateManager.inst.stateLoaded -= OnLoad;
	}

	//Draw lines to all linked activatables
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position + (Vector3)_offset, (Vector3)_checkSize);
		Gizmos.color = Color.blue;
		for(int i = 0; i < _activatables.Length; i++)
		{
			if(_activatables[i] != null)
				Gizmos.DrawLine (transform.position, _activatables[i].transform.position);
		}

	}

	void OnLoad(bool success)
	{
		if(success)
			onInteract();
	}

	/// <summary>
	/// Checks the area on top of the pressure plate for players or push blocks
	/// </summary>
	/// <returns><c>true</c>, if somethings is present, <c>false</c> otherwise.</returns>
	bool CircleCheck()
	{
		bool state = false;
		//Collider2D[] colsHit = Physics2D.OverlapCircleAll (transform.position, transform.localScale.x * _checkRadius);
		Collider2D[] colsHit = Physics2D.OverlapBoxAll ((Vector2)transform.position + _offset, _checkSize, 0f);
		foreach(Collider2D col in colsHit)
		{
            if (col.gameObject.GetComponent<Player>() != null)
            {
				if(!col.gameObject.GetComponent<Player>().dashing())
                	state = true;
            }
            else if (col.gameObject.GetComponent<PushBlock>() != null)
            {
                state = true;
            }
		}
		return state;
	}


	/// <summary>
	/// Trigger the Interactable.
	/// </summary>
	public override void onInteract ()
	{
		ToggleSecurityDoors ();
		if (_activatables.Length == 0)
			return;
		foreach(GameObject activatable in _activatables)
		{
			if(activatable != null)
			{
				if (activatable.GetComponent<IActivatable> () != null)
					activatable.GetComponent<IActivatable> ().onActivate (_pressed);
			}
		}
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

		seed.isPressed = _pressed;

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

		Seed seed = (Seed)s;

		if(seed.isPressed)
		{
			onInteract ();
		}
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the plate pressed?
		public bool isPressed;
	}
}
