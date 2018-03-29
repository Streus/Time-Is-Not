using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallExtruder))]
public class WallExtruderInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		WallExtruder extruder = (WallExtruder)target;
		if(GUILayout.Button("Extrude Walls"))
		{
			extruder.ExtrudeAll();
		}
	}
	 
}
