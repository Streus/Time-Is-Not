using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueObject 
{
	[SerializeField]
	private string _dialogue;

	[SerializeField]
	private Vector2 _location;

	private DialogueObject _next;

	private GameObject _uiObject;

	private float _timer;

	[SerializeField]
	private GameObject _followTarget;

	[SerializeField]
	private float _boxLifetime;

	public DialogueObject(string dialogue, Vector2 location, float lifeTime, DialogueObject next)
	{
		_dialogue = dialogue;
		_location = location;
		_next = next;
		_boxLifetime = lifeTime;
		_timer = lifeTime;
		_followTarget = null;

	}

	public DialogueObject(string dialogue, Vector2 location, float lifeTime)
	{
		_dialogue = dialogue;
		_location = location;
		_next = null;
		_boxLifetime = lifeTime;
		_timer = lifeTime;
		_followTarget = null;
	}

	public DialogueObject(DialogueObject original)
	{
		_dialogue = original.Dialogue;
		_location = original.Location;
		_next = original.Next;
		_timer = original.Timer;
		_boxLifetime = original.Lifetime;
		_followTarget = original.FollowTarget;
	}

	public string Dialogue
	{
		get{return _dialogue;}
		set{_dialogue = value;}
	}

	public Vector2 Location
	{
		get{return _location;}
		set{_location = value;}
	}

	public DialogueObject Next
	{
		get{return _next;}
		set{_next = value;}
	}

	public GameObject UIObject
	{
		get{return _uiObject;}
		set{_uiObject = value;}
	}

	public GameObject FollowTarget
	{
		get{return _followTarget;}
		set{_followTarget = value;}
	}

	public float Timer
	{
		get{return _timer;}
		set
		{
			if (value < 0)
				_timer = 0;
			else
				_timer = value;
		}
	}

	public float Lifetime
	{
		get{return _boxLifetime;}
		set
		{
			if (value < 0)
				_boxLifetime = 0;
			else
				_boxLifetime = value;
		}
	}
}
