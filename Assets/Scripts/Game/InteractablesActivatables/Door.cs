﻿using System.Collections;
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
 	bool _isOpen = false;

	[Tooltip("Is the door openable by hand or controled via interactable")]
	[SerializeField]
	private DoorTypes _type = DoorTypes.Electronic;

	//the door's sprite
	private SpriteRenderer _sprite;

	[Tooltip("Check area for the door.")]
	[SerializeField]
	private Vector2 _checkSize;

	[Tooltip("Layers that the door should check for the player.")]
	[SerializeField]
	private LayerMask _playerLayers;

	//Shows if the player is close enough to open the door
	private bool _playerInRange = false;

	//is the object's default state inverted?
	private bool isInverted = false;

	//collider of the door
	private BoxCollider2D _collider;

	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = false;

	public enum DoorTypes {Manual, Electronic};

	private Animator _anim;

    AudioSource source;

    AudioClip openFieldDoor;
    AudioClip closeFieldDoor;
    AudioClip openMetalDoor;
    AudioClip closeMetalDoor;

	[SerializeField]
	private bool ignoresTimeTether = false;

	public GameObject _stasisEffectClosed;

	public GameObject _stasisEffectOpen;

    // Use this for initialization
    void Start () 
	{
		if (!Application.isPlaying)
			return;
		_sprite = gameObject.GetComponent<SpriteRenderer> ();
		_collider = gameObject.GetComponent<BoxCollider2D> ();
		_anim = gameObject.GetComponent <Animator> ();
		_playerInRange = false;
		isInverted = _isOpen;

		GetComponent<RegisteredObject> ().allowResetChanged += ToggleStasis;

        source = this.GetComponent<AudioSource>();
        openFieldDoor = AudioLibrary.inst.doorFieldOpen;
        closeFieldDoor = AudioLibrary.inst.doorFieldClosed;
        openMetalDoor = AudioLibrary.inst.doorMetalOpen;
        closeMetalDoor = AudioLibrary.inst.doorMetalClose;
        if (source != null)
        {
            source.outputAudioMixerGroup = UIManager.inst.mixer.FindMatchingGroups("SFX")[0];
        }
    }

	// Update is called once per frame
	void Update () 
	{
		if(_collider == null)
			_collider = gameObject.GetComponent<BoxCollider2D> ();
		if(_sprite == null)
			_sprite = gameObject.GetComponent<SpriteRenderer> ();
		if(_anim == null)
			_anim = gameObject.GetComponent <Animator> ();
		_anim.SetBool ("isOpen", _isOpen);
		_anim.SetBool ("isStasised", inStasis);
		_playerInRange = CheckForPlayer();
		if((_type == DoorTypes.Manual) && isEnabled())
		{
			if (_isOpen != _playerInRange)
				onInteract ();
		}

	}

	void OnDrawGizmos()
	{
		float xdist = _checkSize.x / 2;
		float ydist = _checkSize.y / 2;
		if (_collider == null)
			return;
		Vector2 center = (transform.position + (Vector3)(_collider.offset));
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (center + new Vector2(-xdist, ydist), center + new Vector2(xdist, ydist));
		Gizmos.DrawLine (center + new Vector2(xdist, -ydist), center + new Vector2(xdist, ydist));
		Gizmos.DrawLine (center + new Vector2(-xdist, -ydist), center + new Vector2(xdist, -ydist));
		Gizmos.DrawLine (center + new Vector2(-xdist, ydist), center + new Vector2(-xdist, -ydist));
	}

	bool CheckForPlayer()
	{
		if (!Application.isPlaying)
			return false;
		Collider2D[] colsHit = Physics2D.OverlapBoxAll ((new Vector2 (transform.position.x, transform.position.y) + _collider.offset), _checkSize, 0.0f, _playerLayers);
		bool seesPlayer = false;
		for(int i = 0; i < colsHit.Length; i++)
		{
			Entity entityHit = colsHit[i].gameObject.GetComponent<Entity> ();
			if (entityHit != null && entityHit.getFaction () == Entity.Faction.player) 
			{
				seesPlayer = true;
			}
		}
		return seesPlayer;
	}

	public void OnDestroy()
	{
		GetComponent<RegisteredObject> ().allowResetChanged -= ToggleStasis;
	}

	public bool isOpen()
	{
		return _isOpen;
	}

	public override void onInteract ()
	{
		if (_type != DoorTypes.Manual)
			return;
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
		if (_type == DoorTypes.Manual) {
			Debug.Log("NOTE: This door cannot be opened remotely, please change it to an electronic door.");
			return _isOpen;
		}
		//if the door is inverted, a true state closes the door
		if(isInverted)
		{
            if (state)
            {
                Close();
            }
            else
            {
                Open();
            }
		}
		else
		{
            if (state)
            {
                Open();
            }
            else
            {
                Close();
            }
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

    public void PlayFieldOpenSound()
    {
        if (source != null)
        {
            source.clip = openFieldDoor;
            source.Play();
        }
    }

    public void PlayFieldCloseSound()
    {
        if (source != null)
        {
            source.clip = closeFieldDoor;
            source.Play();
        }
    }

    public void PlayMetalOpenSound()
    {
        if (source != null)
        {
            source.clip = openMetalDoor;
            source.Play();
        }
    }

    public void PlayMetalCloseSound()
    {
        if (source != null)
        {
            source.clip = closeMetalDoor;
            source.Play();
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

		seed.isOpen = _isOpen;

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		Seed seed = (Seed)s;

		if (ignoresTimeTether)
			return;

		if (seed.isOpen) {
			Open ();
		}
		else {
			Close ();
		}
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
		Debug.Log ("Stasis set to " + turnOn);
		inStasis = turnOn;
		SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer> ();

		if(inStasis)
		{
			_stasisEffectOpen.GetComponent<SpriteRenderer> ().sortingOrder = gameObject.GetComponent<SpriteRenderer> ().sortingOrder + 1;
			_stasisEffectClosed.GetComponent<SpriteRenderer> ().sortingOrder = gameObject.GetComponent<SpriteRenderer> ().sortingOrder + 1;
			if(_isOpen)
				_stasisEffectOpen.SetActive (true);
			else
				_stasisEffectClosed.SetActive (true);
		}
		else
		{
			_stasisEffectClosed.SetActive (false);
			_stasisEffectOpen.SetActive (false);
		}
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
