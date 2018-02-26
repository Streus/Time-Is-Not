using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RegisteredObject))]
[InitializeOnLoad]
public class ROInspector : Editor
{
	public RegisteredObject ro;
	public SerializedObject so;

	public void OnEnable()
	{
		ro = (RegisteredObject)target;
		try
		{
			so = new SerializedObject(ro);
		}
		#pragma warning disable 0168
		catch(System.NullReferenceException nre) { }
		#pragma warning restore 0168
	}

	public override void OnInspectorGUI ()
	{
		so.Update ();

		string id = ro.rID == default(string) ? "(ID will be generated in Playmode)" : ro.rID;
		GUILayout.Label (id, EditorStyles.largeLabel);

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Stasisable");
		Undo.RecordObject (ro, "Toggle Stasisable");
		ro.setStasisable ((bool)EditorGUILayout.Toggle (ro.getStasisable ()));
		GUILayout.EndHorizontal ();

		if (GUI.changed)
			EditorUtility.SetDirty (ro);
	}
}
