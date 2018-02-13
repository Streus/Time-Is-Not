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

		// Use Stasis Placement ablility
		if ((Input.GetKeyDown (PlayerControlManager.RH_FireStasis) ||
			Input.GetKeyDown(PlayerControlManager.LH_FireStasis)) && 
			GameManager.inst.canUseDash)
			p.getSelf ().getAbility (0).use (p.getSelf (), mousePos);

		// Use Dash ability
        if (Input.GetKeyDown(PlayerControlManager.RH_Dash) || Input.GetKeyDown(PlayerControlManager.LH_Dash))
        {
            p.getSelf().getAbility(1).use(p.getSelf(), mousePos);
        }

		// Movement
		Vector2 movementVector = Vector2.zero;

		if(Input.GetKey(PlayerControlManager.RH_Up) || Input.GetKey(PlayerControlManager.LH_UP)) // UP
			movementVector += Vector2.up;
		if (Input.GetKey (PlayerControlManager.RH_Left) || Input.GetKey (PlayerControlManager.LH_Left)) // LEFT
			movementVector += Vector2.left;
		if(Input.GetKey(PlayerControlManager.RH_Down) || Input.GetKey(PlayerControlManager.LH_Down)) // DOWN
			movementVector += Vector2.down;
		if (Input.GetKey (PlayerControlManager.RH_Right) || Input.GetKey(PlayerControlManager.LH_Right)) // RIGHT
			movementVector += Vector2.right;

		movementVector = movementVector.normalized * p.getSelf().getMovespeed () * Time.deltaTime;

		// Wall check
		RaycastHit2D[] hits = new RaycastHit2D[1];
		int hitCount = 0;
		ContactFilter2D cf = new ContactFilter2D ();
		cf.SetLayerMask(moveMask);
		hitCount = p.GetComponent<Collider2D> ().Cast (movementVector, cf, hits, p.getSelf ().getMovespeed () * Time.deltaTime);
		if(hitCount <= 0)
			p.transform.Translate ((Vector3)movementVector);
	}
}
