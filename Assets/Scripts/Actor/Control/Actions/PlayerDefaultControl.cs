using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Player/Default")]
public class PlayerDefaultControl : Action
{
	public LayerMask moveMask;

	public override void perform (Controller c)
	{
		Player p = State.cast<Player> (c);

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		if (Input.GetKeyDown (KeyCode.Mouse0))
			p.getSelf ().getAbility (0).use (p.getSelf (), mousePos);

		Vector2 movementVector = Vector2.zero;

		if(Input.GetKey(KeyCode.W)) // UP
			movementVector += Vector2.up;
		if (Input.GetKey (KeyCode.A)) // LEFT
			movementVector += Vector2.left;
		if(Input.GetKey(KeyCode.S)) // DOWN
			movementVector += Vector2.down;
		if (Input.GetKey (KeyCode.D)) // RIGHT
			movementVector += Vector2.right;

		movementVector = movementVector.normalized * p.getSelf().getMovespeed () * Time.deltaTime;

		RaycastHit2D[] hits = new RaycastHit2D[1];
		int hitCount = 0;
		ContactFilter2D cf = new ContactFilter2D ();
		cf.SetLayerMask(moveMask);
		hitCount = p.GetComponent<Collider2D> ().Cast (movementVector, cf, hits, p.getSelf ().getMovespeed () * Time.deltaTime);
		//Debug.Log(hitCount); //DEBUGs
		if(hitCount <= 0)
			p.transform.Translate ((Vector3)movementVector);
	}
}
