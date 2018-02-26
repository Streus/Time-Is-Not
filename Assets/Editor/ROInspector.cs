using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RegisteredObject))]
[InitializeOnLoad]
public class ROInspector : Editor
{
	static ROInspector()
	{
		// Tell all ROs to attempt regeneration
		// This is to account for instance IDs being inconsistent between loads of the Unity Editor.
		Object[] ros = Resources.FindObjectsOfTypeAll (typeof(RegisteredObject));
		Debug.Log ("Regenerating " + ros.Length + " ROIDs.");
		for (int i = 0; i < ros.Length; i++)
		{
			((RegisteredObject)ros [i]).generateID ();
			EditorUtility.SetDirty (ros [i]);
		}
		Debug.Log ("Done regenerating ROIDs");
	}

	RegisteredObject ro;
	SerializedObject so;

	public void OnEnable()
	{
		try
		{
			so = new SerializedObject(ro);
		}
		#pragma warning disable 0168
		catch(System.NullReferenceException nre) { }
		#pragma warning restore 0168
	}

	public void Awake()
	{
		ro = (RegisteredObject)target;
		if (!EditorApplication.isPlayingOrWillChangePlaymode)
		{
			Undo.RecordObject (ro, "Generate ID");
			ro.generateID ();
			EditorUtility.SetDirty (ro);
		}
	}

	public override void OnInspectorGUI ()
	{
		so.Update ();

		GUILayout.Label (ro.rID, EditorStyles.largeLabel);

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Stasisable");
		Undo.RecordObject (ro, "Toggle Stasisable");
		ro.setStasisable ((bool)EditorGUILayout.Toggle (ro.getStasisable ()));
		GUILayout.EndHorizontal ();

		if (GUI.changed)
			EditorUtility.SetDirty (ro);
	}
}
