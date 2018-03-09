using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerControlManager))]
public class PCMInspector : Editor
{
	PlayerControlManager pcm;
	SerializedObject tar;

	public void Enable()
	{
		pcm = (PlayerControlManager)target;
		try
		{
			tar = new SerializedObject(pcm);
		}
		catch(System.NullReferenceException nre)
		{
			Debug.LogException (nre);
		}
	}

	public override void OnInspectorGUI ()
	{
		tar.Update ();

		GUILayout.Label ("Set Options", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal ();
		pcm.setSetCount (EditorGUILayout.IntField (pcm.getSetCount ()));
		GUILayout.EndHorizontal ();

		if (GUI.changed)
			EditorUtility.SetDirty (pcm);
	}
}
