using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteOrderer))]
public class SpriteOrdererEditor : Editor 
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		SpriteOrderer orderer = (SpriteOrderer)target;
		if(GUILayout.Button("Order Sprites"))
		{
			orderer.OrderAll ();
		}
	}
}
