using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

/// <summary>
/// An entry point for invokable behaviors used by Controllers.
/// </summary>
public sealed partial class Ability
{
	#region STATIC_VARS

	// The primary repository of all abilities in the game
	private static Dictionary<string, Ability> repo;

	// The latest ID not assigned to an ability
	private static int latestID = 0;
	#endregion

	#region INSTANCE_VARS

	// The unique ID for this ability
	private int _id;
	public int id { get { return _id; } }

	// The displayed name for this ability
	public readonly string name;

	// The displayed description for this ability
	public readonly string desc;

	// The displayed icon associated with this ability
	public readonly Sprite icon;

	// The current cooldown value
	private float _cooldownCurrent;
	public float cooldownCurrent { get { return _cooldownCurrent; } }

	// The maximum possible cooldown value
	public readonly float cooldownMax;

	// The number of uses this ability has before the cooldown check is run
	private int _charges;
	public int charges { get { return _charges; } }

	// The maximum number of charges this ability can accrue
	public readonly int chargesMax;

	// The behavior run upon successful invokation
	private UseEffect effect;

	// Can this ability be invoked?
	public bool available;

	// Is this ability's cooldown being updated>
	public bool active;

	// Data intended to carry over from previous invokations
	private ISerializable persistentData;
	#endregion

	#region STATIC_METHODS

	/// <summary>
	/// Get a copy of an ability from the repository by its name. Returns null if no
	/// ability of the given name could be found.
	/// </summary>
	/// <param name="name">Name.</param>
	public static Ability get(string name)
	{
		Ability a;
		if (name != "" && repo.TryGetValue (name, out a))
			return new Ability (a);
		return null;
	}

	/// <summary>
	/// Place an ability into the global repository.
	/// </summary>
	/// <param name="a">Ability.</param>
	private static void put(Ability a)
	{
		repo.Add (a.name, assignID(a));
	}

	private static Ability assignID(Ability a)
	{
		a._id = latestID++;
		return a;
	}
	#endregion

	#region INSTANCE_METHODS

	public Ability(string name, string desc, Sprite icon, float cooldownMax, int chargesMax, UseEffect effect)
	{
		this._id = -1;
		this.name = name;
		this.desc = desc;
		this.icon = icon;
		this.cooldownMax = cooldownMax;
		_cooldownCurrent = cooldownMax;
		this.chargesMax = chargesMax;
		_charges = 0;

		this.effect = effect;

		active = false;
		available = true;

		persistentData = null;
	}
	public Ability (Ability a) : this (a.name, a.desc, a.icon, a.cooldownMax, a.chargesMax, a.effect) { }

	public bool isReady()
	{
		return (_cooldownCurrent <= 0 || _charges > 0) && active && available;
	}

	public float cooldownPercentage()
	{
		return _cooldownCurrent / cooldownMax;
	}

	public void updateCooldown(float time)
	{
		if (!active)
			return;

		_cooldownCurrent -= time;
		if (_cooldownCurrent <= 0f)
		{
			_cooldownCurrent = 0f;
			if (_charges < chargesMax)
			{
				_charges++;
				if (_charges != chargesMax)
					_cooldownCurrent = cooldownMax;
			}
		}
	}

	public bool use(Entity sub, Vector2 targetPosition)
	{
		if (!isReady ())
			return false;

		if (effect (sub, targetPosition))
		{
			if (_charges > 0)
				_charges--;
			if (_charges < chargesMax || chargesMax == 0)
				_cooldownCurrent = cooldownMax;
			return true;
		}
		return false;
	}
	#endregion

	#region INTERNAL_TYPES
	public delegate bool UseEffect(Entity sub, Vector2 targetPosition);
	#endregion
}
