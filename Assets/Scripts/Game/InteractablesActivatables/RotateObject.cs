using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour, IActivatable, ISavable, IStasisable
{
	[Tooltip("Which direction to rotate.")]
	[SerializeField]
	private bool _clockwise = true;

	[Tooltip("Is the object moving?")]
	[SerializeField]
	private bool _active = true;

	[Tooltip("How fast the object turns.")]
	[SerializeField]
	private float _turnSpeed = 1;

	// Determines whether in stasis. Returned when ISavable calls ignoreReset, and modfied via ToggleStasis
	private bool inStasis = false;

	//is the object's default state inverted?
	private bool isInverted = false;

	// Use this for initialization
	void Start () 
	{
		if (!Application.isPlaying)
			return;
		isInverted = !_active;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Rotate ();
	}

	void Rotate()
	{
		if (!_active || inStasis || GameManager.isPaused())
			return;
		int turnDirection = 1;
		if (_clockwise)
			turnDirection = -1;
			
		transform.Rotate (Vector3.forward * _turnSpeed * turnDirection * Time.deltaTime);
	}

	/// <summary>
	/// Toggles the object's state and returns it.
	/// </summary>
	public bool onActivate()
	{
		_active = !_active;
		return _active;
	}

	public bool onActivate (bool state)
	{
		//if inverted: 'true' state enables movement, otherwise it disables it
		if (isInverted)
			_active = state;
		else
			_active = !state;
		return _active;
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed (gameObject);

		seed.isOn = _active;

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		if (s == null || inStasis)
			return;

		Seed seed = (Seed)s;

		s.defaultLoad (gameObject);

		_active = seed.isOn;
	}

	/// <summary>
	/// Checks if the object should be able to be reset.
	/// </summary>
	/// <returns><c>true</c>, if it should ignore it, <c>false</c> otherwise.</returns>
	public bool shouldIgnoreReset() 
	{ 
		return !inStasis; 
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the object moving?
		public bool isOn;

		public Seed(GameObject subject) : base(subject) {}

	}


	//****Stasisable Object Functions****

	/// <summary>
	/// Toggles if the object is in stasis.
	/// </summary>
	/// <param name="turnOn">If set to <c>true</c> turn on.</param>
	public void ToggleStasis(bool turnOn)
	{
		inStasis = turnOn;

		SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer> ();
		if (sprite == null)
			return;
		if(inStasis)
			sprite.color = Color.yellow;
		else
			sprite.color = Color.white;
	}

	/// <summary>
	/// shows if the object is in stasis
	/// </summary>
	/// <returns><c>true</c>, if stasis is active, <c>false</c> otherwise.</returns>
	public bool InStasis()
	{
		return inStasis; 
	}
}
