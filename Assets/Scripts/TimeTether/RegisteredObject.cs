using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class RegisteredObject : MonoBehaviour
{
	#region STATIC_VARS
	private static List<RegisteredObject> directory;
	static RegisteredObject()
	{ 
		directory = new List<RegisteredObject>();
	}

	private const string DEFAULT_RID = "";
	#endregion

	#region INSTANCE_VARS
	[SerializeField]
	private string registeredID;
	public string rID
	{
		get { return registeredID; }
	}

	// Path to a prefab to which this RO is attached
	private string prefabPath = "";

	[Tooltip("Can this object be stasis'd?")]
	[SerializeField]
	private bool stasisable = true;

	// <= 0 == false; > 0 == true
	private int allowReset = 1;

	public delegate void SetBoolean(bool val);
	public event SetBoolean allowResetChanged;
	#endregion

	#region STATIC_METHODS
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
	}
	#endregion

	#region INSTANCE_METHODS
	public void Reset()
	{
		generateID ();
	}

	private void generateID()
	{
		if(registeredID == DEFAULT_RID)
			registeredID = Convert.ToBase64String (Guid.NewGuid ().ToByteArray ()).TrimEnd('=');
	}

	public void Awake()
	{
		#if UNITY_EDITOR
		if(!EditorApplication.isPlayingOrWillChangePlaymode)
		{
			Undo.RecordObject(this, "Generate ID");
			generateID();
			EditorUtility.SetDirty(this);
		}
		#endif
		directory.Add (this);
	}

	public void OnDestroy()
	{
		#if UNITY_EDITOR
		if (!isQuitting && Application.isPlaying && prefabPath == "")
		{
			Debug.LogError("The RegisteredObject " + gameObject.name + " is being destroyed but does not have a prefab path. " +
				"You shouldn't destroy RegisteredObjects that were directly placed in the Editor and not through a spawner"); 
		}
		#endif

		directory.Remove (this);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		_OnColliderEnter2D(col);
	}
	public void _OnColliderEnter2D(Collider2D col)
	{
		if (col.CompareTag("StasisField"))
		{
			Debug.Log("stasis'd!");
			if (stasisable)
				setAllowReset(false);
		}
	}
	public void OnTriggerExit2D(Collider2D col)
	{
		_OnColliderExit2D(col);
	}
	public void _OnColliderExit2D(Collider2D col)
	{
		if (col.CompareTag("StasisField"))
		{
			Debug.Log("unstasis'd!");
			if (stasisable)
				setAllowReset(true);
		}
	}

	#if UNITY_EDITOR
	bool isQuitting = false; 
	void OnApplicationQuit()
	{ 
		isQuitting = true; 
	}
	#endif

	// Get the savable script attached to this GO and return its seed
	public SeedCollection reap()
	{
		ISavable[] blades = GetComponents<ISavable> ();

		SeedCollection collection = new SeedCollection (gameObject, blades);
		collection.prefabPath = prefabPath;
		collection.registeredID = registeredID;

		//pass in this RO's parent object, ifex
		Transform parent = gameObject.transform.parent;
		if (parent != null)
		{
			RegisteredObject parentRO = parent.GetComponent<RegisteredObject> ();
			if (parentRO != null)
				collection.parentID = parentRO.registeredID;
		}
		else
			collection.parentID = "";

		return collection;
	}

	// Take a seed and pass it along to the savable script attached to this GO
	public void sow(SeedCollection collection)
	{
		if (allowReset <= 0)
			return;

		ISavable[] holes = GetComponents<ISavable> ();

		//intercept and save prefabPath
		prefabPath = collection.prefabPath;

		if(collection.size > 0)
			collection.loadSeeds (gameObject, holes);
	}

	public void setAllowReset(bool val)
	{
		if (val)
			allowReset++;
		else
			allowReset--;

//		if (allowReset < 0)
//			allowReset = 0;

		if (allowResetChanged != null)
			allowResetChanged (!val);
	}

	public bool getAllowReset()
	{
		return allowReset > 0;
	}

	public override string ToString ()
	{
		return "[RO] ID: " + registeredID;
	}
	#endregion
}
