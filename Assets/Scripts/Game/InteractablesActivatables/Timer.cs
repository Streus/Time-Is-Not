using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : Interactable , ISavable, IActivatable
{
	[SerializeField]
	[Tooltip("How long between toggling states")]
	private float _timeInterval;

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

	//the state of the timer
	private bool _state = false;

	private float _timer = 0;

	//does the timer start off?
	private bool isInverted = false;

	// Use this for initialization
	void Start () 
	{
		if (!isEnabled ())
			isInverted = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		_timer += Time.deltaTime;

		if (_timer >= _timeInterval)
			onInteract ();
	}

	/// <summary>
	/// Trigger the Interactable.
	/// </summary>
	public override void onInteract ()
	{
		_state = !_state;
		foreach(GameObject activatable in _activatables)
		{
			if (activatable.GetComponent<IActivatable> () != null)
				activatable.GetComponent<IActivatable> ().onActivate (_state);
		}
	}

	/// <summary>
	/// Toggle the object's state.
	/// </summary>
	public bool onActivate()
	{
		if (isEnabled ())
			disable ();
		else
			enable ();
		return isEnabled ();
	}

	/// <summary>
	/// Set the object's state.
	/// </summary>
	public bool onActivate (bool state)
	{
		//if the door is inverted, a true state enables the timer
		if(isInverted)
		{
			if (state)
				enable ();
			else
				disable ();
		}
		else
		{
			if (state)
				disable ();
			else
				enable ();
		}
		return isEnabled();
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

		seed.state = _state;

		seed.timer = _timer;

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		if (s == null)
			return;

		Seed seed = (Seed)s;

		_state = seed.state;

		_timer = seed.timer;
	}

	/// <summary>
	/// Checks if the object should be able to be reset.
	/// </summary>
	/// <returns><c>true</c>, if it should ignore it, <c>false</c> otherwise.</returns>
	public bool shouldIgnoreReset() 
	{ 
		return false; 
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//the timers state
		public bool state;

		//current timer
		public float timer;

	}

}
