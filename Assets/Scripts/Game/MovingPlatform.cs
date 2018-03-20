using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MoveObject 
{

	//the offest vector between the platform center and the player
	private Vector2 _offset = Vector2.zero;

	private Transform _player = null;

    AudioSource source;
    AudioClip movingPlatform;

    void Start()
    {
        source = this.gameObject.GetComponent<AudioSource>();
        movingPlatform = AudioLibrary.inst.floatingPlatform;
        source.clip = movingPlatform;
        source.Play();
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
			MoveToPoint (getNextPoint());
			_waitTimer = 0;
		}
		setNextPoint ();
		CheckForPlayer ();
	}

	public Transform player
	{
		get { return _player; }
		set { _player = value; }
	}

	public Vector2 offset
	{
		get { return _offset; }
		set { _offset = value; }
	}

	/// <summary>
	/// Moves to point.
	/// </summary>
	/// <param name="point">Point to move to.</param>
	void MoveToPoint(Vector2 point)
	{
		if (!active() || InStasis() || GameManager.isPaused())
			return;
		transform.position = Vector2.MoveTowards (transform.position, getNextPoint(), _moveSpeed * Time.deltaTime);
		if (_player != null) 
		{
			_offset =  _player.position - transform.position;
			_player.position = Vector2.MoveTowards (_player.position, getNextPoint() + _offset, _moveSpeed * Time.deltaTime);
		} 
		else
			_offset = Vector2.zero;
	}

	public void OnDrawGizmos()
	{
		if (_points == null)
			return;
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

		if(_player != null)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine (_player.position, getNextPoint() + _offset);
		}

	}

	public void CheckForPlayer()
	{
		bool seesPlayer = false;
		Collider2D[] colsHit = Physics2D.OverlapBoxAll (transform.position, gameObject.GetComponent<BoxCollider2D> ().size, 0f);
		for(int i = 0; i < colsHit.Length; i++)
		{
			if (colsHit[i].gameObject.GetComponent<Entity> () != null) 
			{
				//Check if 
				Entity entityHit = colsHit[i].gameObject.GetComponent<Entity> ();
				if (entityHit.getFaction () == Entity.Faction.player) 
				{
					if (!entityHit.GetComponent<Player> ().dashing ()) 
					 {
						seesPlayer = true;
						_player = entityHit.transform;
					}
				}
			}
		}
		if (_player != null && !seesPlayer)
			_player = null;
	}
		
}
