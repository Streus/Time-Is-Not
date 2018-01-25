using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> , ISavable
{
	public LevelData levelData; 

	// --- ISavable Methods ---
	public SeedBase saveData()
	{
		//SeedBase seed = new SeedBase (gameObject);
		Seed seed = new Seed (gameObject);
		seed.levelData = levelData; 

		return seed;
	}
	public void loadData(SeedBase s)
	{
		if (s == null)
			return;

		Seed seed = (Seed)s;

		// Seed should not implement a default sow
		//s.defaultSow (gameObject);

		levelData = seed.levelData; 
	}
	public bool ignoreReset() { return false; }

	public class Seed : SeedBase
	{
		/* Instance Vars */
		public LevelData levelData; 

		public Seed(GameObject subject) : base(subject) { }

	}
}

public enum AlertState
{
	INACTIVE,
	TRIGGERED
}; 

public class LevelData
{
	public AlertState alertTriggered; 
}
