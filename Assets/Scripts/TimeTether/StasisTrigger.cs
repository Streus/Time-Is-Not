using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class StasisTrigger : MonoBehaviour 
{
	public GameObject go; 
	private RegisteredObject ro; 

	private List<GameObject> collidingStasisFields; 

	// Use this for initialization
	void Start () 
	{
		/*
		collidingStasisFields = new List<GameObject> (); 

		if (go == null)
		{
			Debug.LogError("Null go. An object with an ro-compatible script must be dragged into the go field"); 
		}

		ro = go.GetComponent<RegisteredObject>(); 

		if (ro == null)
		{
			Debug.LogError("The gameObject in go does not have an ro-compatibile script attached"); 
		}
		*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		if (CollidingStasisFieldsIsNull())
		{
			ToggleStasis(false);
			collidingStasisFields.Clear(); 
		}
		*/
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		/*
		if (other.CompareTag("StasisField"))
		{
			ToggleStasis(true); 
			collidingStasisFields.Add(other.gameObject); 
		}
		*/
	}

	void OnTriggerExit2D(Collider2D other)
	{
		/*
		if (other.CompareTag("StasisField"))
		{
			collidingStasisFields.Remove(other.gameObject); 

			if (collidingStasisFields.Count == 0)
			{
				ToggleStasis(false);
			}
		}
		*/
	}


	bool CollidingStasisFieldsIsNull()
	{
		if (collidingStasisFields.Count > 0)
		{
			// As long as one of the fields isn't null, keep counting the collision
			for (int i = 0; i < collidingStasisFields.Count; i++)
			{
				if (collidingStasisFields[i] != null)
				{
					return false; 
				}
			}
			return true; 
		}

		return false; 
	}

	void ToggleStasis(bool turnOn)
	{
		if (ro != null)
		{
			if (ro.getAllowReset() != turnOn)
			{
				ro.setAllowReset(turnOn);
			}
		}
	}
}
