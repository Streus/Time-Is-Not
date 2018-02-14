using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerScript : MonoBehaviour {

	private Animator _animationController;

	void Start () {
		_animationController = GetComponent<Animator> ();
	}

	void Update () {
//		print ();

		_animationController.SetFloat ("xSpeed", Input.GetAxis ("Horizontal"));
		_animationController.SetFloat ("ySpeed", Input.GetAxis ("Vertical"));
//		_animationController.SetTrigger ("ActivateTimeTether");
//		_animationController.SetTrigger ("Reappear");
	}
}
