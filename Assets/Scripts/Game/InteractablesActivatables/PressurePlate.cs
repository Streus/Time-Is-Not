using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interactable, ISavable
{

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

	[Tooltip("Unpressed state sprite.")]
	[SerializeField]
	private Sprite _unpressedSprite;

	[Tooltip("Pressed state sprite.")]
	[SerializeField]
	private Sprite _pressedSprite;

	//is the button pressed?
	private bool _pressed = false;

	// Use this for initialization
	void Start () 
	{
		if (_pressed)
			gameObject.GetComponent<SpriteRenderer> ().sprite = _pressedSprite;
		else
			gameObject.GetComponent<SpriteRenderer> ().sprite = _unpressedSprite;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Check the area for push blocks or a player
		bool checkTest = CircleCheck ();
		//if it does not match the state, update the state and trigger activatables and update sprite
		if(checkTest != _pressed)
		{
			_pressed = checkTest;
			onInteract ();
			if (_pressed)
				gameObject.GetComponent<SpriteRenderer> ().sprite = _pressedSprite;
			else
				gameObject.GetComponent<SpriteRenderer> ().sprite = _unpressedSprite;
		}
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


	/// <summary>
	/// Checks the area on top of the pressure plate for players or push blocks
	/// </summary>
	/// <returns><c>true</c>, if somethings is present, <c>false</c> otherwise.</returns>
	bool CircleCheck()
	{
		bool state = false;
		Collider2D[] colsHit = Physics2D.OverlapCircleAll (transform.position, transform.localScale.x / 2);
		foreach(Collider2D col in colsHit)
		{
			if (col.gameObject.GetComponent<Player> () != null)
				state = true;
			else if (col.gameObject.GetComponent<PushBlock> () != null)
				state = true;
		}
		return state;
	}


	/// <summary>
	/// Trigger the Interactable.
	/// </summary>
	public override void onInteract ()
	{
		foreach(GameObject activatable in _activatables)
		{
			if(activatable.GetComponent<IActivatable>() != null)
				activatable.GetComponent<IActivatable>().onActivate ();
		}
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed (gameObject);

		seed.isPressed = _pressed;

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

		s.defaultLoad (gameObject);
		if(seed.isPressed)
		{
			onInteract ();
		}
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
		//is the plate pressed?
		public bool isPressed;

		public Seed(GameObject subject) : base(subject) {}

	}



}
