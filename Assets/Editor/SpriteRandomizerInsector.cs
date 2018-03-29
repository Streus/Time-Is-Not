using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteRandomizer))]
public class SpriteRandomizerInsector : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		SpriteRandomizer randomizer = (SpriteRandomizer)target;
		if(GUILayout.Button("Randomize Sprites"))
		{
			randomizer.RandomizeAll ();
		}
	}
}
