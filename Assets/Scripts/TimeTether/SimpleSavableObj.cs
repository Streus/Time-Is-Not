using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RegisteredObject))]
public class SimpleSavableObj : MonoBehaviour, ISavable, IStasisable 
{
	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = true;

	public GameObject stasisVisual; 


	// --- ISavable Methods ---
	public SeedBase saveData()
	{
		SeedBase seed = new SeedBase (gameObject);

		return seed;
	}
	public void loadData(SeedBase s)
	{
		if (s == null)
			return;

		if (!inStasis)
		{
			return; 
		}

		s.defaultLoad (gameObject);
	}
	public bool shouldIgnoreReset() { return !inStasis; }


	// --- IStasisable Methods ---
	public void ToggleStasis(bool turnOn)
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
