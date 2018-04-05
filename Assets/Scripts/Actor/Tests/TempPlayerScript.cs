using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Moved all of these into the main Player Script.")]
public class TempPlayerScript : MonoBehaviour {

	private Animator _animationController;

	private Player _player;

	public float angle;

	private bool isDashing = false;

	private bool isPushing = false;

	void Start () 
	{
		_player = gameObject.GetComponentInParent<Player> ();
		_animationController = GetComponent<Animator> ();
	}

	void Update () 
	{
		if (GameManager.inst != null && GameManager.CheckPause (Player.PAUSEMASK_MOVE))
		{
			_animationController.SetBool ("isMoving", false);
			return;
		}

		if (isPushing != _player.pushing ())
		{
			isPushing = _player.pushing ();
			_animationController.SetBool ("isPushing", _player.pushing ());
		} 

		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A))
		{
			_animationController.SetBool ("isMoving", true);
		}

		if ((Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.A)))
		{
			_animationController.SetBool ("isMoving", false);
		}

		if(isDashing != _player.dashing())
		{
			isDashing = _player.dashing ();
			_animationController.SetBool ("Dash", _player.dashing());
		}

		if (Input.GetKey (KeyCode.W))
		{
			_animationController.SetInteger ("Direction", 1);
		}

		if (Input.GetKey (KeyCode.D))
		{
			_animationController.SetInteger ("Direction", 2);
		}
		if (Input.GetKey (KeyCode.S))
		{
			_animationController.SetInteger ("Direction", 3);
		}
		if (Input.GetKey (KeyCode.A))
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
		// Temp fix
		if (_animationController != null)
			_animationController.SetTrigger ("ActivateTimeTether");
	}

	public void PlayReappearAnimation ()
	{
		// Temp fix
		if (_animationController != null)
			_animationController.SetTrigger ("Reappear");
	}

	float AngleBetweenTwoPoints(Vector3 a, Vector3 b) 
	{
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}
}
