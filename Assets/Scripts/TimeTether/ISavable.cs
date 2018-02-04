using System.Runtime.Serialization;
using UnityEngine; 

/// <summary>
/// The Savable interface defines functions that must be implemented by scripts which save data (seeds) as part of the time tether
/// It is based on a farmer method of serialization that uses reap (save) and sow (load) functions
/// </summary>
public interface ISavable
{
	// Reapable methods

	// Extract important values and return them in a serializable class
	// AKA reap
	SeedBase saveData ();

	// Take a serializable class and attempt to use it to fill values
	// AKA sow
	void loadData(SeedBase seed);
}