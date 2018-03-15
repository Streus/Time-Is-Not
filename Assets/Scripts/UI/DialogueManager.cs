using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager> 
{
	[SerializeField]
	private GameObject _dialogueBoxPrefab;

	private List<DialogueObject> _activeDialogues;

	// Use this for initialization
	void Start () 
	{
		_activeDialogues = new List<DialogueObject>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateTimers ();
	}

	public void CreateBox(DialogueObject dialogue)
	{
		DialogueObject dia = new DialogueObject (dialogue);
		GameObject obj = Instantiate (_dialogueBoxPrefab, transform);
		obj.transform.localPosition = (Vector3)dia.Location;
		obj.GetComponentInChildren<Text> ().text = dia.Dialogue;
		dia.UIObject = obj;
		_activeDialogues.Add (dia);

	}

	void UpdateTimers()
	{
		if (_activeDialogues.Count == 0)
			return;
		foreach(DialogueObject dialogue in _activeDialogues)
		{
			int i = _activeDialogues.IndexOf (dialogue);
			if (dialogue.FollowTarget != null)
			{
				Vector3 loc = _activeDialogues [i].FollowTarget.transform.position;
				Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(loc.x, loc.y + 1, loc.z));
				((RectTransform)_activeDialogues [i].UIObject.transform).anchoredPosition = screenPoint - ((RectTransform)transform).sizeDelta / 2f;
			}
			_activeDialogues [i].Timer -= Time.deltaTime;
			if(_activeDialogues[i].Timer <=0)
			{
				DialogueObject nextDialogue = null; 
				if(_activeDialogues[i].Next != null)
					nextDialogue = new DialogueObject(_activeDialogues [i].Next);
				Destroy (_activeDialogues [i].UIObject);
				_activeDialogues.RemoveAt (i);
				if (nextDialogue != null)
					CreateBox (nextDialogue);
				return;
			}
		}
	}

	public void DeleteAllDialogueBoxes()
	{
		for(int i = 0; i < _activeDialogues.Count; i++)
		{
			Destroy (_activeDialogues [i].UIObject);
		}
		_activeDialogues.Clear ();
	}
}
