using System.Runtime.Serialization;

public interface ISavable
{
	// Extract important values and return them in a serializable class
	// AKA reap
	SeedBase saveData ();

	// Take a serializable class and attempt to use it to fill values
	// AKA sow
	void loadData(SeedBase seed);

	// if true, the SSM will never reset this object back to its default values
	bool ignoreReset ();
}