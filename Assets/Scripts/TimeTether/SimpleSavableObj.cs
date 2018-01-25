using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSavableObj : MonoBehaviour, ISavable 
{
	[SerializeField]
	private bool allowReset = true;


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

		s.defaultSow (gameObject);
	}
	public bool ignoreReset() { return !allowReset; }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
