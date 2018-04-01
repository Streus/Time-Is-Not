using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MoveObject : MonoBehaviour, IActivatable, ISavable
{
	//TODO: draw gizmos along path

	public enum EndStyle {LoopToStart, BackAndForth, Stop, TeleportToStart};


	[Tooltip("(READ ONLY)World Space Coordinates of the object")]
	public Vector2 _objectWorldSpacePositionREADONLY;

	[Tooltip("Path object will follow")]
	public Vector2[] _points;

	[Tooltip("Is the object moving?")]
	[SerializeField]
	private bool _active = true;

	[Tooltip("How fast the object moves.")]
	public float _moveSpeed = 1;

	[Tooltip("What does the object do when it reaches the last point?")]
	public EndStyle _endBehavior = EndStyle.TeleportToStart;

	[Tooltip("How long the platform will wait at all waypoints")]
	public float delayPerPoint = 0;

	[Tooltip("How long the platform will wait at ending waypoints (if delayPerPoint is higher, that will be used instead)")]
	public float delayAtEnds = 0;

	//if true, the object will move backwards through the points
	public bool _reverseMovement = false;

	//The current target Point
	private int _nextPoint = 0;

	//is the object's default state inverted?
	private bool isInverted = false;

	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = false;

	private bool stuckOnBlock = false;


	public float _waitTimer = 0;


	public Vector2 _movementVector;


	// Use this for initialization
	void Start () 
	{
		_waitTimer = 0;
		if (!Application.isPlaying)
			return;
		isInverted = !_active;
		GetComponent<RegisteredObject> ().allowResetChanged += ToggleStasis;
	}

	public void OnDestroy()
	{
		GetComponent<RegisteredObject> ().allowResetChanged -= ToggleStasis;
	}
	
	// Update is called once per frame
	void Update () 
	{
		_objectWorldSpacePositionREADONLY = (Vector2)transform.position;
		if (!Application.isPlaying)
			return;
		if (_waitTimer > 0)
			_waitTimer -= Time.deltaTime;
		else
		{
			MoveToPoint (_points[_nextPoint]);
			_waitTimer = 0;
		}
		setNextPoint ();
		Vector2 movementVector = (_points [_nextPoint] - (Vector2)transform.position).normalized;
		_movementVector = movementVector;
		CheckForBlocks ();
	}

	void OnDrawGizmos()
	{
		if (_points.Length == 0)
			return;
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (transform.position, _points[0]);
		for(int i = 0; i < _points.Length - 1; i++)
		{
			Gizmos.DrawLine (_points[i], _points[i+1]);
		}
		if(_endBehavior == EndStyle.LoopToStart)
			Gizmos.DrawLine (_points[ _points.Length - 1], _points[0]);

		BoxCollider2D col = gameObject.GetComponent<BoxCollider2D> ();
		Vector2 adjustment;

		Vector2 truePosition = transform.position;

		if (col != null)
		{
			adjustment = col.size;
			truePosition += col.offset;
		}


		else
			adjustment = new Vector2 (1, 1);
		//Gizmos.DrawCube (transform.position, (Vector3)transform.localScale + (new Vector3 (1, 1, 1) * (-0.02f)));
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine ((Vector3)truePosition + new Vector3 (-1 * (transform.localScale.x * adjustment.x / 2 + (-0.02f)),transform.localScale.y * adjustment.y / 2 + (-0.02f), 0), (Vector3)truePosition + new Vector3 (transform.localScale.x * adjustment.x / 2 + (-0.02f), transform.localScale.y * adjustment.y / 2 + (-0.02f), 0));
		Gizmos.DrawLine ((Vector3)truePosition + new Vector3 (transform.localScale.x * adjustment.x / 2 + (-0.02f), transform.localScale.y * adjustment.y / 2 + (-0.02f), 0), (Vector3)truePosition + new Vector3 (transform.localScale.x * adjustment.x / 2 + (-0.02f), -1 * (transform.localScale.y * adjustment.y / 2 + (-0.02f)), 0));
		Gizmos.DrawLine ((Vector3)truePosition + new Vector3 (transform.localScale.x * adjustment.x / 2 + (-0.02f), -1 * (transform.localScale.y * adjustment.y / 2 + (-0.02f)), 0), (Vector3)truePosition + new Vector3 (-1 * (transform.localScale.x * adjustment.x / 2 + (-0.02f)), -1 * (transform.localScale.y * adjustment.y / 2 + (-0.02f)), 0));
		Gizmos.DrawLine ((Vector3)truePosition + new Vector3 (-1 * (transform.localScale.x * adjustment.x / 2 + (-0.02f)), -1 * (transform.localScale.y * adjustment.y / 2 + (-0.02f)), 0), (Vector3)truePosition + new Vector3 (-1 * (transform.localScale.x * adjustment.x / 2 + (-0.02f)), transform.localScale.y * adjustment.y / 2 + (-0.02f), 0));

	}

	bool CheckForBlocks()
	{
		bool blockedByBlock = false;
		BoxCollider2D col = gameObject.GetComponent<BoxCollider2D> ();
		if((gameObject.layer == LayerMask.NameToLayer("Wall") || gameObject.layer == LayerMask.NameToLayer("HalfWall")) && col != null)
		{
			Collider2D[] colsHit = Physics2D.OverlapBoxAll (transform.position, (new Vector2(col.transform.localScale.x * col.size.x, col.transform.localScale.y * col.size.y)) + new Vector2 ((-0.02f), (-0.02f)), 0f);
			for(int i = 0; i < colsHit.Length; i++)
			{
				PushBlock block = colsHit [i].GetComponent<PushBlock> ();
				if(block != null)
				{
					Vector2 blockDir = ((Vector2)block.transform.position - (Vector2)transform.position).normalized;
					if((blockDir.x >= 0.2 && _movementVector.x >= 0.1)|| (blockDir.x <= -0.2 && _movementVector.x <= -0.1) || (blockDir.y >= 0.2 && _movementVector.y >= 0.1) || (blockDir.y <= -0.2 && _movementVector.y <= -0.1))
					{
						if (block.InStasis ()) 
						{
							//block is in stasis, freeze the moving object.
							blockedByBlock = true;
						} else 
						{
							Vector2 blockOffset = (Vector2)(block.transform.position - transform.position);
							if (_active && !inStasis && !GameManager.isPaused())
								block.transform.position = Vector2.MoveTowards (block.transform.position, _points [_nextPoint] + blockOffset, _moveSpeed * Time.deltaTime);
						}
					}
					
				}
			}
		}
		stuckOnBlock = blockedByBlock;
		return false;
	}

	/// <summary>
	/// Moves to point.
	/// </summary>
	/// <param name="point">Point to move to.</param>
	void MoveToPoint(Vector2 point)
	{
		if (!_active || inStasis || GameManager.isPaused() || stuckOnBlock)
			return;
		transform.position = Vector2.MoveTowards (transform.position, _points [_nextPoint], _moveSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Gets the next point.
	/// </summary>
	public void setNextPoint()
	{
		if (_nextPoint >= _points.Length || _nextPoint < 0)
			return;
		if(Vector2.Distance(transform.position, _points[_nextPoint]) < 0.01f)
		{
			//get next point
			if(_nextPoint == _points.Length -1 || (_reverseMovement && _nextPoint == 0))
			{
				//the platform is at the last point
				if(_endBehavior != EndStyle.LoopToStart)
				{
					if (delayPerPoint > delayAtEnds)
						_waitTimer = delayPerPoint;
					else
						_waitTimer = delayAtEnds;
				}
				switch(_endBehavior)
				{
				case EndStyle.Stop:
					//stop the object
					_active = false;
					break;
				case EndStyle.LoopToStart:
					//set next point to the start (or end if reversing)
					if (_reverseMovement)
						_nextPoint = _points.Length - 1;
					else
						_nextPoint = 0;
					break;
				case EndStyle.TeleportToStart:
					//teleport to start (or end if reversing)
					if(_reverseMovement)
						_nextPoint = _points.Length - 1;
					else 
						_nextPoint = 0;
					transform.position = _points [_nextPoint];
					break;
				case EndStyle.BackAndForth:
					//toggle the direction and set next point
					_reverseMovement = !_reverseMovement;
					if (_reverseMovement)
						_nextPoint--;
					else
						_nextPoint++;
					break;
				}
			}
			else
			{
				if((_endBehavior == EndStyle.LoopToStart && _nextPoint == 0))
				{
					if (delayPerPoint > delayAtEnds)
						_waitTimer = delayPerPoint;
					else
						_waitTimer = delayAtEnds;
				}
				else
					_waitTimer = delayPerPoint;
				//incriment the target point
				if (_reverseMovement)
					_nextPoint--;
				else
					_nextPoint++;
			}
		}
	}

	public Vector2 getNextPoint()
	{
		if (_nextPoint >= _points.Length || _nextPoint < 0)
			return (Vector2)transform.position;
		return _points[_nextPoint];
	}

	/// <summary>
	/// Toggles the Object's state and returns it.
	/// </summary>
	public bool onActivate()
	{
		_active = !_active;
		return _active;
	}

	/// <summary>
	/// Sets the Object's state and returns it.
	/// </summary>
	public bool onActivate(bool state)
	{
		//if inverted: 'true' state enables movement, otherwise it disables it
		if (isInverted)
			_active = state;
		else
			_active = !state;
		return _active;
	}

	public bool active()
	{
		return _active;
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

		seed.isOn = _active;

		seed.isReversed = _reverseMovement;

		seed.nextPoint = _nextPoint;

		seed.waitingTime = _waitTimer;

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		Seed seed = (Seed)s;

		_active = seed.isOn;

		_reverseMovement = seed.isReversed;

		_nextPoint = seed.nextPoint;

		_waitTimer = seed.waitingTime;
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the object moving?
		public bool isOn;

		//is it moving backward?
		public bool isReversed;

		//which point should it move toward?
		public int nextPoint;

		public float waitingTime;
	}


	//****Stasisable Object Functions****

	/// <summary>
	/// Toggles if the object is in stasis.
	/// </summary>
	/// <param name="turnOn">If set to <c>true</c> turn on.</param>
	private void ToggleStasis(bool turnOn)
	{
		inStasis = turnOn;

		SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer> ();
		if (sprite == null)
			return;
		if(inStasis)
			sprite.color = Color.yellow;
		else
			sprite.color = Color.white;
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
