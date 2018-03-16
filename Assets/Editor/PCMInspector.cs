using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerControlManager))]
public class PCMInspector : Editor
{
	PlayerControlManager pcm;
	SerializedObject tar;

	public void OnEnable()
	{
		pcm = (PlayerControlManager)target;
		try
		{
			tar = new SerializedObject(pcm);
		}
		catch(System.NullReferenceException nre)
		{
			Debug.LogError (nre.Message + "\nCould not make serialized object of " + pcm.gameObject);
		}
	}

	public override void OnInspectorGUI ()
	{
		tar.Update ();

		GUILayout.Label ("Set Options", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Number of Sets", EditorStyles.label);
		pcm.setSetCount (EditorGUILayout.IntField (pcm.getSetCount ()));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Current Set", EditorStyles.label);
		pcm.setCurrentSet (EditorGUILayout.IntField (pcm.getCurrentSet ()));
		GUILayout.EndHorizontal ();

		GUILayout.Label ("Sets", EditorStyles.boldLabel);

		for (int s = 0; s < pcm.getSetCount (); s++)
		{
			EditorGUILayout.BeginFadeGroup (1f);
			pcm.setSetName(EditorGUILayout.TextField(pcm.getSetName (s), EditorStyles.textField), s);
			for (int k = 0; k < PlayerControlManager.ciLength; k++)
			{
				GUILayout.BeginHorizontal ();
				GUILayout.Label (System.Enum.GetName (typeof(ControlInput), k), EditorStyles.label);
				int index = (s * PlayerControlManager.ciLength) + k;
				pcm.setBinding ((KeyCode)EditorGUILayout.EnumPopup ((KeyCode)pcm.getBinding(index)), index);
				GUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndFadeGroup ();
			GUILayout.Space (15f);
		}

		if (GUI.changed)
			EditorUtility.SetDirty (pcm);
	}
}
