using UnityEngine;
using System.Runtime.Serialization;
using System;

public class SeedBase
{
	#region INSTANCE_VARS

	// Whether the subject will allow itself to be reset by the SSM
	public bool ignoreReset;
	#endregion

	#region INSTANCE_METHODS

	// Create a new Seed that will pull transform and rigidbody info when serialized
	public SeedBase(ISavable subject)
	{
		ignoreReset = subject.shouldIgnoreReset ();
	}
	#endregion
}