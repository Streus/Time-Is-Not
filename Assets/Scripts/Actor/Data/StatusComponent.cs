using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to build Statuses.
/// </summary>
public class StatusComponent
{
	#region STATIC_VARS

	#endregion

	#region INSTANCE_VARS

	public int stacks;

	protected Status parent;
	#endregion

	#region STATIC_METHODS

	#endregion

	#region INSTANCE_METHODS

	public StatusComponent()
	{
		stacks = 1;
		parent = null;
	}
	public StatusComponent(int stacks)
	{
		this.stacks = stacks;
		parent = null;
	}
	public StatusComponent(StatusComponent other) : this(other.stacks) { }

	public StatusComponent setParent(Status s)
	{
		parent = s;
		return this;
	}

	public virtual void onApply (Entity subject) { }
	public virtual void onRevert(Entity subject) { }
	public virtual void onUpdate(Entity subject) { }
	public virtual void onDeath(Entity  subject) { }
	#endregion
}
