using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerScript : MonoBehaviour {

	private Animator _animationController;

	void Start () 
	{
		_animationController = GetComponent<Animator> ();
	}

	void Update () {
//		print ();

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
