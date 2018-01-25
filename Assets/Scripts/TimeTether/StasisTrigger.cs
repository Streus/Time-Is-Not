using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisTrigger : MonoBehaviour 
{
	public GameObject stasisableObject; 
	IStasisable istasisable; 

	List<GameObject> collidingStasisFields; 

	// Use this for initialization
	void Start () 
	{
		collidingStasisFields = new List<GameObject> (); 

		if (stasisableObject == null)
		{
			Debug.LogError("Null stasisableObject. An object with an IStasisable-compatible script must be dragged into the stasisableObject field"); 
		}

		istasisable = (IStasisable)stasisableObject.GetComponent<IStasisable>(); 

		if (istasisable == null)
		{
			Debug.LogError("The gameObject in stasisableObject does not have an IStasisable-compatibile script attached"); 
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (CollidingStasisFieldsIsNull())
		{
			ToggleStasis(false);
			collidingStasisFields.Clear(); 
		}
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.CompareTag("StasisField"))
		{
			ToggleStasis(true); 
			collidingStasisFields.Add(other.gameObject); 
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("StasisField"))
		{
			collidingStasisFields.Remove(other.gameObject); 

			if (collidingStasisFields.Count == 0)
			{
				ToggleStasis(false);
			}
		}
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
		if (istasisable != null)
		{
			if (istasisable.IsInStasis() != turnOn)
			{
				istasisable.ToggleStasis(turnOn);
			}
		}
	}
}
