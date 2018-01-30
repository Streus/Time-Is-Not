using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour, IActivatable
{

	enum EndStyle {LoopToStart, BackAndForth, Stop, TeleportToStart};

	[Tooltip("Path object will follow")]
	[SerializeField]
	private Vector2[] _points;

	[Tooltip("Is the object moving?")]
	[SerializeField]
	private bool _active = true;

	[Tooltip("How fast the object moves.")]
	[SerializeField]
	private float _moveSpeed = 1;

	[Tooltip("What does the object do when it reaches the last point?")]
	[SerializeField]
	private EndStyle _endBehavior = EndStyle.TeleportToStart;

	//if true, the object will move backwards through the points
	private bool reverseMovement = false;

	//The current target Point
	private int _nextPoint = 0;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveToPoint (_points[_nextPoint]);
		GetNextPoint ();
	}

	/// <summary>
	/// Moves to point.
	/// </summary>
	/// <param name="point">Point to move to.</param>
	void MoveToPoint(Vector2 point)
	{
		if (!_active)
			return;
		transform.position = Vector2.MoveTowards (transform.position, _points [_nextPoint], _moveSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Gets the next point.
	/// </summary>
	void GetNextPoint()
	{
		if(Vector2.Distance(transform.position, _points[_nextPoint]) < 0.1f)
		{
			//get next point
			if(_nextPoint == _points.Length -1 || (reverseMovement && _nextPoint == 0))
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
					if (reverseMovement)
						_nextPoint = _points.Length - 1;
					else
						_nextPoint = 0;
					break;
				case EndStyle.TeleportToStart:
					//teleport to start (or end if reversing)
					if(reverseMovement)
						_nextPoint = _points.Length - 1;
					else 
						_nextPoint = 0;
					transform.position = _points [_nextPoint];
					break;
				case EndStyle.BackAndForth:
					//toggle the direction and set next point
					reverseMovement = !reverseMovement;
					if (reverseMovement)
						_nextPoint--;
					else
						_nextPoint++;
					break;
				}
			}
			else
			{
				//incriment the target point
				if (reverseMovement)
					_nextPoint--;
				else
					_nextPoint++;
			}
		}
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
		_active = state;
		return _active;
	}

}
