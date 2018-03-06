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

	private Dictionary<Direction, bool> canMoveInDirection;

	[Tooltip("Push speed Multiplier.")]
	[SerializeField]
	private float _speedMult = 1;

	[Tooltip("Controls if the box can be pushed or not.")]
	[SerializeField]
	private bool _canMove = true;

	[Header("Controls (TEMPORARY)")]
	[SerializeField]
	private KeyCode _upKey = KeyCode.W;
	[SerializeField]
	private KeyCode _rightKey = KeyCode.D;
	[SerializeField]
	private KeyCode _downKey = KeyCode.S;
	[SerializeField]
	private KeyCode _leftKey = KeyCode.A;

    AudioSource source;

    AudioClip pushBlockSound;

	public bool isInArea;


	private BoxCollider2D _col;
	// Use this for initialization
	void Start ()
	{
		//TODO: get input keys from input module
		_rb2d = gameObject.GetComponent<Rigidbody2D> ();

		GetComponent<RegisteredObject> ().allowResetChanged += ToggleStasis;

        source = this.GetComponent<AudioSource>();
        pushBlockSound = AudioLibrary.inst.pushBlockMoving;
        source.outputAudioMixerGroup = UIManager.inst.mixer.FindMatchingGroups("SFX")[0];

        _col = gameObject.GetComponent<BoxCollider2D> ();
	}

	public void OnDestroy()
	{
		GetComponent<RegisteredObject> ().allowResetChanged -= ToggleStasis;
	}

	// Update is called once per frame
	void Update()
	{
		getInput ();

		if(_beingPushed)
		{
			_rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
			switch (_moveDirection) 
			{
			case Direction.Up:
				transform.Translate (Vector2.up * _moveSpeed * Time.deltaTime);
				_player.transform.Translate (Vector2.up * _moveSpeed * Time.deltaTime);
                    break;
			case Direction.Right:
				transform.Translate (Vector2.right * _moveSpeed * Time.deltaTime);
				_player.transform.Translate (Vector2.right * _moveSpeed * Time.deltaTime);
				break;
			case Direction.Down:
				transform.Translate (Vector2.down * _moveSpeed * Time.deltaTime);
				_player.transform.Translate (Vector2.down * _moveSpeed * Time.deltaTime);
				break;
			case Direction.Left:
				transform.Translate (Vector2.left * _moveSpeed * Time.deltaTime);
				_player.transform.Translate (Vector2.left * _moveSpeed * Time.deltaTime);
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
		if(!_beingPushed && _playerInRange)
		{
			//Move in direction if key is pressed
			switch(_moveDirection)
			{
			case Direction.Up:
				if (Input.GetKey (_upKey))
					move (_moveDirection);
                    break;
			case Direction.Right:
				if (Input.GetKey (_rightKey))
					move (_moveDirection);
				break;
			case Direction.Down:
				if (Input.GetKey (_downKey))
					move (_moveDirection);
				break;
			case Direction.Left:
				if (Input.GetKey (_leftKey))
					move (_moveDirection);
				break;
			}
		}
		if(_beingPushed)
		{
			if (!_playerInRange || !_canMove || GameManager.isPaused () || inStasis || CheckPath())
				stop ();
			//stop moving when key is released
			switch(_moveDirection)
			{
			case Direction.Up:
				if (!Input.GetKey (_upKey))
					stop();
				break;
			case Direction.Right:
				if (!Input.GetKey (_rightKey))
					stop();
				break;
			case Direction.Down:
				if (!Input.GetKey (_downKey))
					stop();
				break;
			case Direction.Left:
				if (!Input.GetKey (_leftKey))
					stop();
				break;
			}
		}

	}

	void OnDrawGizmos()
	{
	/*	if (_col == null)
			_col = gameObject.GetComponent<BoxCollider2D> ();
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

		moveDir *= 0.1f;
		Gizmos.DrawCube ((Vector2)transform.position + moveDir + _col.offset, _col.size);
		*/

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

	/// <summary>
	/// Moves the box in the given direction.
	/// </summary>
	void move(Direction dir)
	{
		if (!_player.pushing ())
			_player.enterPushState ();
		_beingPushed = true;
        if (source != null)
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
        if (source != null)
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
		if (inStasis)
			sprite.color = Color.yellow;
		else
			sprite.color = Color.white;
	}

	public bool InStasis()
	{
		return inStasis; 
	}
}
