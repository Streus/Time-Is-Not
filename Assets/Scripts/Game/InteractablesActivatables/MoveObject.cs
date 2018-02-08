using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour, IActivatable, ISavable, IStasisable
{
	//TODO: draw gizmos along path

	public enum EndStyle {LoopToStart, BackAndForth, Stop, TeleportToStart};

	[Tooltip("Path object will follow")]
	public Vector2[] _points;

	[Tooltip("Is the object moving?")]
	[SerializeField]
	private bool _active = true;

	[Tooltip("How fast the object moves.")]
	public float _moveSpeed = 1;

	[Tooltip("What does the object do when it reaches the last point?")]
	public EndStyle _endBehavior = EndStyle.TeleportToStart;

	//if true, the object will move backwards through the points
	private bool _reverseMovement = false;

	//The current target Point
	private int _nextPoint = 0;

	//is the object's default state inverted?
	private bool isInverted = false;

	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = false;


	// Use this for initialization
	void Start () 
	{
		if (!Application.isPlaying)
			return;
		isInverted = !_active;
	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveToPoint (_points[_nextPoint]);
		setNextPoint ();
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

	}

	/// <summary>
	/// Moves to point.
	/// </summary>
	/// <param name="point">Point to move to.</param>
	void MoveToPoint(Vector2 point)
	{
		if (!_active || inStasis || GameManager.isPaused())
			return;
		transform.position = Vector2.MoveTowards (transform.position, _points [_nextPoint], _moveSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Gets the next point.
	/// </summary>
	public void setNextPoint()
	{
		if(Vector2.Distance(transform.position, _points[_nextPoint]) < 0.1f)
		{
			//get next point
			if(_nextPoint == _points.Length -1 || (_reverseMovement && _nextPoint == 0))
			{
				//the platform is at the last point
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
		Seed seed = new Seed (gameObject);

		seed.isOn = _active;

		seed.isReversed = _reverseMovement;

		seed.nextPoint = _nextPoint;

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

		s.defaultLoad (gameObject);

		_active = seed.isOn;

		_reverseMovement = seed.isReversed;

		_nextPoint = seed.nextPoint;
	}

	/// <summary>
	/// Checks if the object should be able to be reset.
	/// </summary>
	/// <returns><c>true</c>, if it should ignore it, <c>false</c> otherwise.</returns>
	public bool shouldIgnoreReset() 
	{ 
		return !inStasis; 
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

		public Seed(GameObject subject) : base(subject) {}

	}


	//****Stasisable Object Functions****

	/// <summary>
	/// Toggles if the object is in stasis.
	/// </summary>
	/// <param name="turnOn">If set to <c>true</c> turn on.</param>
	public void ToggleStasis(bool turnOn)
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
