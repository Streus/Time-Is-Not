using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MoveObject 
{

	//the offest vector between the platform center and the player
	public Vector2 _offset = Vector2.zero;

	private Transform _player = null;

	// Update is called once per frame
	void Update () 
	{
		MoveToPoint (getNextPoint());
		setNextPoint ();
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

		if(_player != null)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine (_player.position, getNextPoint() + _offset);
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
				//col.gameObject.layer = LayerMask.NameToLayer ("MPPassenger");
				_player = entityHit.transform;
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
				//col.gameObject.layer = LayerMask.NameToLayer ("GroundEnts");
				_player = null;
			}
		}
	}
}
