using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerScript : MonoBehaviour {

	private Animator _animationController;

	Vector2 position;
	Vector3 mousePosition;

	void Start () 
	{
		_animationController = GetComponent<Animator> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.W))
		{
			_animationController.SetInteger ("Direction", 1);
		}

		if (Input.GetKeyDown (KeyCode.D))
		{
			_animationController.SetInteger ("Direction", 2);
		}
		if (Input.GetKeyDown (KeyCode.S))
		{
			_animationController.SetInteger ("Direction", 3);
		}
		if (Input.GetKeyDown (KeyCode.A))
		{
			_animationController.SetInteger ("Direction", 4);
		}

		if (Input.GetKeyDown (KeyCode.Mouse1))
		{
			_animationController.SetTrigger ("StasisBubble");
		}

		if (Input.GetKeyDown (KeyCode.Space))
		{
			_animationController.SetTrigger ("PlaceAnchor");
		}

		if (Input.GetKeyDown (KeyCode.Mouse0))
		{
			_animationController.SetTrigger ("Dash");
		}
			
		position = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
		mousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y); 

		_animationController.SetFloat ("Angle", Vector2.Angle (position, mousePosition));

		print ("Angle between player and cursor: " + Vector2.Angle (position, mousePosition)); 
	}

	public void PlayTetherAnimation ()
	{
		_animationController.SetTrigger ("ActivateTimeTether");
	}

	public void PlayReappearAnimation ()
	{
		_animationController.SetTrigger ("Reappear");
	}
}
