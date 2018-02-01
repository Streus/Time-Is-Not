using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour, ISavable, IStasisable
{
	public enum Direction {Up, Right, Down, Left, None};

	//Shows the direction the box is currently allowed to move.
	private Direction _moveDirection = Direction.None;

	//Check to see if the box is currently moving or not.
	private bool _moving = false;

	//Rigidbody component of the object.
	private Rigidbody2D _rb2d;

	//Check to see if the player is touching the block.
	private bool _playerInRange = false;

	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = false;

	[Tooltip("Push speed.")]
	[SerializeField]
	private float _moveSpeed = 1;

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

	// Use this for initialization
	void Start ()
	{
		//TODO: get input keys from input module
		_rb2d = gameObject.GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update()
	{
		getInput ();
	}

	/// <summary>
	/// Checks the keyboard for movement input.
	/// </summary>
	void getInput()
	{
		if(!_moving && _playerInRange)
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
		if(_moving)
		{
			if (!_playerInRange || !_canMove || GameManager.isPaused ())
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

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.GetComponent<Entity> () != null) 
		{
			//Check if 
			Entity entityHit = col.gameObject.GetComponent<Entity> ();
			if (entityHit.getFaction () == Entity.Faction.player) 
			{
				_playerInRange = true;
				float xDist = col.transform.position.x - transform.position.x;
				float yDist = col.transform.position.y - transform.position.y;

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
		_moving = true;
		switch(dir)
		{
		case Direction.Up:
			_rb2d.velocity = Vector2.up * _moveSpeed;
			break;
		case Direction.Right:
			_rb2d.velocity = Vector2.right * _moveSpeed;
			break;
		case Direction.Down:
			_rb2d.velocity = Vector2.down * _moveSpeed;
			break;
		case Direction.Left:
			_rb2d.velocity = Vector2.left * _moveSpeed;
			break;
		}
		//TODO: put player in push mode and move with block;
	}

	/// <summary>
	/// Stops the box's movement.
	/// </summary>
	void stop()
	{
		_rb2d.velocity = Vector2.zero;
		_moving = false;
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
		Seed seed = new Seed (gameObject);

		return seed;
	}
	public void loadData(SeedBase s)
	{
		if (s == null)
			return;

		if (inStasis)
		{
			return; 
		}

		Seed seed = (Seed)s;

		s.defaultLoad (gameObject);

		_canMove = seed.canMove;

		stop ();
	}
		
	public bool shouldIgnoreReset() { return !inStasis; }

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//can the block move?
		public bool canMove;

		public Seed(GameObject subject) : base(subject) {}

	}


	// --- IStasisable Methods ---
	public void ToggleStasis(bool turnOn)
	{
		inStasis = turnOn; 
	}

	public bool InStasis()
	{
		return inStasis; 
	}
}
