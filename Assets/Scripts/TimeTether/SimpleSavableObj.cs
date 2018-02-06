using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RegisteredObject))]
public class SimpleSavableObj : MonoBehaviour, ISavable 
{
	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = false;

	public GameObject stasisVisual; 

	public void OnDestroy()
	{
		GetComponent<RegisteredObject> ().allowResetChanged -= ToggleStasis;
	}

	// --- ISavable Methods ---
	public SeedBase saveData()
	{
		SeedBase seed = new SeedBase ();

		return seed;
	}
	public void loadData(SeedBase s)
	{
		
	}

	// --- IStasisable Methods ---
	private void ToggleStasis(bool turnOn)
	{
		inStasis = turnOn; 

		if (turnOn)
		{
			if (stasisVisual != null)
			{
				stasisVisual.SetActive(true); 
			}

			// Play sound
		}
		else
		{
			if (stasisVisual != null)
			{
				stasisVisual.SetActive(false); 
			}

			// Play sound
		}
	}

	public bool InStasis()
	{
		return inStasis; 
	}


	// --- Monobehaviors ---

	// Use this for initialization
	void Start () 
	{
		if (stasisVisual != null)
		{
			stasisVisual.SetActive(inStasis); 
		}

		GetComponent<RegisteredObject> ().allowResetChanged += ToggleStasis;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
