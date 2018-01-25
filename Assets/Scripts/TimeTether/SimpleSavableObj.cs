using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SimpleSavableObj : MonoBehaviour, ISavable, IStasisable 
{
	//[SerializeField]
	private bool allowReset = true;

	[SerializeField]
	private bool allowStasis = true;

	bool inStasis; 

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

		if (!allowReset)
		{
			return; 
		}

		s.defaultLoad (gameObject);
	}
	public bool ignoreReset() { return !allowReset; }


	// --- IStasisable Methods ---
	public void ToggleStasis(bool turnOn)
	{
		inStasis = turnOn; 

		if (turnOn && allowStasis)
		{
			allowReset = false; 

			if (stasisVisual != null)
			{
				stasisVisual.SetActive(true); 
			}

			// Play sound
		}
		else
		{
			allowReset = true; 

			if (stasisVisual != null)
			{
				stasisVisual.SetActive(false); 
			}

			// Play sound
		}
	}

	public bool IsInStasis()
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
			if (!allowStasis)
			{
				stasisVisual.SetActive(false); 
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
