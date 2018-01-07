using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applied to an Entity to change its default behavior
/// </summary>
public class Status
{
	#region STATIC_VARS

	// A repository for commonly used statuses
	private static Dictionary<string, Status> repo;

	// The latest ID not assigned to an status
	private static int latestID = 0;
	#endregion

	#region INSTANCE_VARS

	// The unique ID for this status
	private int _id;
	public int id { get { return _id; } }

	// The displayed name of this status
	public readonly string name;

	// The base description of this status
	public readonly string desc;

	// The sprite displayed for this status
	public readonly Sprite icon;

	// The type of decay behavior this status has
	public readonly DecayType decayType;

	// The current number of applied stacks
	private int _stacks;
	public int stacks { get { return _stacks; } }

	// The maximum number of stacks this status can accrue
	public readonly int stacksMax;

	// The remaining time this status will be applied before going through decay
	private float _duration;
	public float duration { get { return _duration; } }

	// The inital duration of this status when it was applied
	private float initDuration;

	// The individual components that make up this status
	private StatusComponent[] components;
	#endregion

	#region STATIC_METHODS

	public static Status get(string name, float newDur = float.NaN)
	{
		Status s;
		if (repo.TryGetValue (name, out s))
		{
			if (newDur != float.NaN)
				return new Status (s, newDur);
			return new Status (s);
		}
		return null;
	}

	private static void put(Status s)
	{
		repo.Add (s.name, assignID (s));
	}

	private static Status assignID(Status s)
	{
		s._id = latestID++;
		return s;
	}
	#endregion

	#region INSTANCE_METHODS

	public Status(string name, string desc, Sprite icon, DecayType dt, int stacksMax, float duration, params StatusComponent[] components)
	{
		this.name = name;
		this.desc = desc;
		this.icon = icon;
		this.decayType = dt;
		initDuration = duration;
		_duration = initDuration;

		this.stacksMax = stacksMax;
		_stacks = 1;

		this.components = components;
	}
	public Status(Status s) : this(s.name, s.desc, s.icon, s.decayType, s.stacksMax, s.initDuration, s.components) { }
	public Status(Status s, float duration) : this(s)
	{
		initDuration = duration;
		_duration = initDuration;
	}

	public float durationPercentage()
	{
		return _duration / initDuration;
	}

	#endregion

	#region INTERNAL_TYPES

	/// <summary>
	/// The types of status decay behavior
	/// </summary>
	public enum DecayType { communal, serial }
	#endregion
}
