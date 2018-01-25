using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

public class RegisteredObject : MonoBehaviour
{
	/* Static Vars */
	private static List<RegisteredObject> directory;
	static RegisteredObject()
	{ 
		directory = new List<RegisteredObject>();
	}

	/* Instance Vars */
	[SerializeField]
	private string registeredID;
	public string rID
	{
		get { return registeredID; }
	}

	// TODO- test if this works
	//public GameObject recreatePrefab; 

	// Path to a prefab to which this RO is attached
	private string prefabPath = "";

	/* Static Methods */
	public static RegisteredObject[] getObjects()
	{
		return directory.ToArray ();
	}

	public static RegisteredObject findObject(string ID)
	{
		return directory.Find (delegate(RegisteredObject obj) {
			return obj.registeredID == ID;
		});
	}

	// For first-time spawning of prefabs that should be tracked by the SSM
	public static GameObject create(string prefabPath, Vector3 position, Quaternion rotation)
	{
		return create (prefabPath, position, rotation, null);
	}
	public static GameObject create(string prefabPath, Vector3 position, Quaternion rotation, Transform parent)
	{
		GameObject go = Resources.Load<GameObject> ("Prefabs/" + prefabPath);
		GameObject inst;
		if(parent == null)
			inst = Instantiate (go, position, rotation);
		else
			inst = Instantiate (go, position, rotation, parent);
		RegisteredObject ro = inst.GetComponent<RegisteredObject> ();
		ro.Reset ();
		ro.prefabPath = prefabPath;
		return inst;
	}

	// For respawning a prefab in the sow cycle
	public static GameObject recreate(string prefabPath, string registeredID, string parentID)
	{
		GameObject go = Resources.Load<GameObject> ("Prefabs/" + prefabPath);
		GameObject inst;
		if (parentID == "")
			inst = Instantiate (go, Vector3.zero, Quaternion.identity);
		else
		{
			RegisteredObject parent = RegisteredObject.findObject (parentID);
			if (parent == null)
				throw new ArgumentException ("[RO] " + parentID + " does not exist!");
			inst = Instantiate (go, Vector3.zero, Quaternion.identity, parent.transform);
		}
		RegisteredObject ro = inst.GetComponent<RegisteredObject> ();
		ro.registeredID = registeredID;
		ro.prefabPath = prefabPath;
		return inst;


		// TODO- clean this up, if it's used 
		/*
		GameObject inst; 
		if (parentID == "")
			inst = Instantiate (recreatePrefab, Vector3.zero, Quaternion.identity);
		else
		{
			RegisteredObject parent = RegisteredObject.findObject (parentID);
			if (parent == null)
				throw new ArgumentException ("[RO] " + parentID + " does not exist!");
			inst = Instantiate (recreatePrefab, Vector3.zero, Quaternion.identity, parent.transform);
		}

		RegisteredObject ro = inst.GetComponent<RegisteredObject> ();
		ro.registeredID = registeredID;
		ro.prefabPath = prefabPath;
		return inst;
		*/ 
	}

	// Special wrapper method for the generic Monobehaviour#Destroy
	// Manages saving the destruction state of a RO
	/*
	public static void destroy(GameObject go)
	{
		RegisteredObject ro = go.GetComponent<RegisteredObject> ();
		if (ro != null)
			ro.saveDestruction ();
		Destroy (go);
	}
	*/ 

	/* Instance Methods */
	public void Reset()
	{
		registeredID = Convert.ToBase64String (Guid.NewGuid ().ToByteArray ()).TrimEnd('=');
	}

	public void Awake()
	{
		directory.Add (this);
	}

	public void OnDestroy()
	{
		directory.Remove (this);
	}

	// Get the savable script attached to this GO and return its seed
	public SeedBase reap()
	{
		ISavable blade = GetComponent<ISavable> ();
		if (blade == null)
			throw new ReapException (ToString() + " has no values to reap");
		//Debug.Log(ToString () + " reaped values.", Console.LogTag.info);

		SeedBase seed = blade.saveData();

		//pass in a prefabPath so that if this RO is a prefab, it can be spawned again later
		seed.prefabPath = prefabPath;
		if (prefabPath != "")
			seed.registeredID = registeredID;

		//pass in this RO's parent object, ifex
		Transform parent = gameObject.transform.parent;
		if (parent != null)
		{
			RegisteredObject parentRO = parent.GetComponent<RegisteredObject> ();
			if (parentRO != null)
				seed.parentID = parentRO.registeredID;
			//else
				//Debug.Log(ToString () + " is under a non-RO. Make its parent an RO to save its data properly.", Console.LogTag.error);
		}
		else
			seed.parentID = "";

		return seed;
	}

	// Take a seed and pass it along to the savable script attached to this GO
	public void sow(SeedBase seed)
	{
		ISavable hole = GetComponent<ISavable> ();
		if (hole == null)
			throw new ReapException (ToString() + " has nowhere to sow values");
		//Debug.Log(ToString () + " sowed values.", Console.LogTag.info);

		//intercept and save prefabPath
		prefabPath = seed.prefabPath;

		//destroy object if it saved a destroyed state
		if (seed.destroyed)
			Destroy (gameObject);
		else
			hole.loadData (seed);
	}

	// Tells the SSM that a RO has been destroyed through gameplay
	/*
	private void saveDestruction()
	{
		if (prefabPath != "")
			return;

		SeedBase seed = reap ();
		seed.destroyed = true;

		LevelStateManager.inst.store (rID, seed);
	}
	*/ 

	public override string ToString ()
	{
		return "[RO] ID: " + registeredID;
	}
}

public class ReapException : ApplicationException
{
	public ReapException(string message) : base(message) { }
}
