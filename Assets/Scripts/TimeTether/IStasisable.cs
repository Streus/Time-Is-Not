using System.Runtime.Serialization;

/// <summary>
/// The Stasisable interface defines objects that can be affected by stasis bubbles
/// </summary>
public interface IStasisable 
{
	void ToggleStasis(bool turnOn); 

	bool IsInStasis (); 
}
