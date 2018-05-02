using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour, IActivatable, ISavable
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

	public GameObject _stasisEffect;

	// Use this for initialization
	void Start () 
	{
		if (!Application.isPlaying)
			return;
		isInverted = !_active;
		if (GetComponent<RegisteredObject> () != null)
			GetComponent<RegisteredObject> ().allowResetChanged += ToggleStasis;
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

	public void OnDestroy()
	{
		if (GetComponent<RegisteredObject> () != null)
			GetComponent<RegisteredObject> ().allowResetChanged -= ToggleStasis;
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
		Seed seed = new Seed ();

		seed.isOn = _active;

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		Seed seed = (Seed)s;

		_active = seed.isOn;
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the object moving?
		public bool isOn;
	}


	//****Stasisable Object Functions****

	/// <summary>
	/// Toggles if the object is in stasis.
	/// </summary>
	/// <param name="turnOn">If set to <c>true</c> turn on.</param>
	private void ToggleStasis(bool turnOn)
	{
		inStasis = turnOn;

		SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer> ();
		if (sprite == null)
			return;
		if(_stasisEffect != null) 
		{
			_stasisEffect.SetActive (inStasis);
			_stasisEffect.GetComponent<SpriteRenderer> ().sortingOrder = gameObject.GetComponent<SpriteRenderer> ().sortingOrder + 1;
		}
		else
		{
			if (inStasis) 
				sprite.color = Color.yellow;
			else
				sprite.color = Color.white;
		}
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
