using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RegisteredObject))]
public class ROInspector : Editor
{
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
		if(!EditorApplication.isPlayingOrWillChangePlaymode)
			ro.generateID ();
	}

	public override void OnInspectorGUI ()
	{
		so.Update ();

		GUILayout.Label (ro.rID, EditorStyles.largeLabel);

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Stasisable");
		ro.setStasisable ((bool)EditorGUILayout.Toggle (ro.getStasisable ()));
		GUILayout.EndHorizontal ();

		if (GUI.changed)
			EditorUtility.SetDirty (ro);
	}
}
