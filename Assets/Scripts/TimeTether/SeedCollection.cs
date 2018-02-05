using System;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollection
{
	#region INSTANCE_VARS

	// The destroyed state of the subject
	public bool destroyed;

	// Values for filling a transform
	public Vector3 tPosition;
	public Quaternion tRotation;

	// Values for filling a Rigidbody2D
	public Vector2 rbPosition;
	public float rbRotation;
	public Vector2 rbVelocity;
	public float rbAngVelocity;

	// Path for a prefab that this seed should spawn
	public string prefabPath;

	// In the case of prefab saving, the Registered ID is saved here
	public string registeredID;

	// In the case of saving a prefab that is a child GO, the parent's rID is saved here
	public string parentID;

	private Dictionary<Type, SeedBase> seeds;

	public int size { get { return seeds.Count; } }
	#endregion

	#region INSTANCE_METHODS

	public SeedCollection(GameObject subject, params ISavable[] scripts)
	{
		destroyed = false;

		tPosition = subject.transform.position;
		tRotation = subject.transform.rotation;
		Rigidbody2D rb2d = subject.GetComponent<Rigidbody2D> ();
		if(rb2d != null)
		{
			rbPosition = rb2d.position;
			rbRotation = rb2d.rotation;
			rbVelocity = rb2d.velocity;
			rbAngVelocity = rb2d.angularVelocity;
		}

		prefabPath = "";
		registeredID = "";
		parentID = "";

		seeds = new Dictionary<Type, SeedBase> ();
		for (int i = 0; i < scripts.Length; i++)
		{
			Type t = scripts [i].GetType ();
			seeds.Add (t, scripts [i].saveData ());
		}
	}

	public void loadSeeds(GameObject subject, params ISavable[] holes)
	{
		defaultLoad (subject);

		SeedBase seed;
		for (int i = 0; i < holes.Length; i++)
		{
			if (seeds.TryGetValue (holes [i].GetType (), out seed))
			{
				holes [i].loadData (seed);
			}
			else
				Debug.LogError ("Script mismatch! " + holes [i].GetType ().FullName +
				" is on the GameObject, but not in the collection!");
		}
	}

	private void defaultLoad(GameObject subject)
	{
		if (destroyed)
		{
			//Entity is destroyed
			MonoBehaviour.Destroy (subject);
			return;
		}

		//-SeedBase values-
		subject.transform.position = tPosition;
		subject.transform.rotation = tRotation;

		Rigidbody2D body = subject.GetComponent<Rigidbody2D> ();
		if (body != null)
		{
			body.position = rbPosition;
			body.rotation = rbRotation;
			body.velocity = rbVelocity;
			body.angularVelocity = rbAngVelocity;
		}
	}
	#endregion
}

