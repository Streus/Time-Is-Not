using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANDOutput : Interactable, ISavable
{

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

	private ANDInput[] _inputs;

	private bool _state = false;



	// Use this for initialization
	void Awake () 
	{
		_inputs = GetComponentsInChildren<ANDInput> ();
	}

	// Update is called once per frame
	void Update () 
	{
		bool checkTest = true;
		for(int i = 0; i < _inputs.Length; i++)
		{
			if (!_inputs [i].State)
				checkTest = false;
		}
		//if it does not match the state, update the state and trigger activatables
		if(checkTest != _state)
		{
			_state = checkTest;
			onInteract ();
		}
		
	}

	/// <summary>
	/// Trigger the Interactable.
	/// </summary>
	public override void onInteract ()
	{
		if (_activatables.Length == 0)
			return;
		foreach(GameObject activatable in _activatables)
		{
			if(activatable != null)
			{
				if (activatable.GetComponent<IActivatable> () != null)
					activatable.GetComponent<IActivatable> ().onActivate (_state);
			}
		}
	}

	void OnLoad(bool success)
	{
		if(success)
			onInteract();
	}

	void OnDestroy()
	{
		if(LevelStateManager.inst != null)
			LevelStateManager.inst.stateLoaded -= OnLoad;
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

		if(seed.state)
		{
			onInteract ();
		}
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the plate pressed?
		public bool state;
	}

	//Draw lines to all linked activatables
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		for(int i = 0; i < _activatables.Length; i++)
		{
			if(_activatables[i] != null)
				Gizmos.DrawLine (transform.position, _activatables[i].transform.position);
		}

	}
}
