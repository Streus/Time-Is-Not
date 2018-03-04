using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A repository class used by Controllers to manage and update Abilities, Statuses,
/// and other data.
/// </summary>
public sealed class Entity : MonoBehaviour, ISavable
{
	#region STATIC_VARS

	#endregion

	#region INSTANCE_VARS

	[Tooltip("Used to distinguish players from enemies")]
	[SerializeField]
	private Faction faction = Faction.neutral;

	[Tooltip("How fast this Entity can move.")]
	[SerializeField]
	private float movespeed = 4f;

	private List<Ability> abilities;
	private List<Status> statuses;

	public int abilityCount { get { return abilities.Count; } }
	public int abilityCap { get { return abilities.Capacity; } }

	public event AbilityChanged abilityAdded, abilityRemoved;
	public event AbilitySwap abilitySwapped;
	public event StatusChanged statusAdded, statusRemoved;
	public event EntityGeneric died;
	#endregion

	#region STATIC_METHODS

	#endregion

	#region INSTANCE_METHODS

	public void Awake()
	{
		abilities = new List<Ability> (2);
		statuses = new List<Status> ();
	}

	public void Update()
	{
		if (GameManager.inst != null && GameManager.isPaused ())
			return;

		int i;

		//update all statuses
		for (i = 0; i < statuses.Count; i++)
		{
			if (statuses [i].updateDuration (this, Time.deltaTime))
			{
				//if a status ended and was removed from the list, then backtrack
				//to ensure a status is not skipped
				i--;
			}
		}

		if (abilities.Count > 0)
		{
			//update abilities
			for (i = 0; i < abilities.Capacity; i++)
			{
				//Debug.Log (gameObject.name + "|" + i); //DEBUG
				if (abilities [i] != null)
					abilities [i].updateCooldown (Time.deltaTime);
			}
		}
	}
		
	#region GETTERS_SETTERS
	public Faction getFaction()
	{
		return faction;
	}
		
	public float getMovespeed()
	{
		return movespeed;
	}

	public void setMovespeed(float val)
	{
		movespeed = val;
	}

	public void modMovespeed(float mult)
	{
		movespeed *= mult;
	}
	#endregion

	#region ABILITY_MANAGEMENT

	public void addAbility(Ability a, int index = -1)
	{
		if (a == null)
			return;

		if (index >= 0 && index < abilities.Capacity)
		{
			if (abilities [index] != null && abilityRemoved != null)
				abilityRemoved (abilities [index], index);
			abilities [index] = a;
		}
		else
			abilities.Add (a);

		a.active = true;

		if (abilityAdded != null)
			abilityAdded (a, index);
	}

	/// <summary>
	/// Removes the given ability from the ability list. Returns false if the removal
	/// was unsuccessful.
	/// </summary>
	public bool removeAbility(Ability a)
	{
		for (int i = 0; i < abilities.Capacity; i++)
		{
			if (abilities [i].id == a.id)
			{
				removeAbility (i);
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Removes the given ability from the ability list. Returns false if the removal
	/// was unsuccessful.
	/// </summary>
	public bool removeAbility(int index)
	{
		if (index < 0 || index >= abilities.Capacity)
			return false;

		if (abilities [index] == null)
			return false;

		Ability removed = abilities [index];
		abilities [index] = null;
		removed.active = false;

		if (abilityRemoved != null)
			abilityRemoved (removed, index);

		return true;
	}

	/// <summary>
	/// Swap an ability in the list for another. Returns the old ability.
	/// </summary>
	public Ability swapAbility(Ability a, int index)
	{
		Ability old = null;
		try
		{
			old = abilities[index];
			abilities[index] = a;
			if(a != null)
				a.active = true;
			if(old != null)
				old.active = false;
		}
		catch(System.IndexOutOfRangeException ioore)
		{
			Debug.LogError ("Encountered an error swapping abilities on " + gameObject.name
				+ ".\n" + ioore.Message);
			return null;
		}

		if (abilitySwapped != null)
			abilitySwapped (a, old, index);

		return old;
	}

	public Ability getAbility(int index)
	{
		if (index >= 0 && index < abilities.Capacity)
			return abilities [index];
		return null;
	}

	/// <summary>
	/// Returns an ability in this entity's list with the name provided.
	/// </summary>
	public Ability getAbility(string name)
	{
		for (int i = 0; i < abilities.Capacity; i++)
		{
			if (abilities [i] == null)
				continue;

			if (abilities [i].name == name)
				return abilities [i];
		}

		return null;
	}
	#endregion

	#region STATUS_MANAGEMENT
	public void addStatus(Status s)
	{
		Status existing = statuses.Find (delegate(Status obj)
		{
			return obj.Equals (s);
		});

		if (existing != null)
		{
			//a status of the given type is already applied to this entity
			existing.stack (this, 1);
			return;
		}

		//add this new status to this entity
		statuses.Add (s);
		s.durationCompleted += removeStatus;
		s.onApply (this);

		//notify listeners
		if (statusAdded != null)
			statusAdded (s);
	}

	public void removeStatus(Status s)
	{
		s.onRevert (this);
		s.durationCompleted -= removeStatus;
		statuses.Remove (s);

		//notify listeners
		if (statusRemoved != null)
			statusRemoved (s);
	}

	public void removeStatus(string name)
	{
		Status s = Status.get (name, float.PositiveInfinity);
		if (s == null)
			return;
		
		Status existing = statuses.Find (delegate(Status obj)
		{
			return obj.Equals (s);
		});

		removeStatus (existing);
	}
	#endregion

	#region ISAVABLE_METHODS

	// These are mostly for show.  I'm pretty positive they don't actually do anything
	//in the current way they're implemented
	public SeedBase saveData()
	{
		Seed s = new Seed (this);
		return s;
	}

	public void loadData(SeedBase seed)
	{
		Seed s = (Seed)seed;
		movespeed = s.movespeed;
		abilities = s.abilities;
		statuses = s.statuses;
	}
	#endregion

	public void onDeath()
	{
		//invoke statuses
		for (int i = 0; i < statuses.Count; i++)
			statuses [i].onDeath (this);

		if (died != null)
			died ();
	}

	#endregion

	#region INTERNAL_TYPES

	private class Seed : SeedBase
	{
		public float movespeed;
		public List<Ability> abilities;
		public List<Status> statuses;

		public Seed(Entity e)
		{
			movespeed = e.movespeed;
			abilities = e.abilities;
			statuses = e.statuses;
		}
	}

	/// <summary>
	/// Used to distinguish between different groups of Actors in gameplay
	/// </summary>
	public enum Faction { player, enemy, neutral }

	public delegate void EntityGeneric();
	public delegate void AbilityChanged(Ability a, int index = -1);
	public delegate void AbilitySwap (Ability a, Ability old, int index);
	public delegate void StatusChanged(Status s); 
	#endregion
}
