using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour, ISavable
{
	public enum Direction {Up, Right, Down, Left, None};

	//Shows the direction the box is currently allowed to move.
	private Direction _moveDirection = Direction.None;

	//Check to see if the box is currently being pushed or not.
	public bool _beingPushed = false;

	//Rigidbody component of the object.
	private Rigidbody2D _rb2d;

	//Check to see if the player is touching the block.
	private bool _playerInRange = false;

	private Player _player;

	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = false;

	//move speed
	private float _moveSpeed = 2; 

	[SerializeField]
	private float _playerCheckRange = 0.2f;

	private Dictionary<Direction, bool> canMoveInDirection;

	[Tooltip("Push speed Multiplier.")]
	[SerializeField]
	private float _speedMult = 1;

	[Tooltip("Controls if the box can be pushed or not.")]
	[SerializeField]
	private bool _canMove = true;

    AudioSource source;

    AudioClip pushBlockSound;

	public bool isInArea;

    [SerializeField] bool isHermitCrab;

	[SerializeField]
	float postStasisDelay = 0.25f;

	private float _stasisTimer;

	private BoxCollider2D _col;

	public GameObject _stasisEffect;
	// Use this for initialization
	void Start ()
	{
		//TODO: get input keys from input module
		_rb2d = gameObject.GetComponent<Rigidbody2D> ();
		_col = gameObject.GetComponent<BoxCollider2D> ();

		GetComponent<RegisteredObject> ().allowResetChanged += ToggleStasis;
        if (!isHermitCrab)
        {
            source = this.GetComponent<AudioSource>();
            pushBlockSound = AudioLibrary.inst.pushBlockMoving;
            if(source != null)
            {
                source.outputAudioMixerGroup = UIManager.inst.mixer.FindMatchingGroups("SFX")[0];
            }
        }
		_stasisTimer = 0;
	}

	public void OnDestroy()
	{
		GetComponent<RegisteredObject> ().allowResetChanged -= ToggleStasis;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (GameManager.CameraIsZoomedOut ()) 
		{
			if (_beingPushed)
				stop ();
			return;
		}
		BoxCheck ();
		getInput ();

		//timer to control delay after stasis
		if (_stasisTimer > 0)
			_stasisTimer -= Time.deltaTime;
		else
			_stasisTimer = 0;

		if(_beingPushed)
		{
			_rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
			switch (_moveDirection) 
			{
			case Direction.Up:
				transform.Translate (Vector2.up * _moveSpeed * Time.deltaTime);
				_player.transform.Translate (Vector2.up * _moveSpeed * Time.deltaTime);
				_player.GetComponent<Animator> ().SetInteger ("Direction", 2);
                    break;
			case Direction.Right:
				transform.Translate (Vector2.right * _moveSpeed * Time.deltaTime);
				_player.transform.Translate (Vector2.right * _moveSpeed * Time.deltaTime);
				_player.GetComponent<Animator> ().SetInteger ("Direction", 1);
				break;
			case Direction.Down:
				transform.Translate (Vector2.down * _moveSpeed * Time.deltaTime);
				_player.transform.Translate (Vector2.down * _moveSpeed * Time.deltaTime);
				_player.GetComponent<Animator> ().SetInteger ("Direction", 4);
				break;
			case Direction.Left:
				transform.Translate (Vector2.left * _moveSpeed * Time.deltaTime);
				_player.transform.Translate (Vector2.left * _moveSpeed * Time.deltaTime);
				_player.GetComponent<Animator> ().SetInteger ("Direction", 3);
				break;
			}
		}
		else
		{
			_rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer> ();
		if(rend != null)

			rend.sortingOrder = SpriteOrderer.inst.OrderMe (transform);
	}

	/// <summary>
	/// Checks the keyboard for movement input.
	/// </summary>
	void getInput()
	{
		if(!_beingPushed && _playerInRange && !GameManager.isPaused() && _stasisTimer == 0)
		{
			//Move in direction if key is pressed
			switch(_moveDirection)
			{
			case Direction.Up:
				if (PlayerControlManager.GetKey(ControlInput.UP))
					move (_moveDirection);
                    break;
			case Direction.Right:
				if (PlayerControlManager.GetKey(ControlInput.RIGHT))
					move (_moveDirection);
				break;
			case Direction.Down:
				if (PlayerControlManager.GetKey(ControlInput.DOWN))
					move (_moveDirection);
				break;
			case Direction.Left:
				if (PlayerControlManager.GetKey(ControlInput.LEFT))
					move (_moveDirection);
				break;
			}
		}
		if(_beingPushed)
		{
			if (!_playerInRange || !_canMove || GameManager.isPaused () || inStasis || CheckPath() || _stasisTimer > 0)
				stop ();
			//stop moving when key is released
			switch(_moveDirection)
			{
			case Direction.Up:
				if (!PlayerControlManager.GetKey(ControlInput.UP))
					stop();
				break;
			case Direction.Right:
				if (!PlayerControlManager.GetKey(ControlInput.RIGHT))
					stop();
				break;
			case Direction.Down:
				if (!PlayerControlManager.GetKey(ControlInput.DOWN))
					stop();
				break;
			case Direction.Left:
				if (!PlayerControlManager.GetKey(ControlInput.LEFT))
					stop();
				break;
			}
		}

	}

	void OnDrawGizmos()
	{
		if (_col == null)
			_col = gameObject.GetComponent<BoxCollider2D> ();
		Gizmos.DrawCube((Vector2)transform.position + _col.offset + ((Vector2)transform.up * _col.size.y / 2) + ((Vector2)transform.up * _playerCheckRange / 2),new Vector2(_col.size.x / 1.25f, _playerCheckRange));

		//right check
		Gizmos.DrawCube((Vector2)transform.position + _col.offset + ((Vector2)transform.right * _col.size.x / 2) + ((Vector2)transform.right * _playerCheckRange / 2),new Vector2(_playerCheckRange, _col.size.y / 1.25f));

		//bottom check
		Gizmos.DrawCube((Vector2)transform.position + _col.offset + ((Vector2)transform.up * _col.size.y / -2) + ((Vector2)transform.up * _playerCheckRange / -2),new Vector2(_col.size.x / 1.25f, _playerCheckRange));

		//left check
		Gizmos.DrawCube((Vector2)transform.position + _col.offset + ((Vector2)transform.right * _col.size.x / -2) + ((Vector2)transform.right * _playerCheckRange / -2),new Vector2(_playerCheckRange, _col.size.y / 1.25f));

	}

	bool CheckPath()
	{
		float dist = 0f;
		Vector2 moveDir = Vector2.zero;
		switch(_moveDirection)
		{
		case Direction.Up:
			moveDir = Vector2.up * _col.size.y;
			break;
		case Direction.Right:
			moveDir = Vector2.right * _col.size.x;
			break;
		case Direction.Down:
			moveDir = Vector2.down * _col.size.y;
			break;
		case Direction.Left:
			moveDir = Vector2.left * _col.size.x;
			break;
		}
		RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)transform.position + _col.offset, _col.size, 0, moveDir, 0.1f, 1 << LayerMask.NameToLayer("PushBlockArea") | 1 << LayerMask.NameToLayer("Wall"));
		//GetComponent<Collider2D>().Cast(moveDir, hits, dist, true);

		bool seesArea = false;

		for(int i = 0; i < hits.Length; i++)
		{
			if ((hits [i].collider.gameObject.layer == LayerMask.NameToLayer ("Wall") && !hits [i].collider.isTrigger) || hits [i].collider.gameObject.layer == LayerMask.NameToLayer ("PushBlockArea"))
				seesArea = true;
		}

		return seesArea;

	}
	/*
	 * THIS IS FOR THE OLD MOVEMENT
	 * WE ARE CHANING IT TO BE BOXCHECK BASED
	 * UNCOMMENT THIS AND COMMENT THE CASTCHECK METHOD TO REVERT TO OLD MOVEMENT
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.GetComponent<Entity> () != null) 
		{
			//Check if 
			Entity entityHit = col.gameObject.GetComponent<Entity> ();
			if (entityHit.getFaction () == Entity.Faction.player) 
			{
				if (_player == null)
					_player = entityHit.GetComponent<Player> ();
				_moveSpeed = (entityHit.getMovespeed () * _speedMult);
				_playerInRange = true;
				float xDist = (col.transform.position.x + col.offset.x) - (transform.position.x + _col.offset.x);
				float yDist = (col.transform.position.y + col.offset.y) - (transform.position.y + _col.offset.y);

				//Calculate which direction to move
				if(Mathf.Abs(xDist) > Mathf.Abs(yDist))
				{
					//Move horizontal
					if (xDist > 0)
						_moveDirection = Direction.Left;
					else
						_moveDirection = Direction.Right;
				}
				else
				{
					//move Vertical
					if (yDist > 0)
						_moveDirection = Direction.Down;
					else
						_moveDirection = Direction.Up;
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
				_moveDirection = Direction.None;
			}
		}
	}
	*/

	void BoxCheck()
	{
		_moveSpeed = 0;
		_moveDirection = Direction.None;
		bool seesup = false;
		bool seesright = false;
		bool seesdown = false;
		bool seesleft = false;
		//top check
		Collider2D[] colsTop = Physics2D.OverlapBoxAll((Vector2)transform.position + _col.offset + ((Vector2)transform.up * _col.size.y / 2) + ((Vector2)transform.up * _playerCheckRange / 2),new Vector2(_col.size.x / 1.25f, _playerCheckRange), 0);
		for(int i = 0; i < colsTop.Length; i++)
		{
			if (colsTop [i].CompareTag ("Player")) 
			{
				if (_player == null)
					_player = colsTop [i].GetComponent<Player> ();
				_moveSpeed = (3 * _speedMult);
				Debug.Log ("Sees Player");
				seesup = true;
				_moveDirection = Direction.Down;
			}
		}
		//right check
		Collider2D[] colsRight = Physics2D.OverlapBoxAll((Vector2)transform.position + _col.offset + ((Vector2)transform.right * _col.size.x / 2) + ((Vector2)transform.right * _playerCheckRange / 2),new Vector2(_playerCheckRange, _col.size.y / 1.25f), 0);
		for(int i = 0; i < colsRight.Length; i++)
		{
			if (colsRight [i].CompareTag ("Player")) 
			{
				if (_player == null)
					_player = colsRight [i].GetComponent<Player> ();
				_moveSpeed = (3 * _speedMult);
				Debug.Log ("Sees Player");
				seesright = true;
				_moveDirection = Direction.Left;
			}
		}
		//bottom check
		Collider2D[] colsBottom = Physics2D.OverlapBoxAll((Vector2)transform.position + _col.offset + ((Vector2)transform.up * _col.size.y / -2) + ((Vector2)transform.up * _playerCheckRange / -2),new Vector2(_col.size.x / 1.25f, _playerCheckRange), 0);
		for(int i = 0; i < colsBottom.Length; i++)
		{
			if (colsBottom [i].CompareTag ("Player")) 
			{
				if (_player == null)
					_player = colsBottom [i].GetComponent<Player> ();
				_moveSpeed = (3 * _speedMult);
				Debug.Log ("Sees Player");
				seesdown = true;
				_moveDirection = Direction.Up;
			}
		}
		//left check
		Collider2D[] colsLeft = Physics2D.OverlapBoxAll((Vector2)transform.position + _col.offset + ((Vector2)transform.right * _col.size.x / -2) + ((Vector2)transform.right * _playerCheckRange / -2),new Vector2(_playerCheckRange, _col.size.y / 1.25f), 0);
		for(int i = 0; i < colsLeft.Length; i++)
		{
			if (colsLeft [i].CompareTag ("Player")) 
			{
				if (_player == null)
					_player = colsLeft [i].GetComponent<Player> ();
				_moveSpeed = (3 * _speedMult);
				Debug.Log ("Sees Player");
				seesleft = true;
				_moveDirection = Direction.Right;
			}
		}
		_playerInRange = (seesup || seesleft || seesright || seesdown);
	}

	/// <summary>
	/// Moves the box in the given direction.
	/// </summary>
	void move(Direction dir)
	{
		if (!_player.pushing ())
			_player.enterPushState ();
		_beingPushed = true;
        if (source != null && !isHermitCrab)
        {
            source.clip = pushBlockSound;
            source.Play();
        }
        //TODO: put player in push mode and move with block;
    }

	/// <summary>
	/// Stops the box's movement.
	/// </summary>
	void stop()
	{
		if (_player.pushing ())
			_player.exitPushState ();
		_rb2d.velocity = Vector2.zero;
		_beingPushed = false;
        if (source != null && !isHermitCrab)
        {
            source.Stop();
        }
		//TODO: put player out of push mode and move on its own;
	}

	/// <summary>
	/// Disables movement.
	/// </summary>
	public void disableMovement()
	{
		_canMove = false;
	}

	/// <summary>
	/// Enables movement.
	/// </summary>
	public void enableMovement()
	{
		_canMove = true;
	}

	/// <summary>
	/// Returns if the box can move or not.
	/// </summary>
	public bool canMove()
	{
		return _canMove;
	}

	// --- ISavable Methods ---
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

		seed.canMove = _canMove;

		return seed;
	}
	public void loadData(SeedBase s)
	{
		if (s == null || inStasis)
			return;

		Seed seed = (Seed)s;

		_canMove = seed.canMove;

		_rb2d.velocity = Vector2.zero;

		_beingPushed = false;

		_rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//can the block move?
		public bool canMove;
	}


	// --- IStasisable Methods ---
	private void ToggleStasis(bool turnOn)
	{
		inStasis = turnOn; 
		SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer> ();
		if (sprite == null)
			return;
		if (!inStasis)
			_stasisTimer = postStasisDelay;
		if(_stasisEffect != null) 
		{
			_stasisEffect.SetActive (inStasis);
			_stasisEffect.GetComponent<SpriteRenderer> ().sortingOrder = gameObject.GetComponent<SpriteRenderer> ().sortingOrder + 1;
		}
		else
		{
			if (inStasis) 
				sprite.color = Color.yellow;
			else
				sprite.color = Color.white;
		}
	}

	public bool InStasis()
	{
		return inStasis; 
	}
}
