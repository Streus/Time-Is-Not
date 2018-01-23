using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Atlas))]
public class AtlasInspector : Editor
{
	private const int MAX_RESOLUTION = 100;

	Atlas map;
	SerializedObject tar;

	public void OnEnable()
	{
		map = (Atlas)target;
		try
		{
			tar = new SerializedObject(map);
		}
		#pragma warning disable 0168
		catch(System.NullReferenceException nre) { }
		#pragma warning restore 0168
	}

	public override void OnInspectorGUI ()
	{
		tar.Update ();

		GUILayout.Label ("Grid Options", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Maximum Point");
		map.setMaxpoint((GameObject)EditorGUILayout.ObjectField (map.getMaxpoint (), typeof(GameObject), true));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Resolution");
		map.setResolution ((int)EditorGUILayout.IntSlider (map.getResolution (), 1, MAX_RESOLUTION));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Bind to Width");
		map.setBindToWidth ((bool)EditorGUILayout.Toggle (map.getBindToWidth ()));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Layer Mask");
		List<string> layers = new List<string> ();
		for (int i = 0; i < 32; i++)
		{
			string s = LayerMask.LayerToName (i);
			if (s.Length > 0)
				layers.Add (s);
			else
				break;
		}
		map.setMask ((LayerMask)EditorGUILayout.MaskField(map.getMask().value, layers.ToArray()));
		GUILayout.EndHorizontal ();

		GUILayout.Label ("Graph Options", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Diagonal Routes");
		map.setDiagonalRoutes((bool)EditorGUILayout.Toggle (map.getDiagonalRoutes()));
		GUILayout.EndHorizontal ();

		if (GUILayout.Button ("Generate"))
			map.generate ();
		if (GUILayout.Button ("Clear"))
			map.clear ();

		GUILayout.Label ("Other Options", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Show Grid");
		map.gizmoGrid = (bool)EditorGUILayout.Toggle (map.gizmoGrid);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Show Graph");
		map.gizmoNodes = (bool)EditorGUILayout.Toggle (map.gizmoNodes && map.gizmoGrid);
		GUILayout.EndHorizontal ();

		if (GUI.changed)
			EditorUtility.SetDirty (map);
	}
}
