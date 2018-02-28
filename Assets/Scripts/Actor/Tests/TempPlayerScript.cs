using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerScript : MonoBehaviour {

	private Animator _animationController;

	private Player _player;

	public float angle;

	private bool isDashing = false;

	void Start () 
	{
		_player = gameObject.GetComponentInParent<Player> ();
		_animationController = GetComponent<Animator> ();
	}

	void Update () 
	{
		if(isDashing != _player.dashing())
		{
			isDashing = _player.dashing ();
			_animationController.SetBool ("Dash", _player.dashing());
		}
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

		if (Input.GetKeyDown (KeyCode.Mouse1) && LevelStateManager.numStasisLeft > 0)
		{
			_animationController.SetTrigger ("StasisBubble");
		}

		if (Input.GetKeyDown (KeyCode.Space) && LevelStateManager.numTetherPointsLeft > 0)
		{
			_animationController.SetTrigger ("PlaceAnchor");
		}


		Vector3 offset = new Vector3 (transform.parent.GetComponent<BoxCollider2D> ().offset.x, transform.parent.GetComponent<BoxCollider2D> ().offset.y, transform.parent.position.z);

		Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.parent.position + offset);

		Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

		angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

		_animationController.SetFloat ("Angle", angle);
	}

	public void ChangeToFacingLeft()
	{
		_animationController.SetInteger ("Direction", 4);
	}

	public void ChangeToFacingDown ()
	{
		_animationController.SetInteger ("Direction", 3);
	}

	public void ChangeToFacingRight()
	{
		_animationController.SetInteger ("Direction", 2);
	}

	public void ChangeToFacingUp ()
	{
		_animationController.SetInteger ("Direction", 1);
	}

	public void PlayTetherAnimation ()
	{
		_animationController.SetTrigger ("ActivateTimeTether");
	}

	public void PlayReappearAnimation ()
	{
		_animationController.SetTrigger ("Reappear");
	}

	float AngleBetweenTwoPoints(Vector3 a, Vector3 b) 
	{
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}
}
