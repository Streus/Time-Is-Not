using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Player/Dashing")]
public class PlayerDashing : Action
{
	public float dashSpeed = 1f;

    public override void perform(Controller c)
    {
        Player p = State.cast<Player>(c);
        Collider2D hitBox = p.GetComponent<Collider2D>();
		p.gameObject.layer = LayerMask.NameToLayer("DashingPlayer");
		p.transform.position = Vector2.MoveTowards (p.transform.position, (Vector2)p.getJumpTargetPos() - hitBox.offset, dashSpeed * Time.deltaTime);
		p.setDashingAnim (true);

		Vector3 offset = new Vector3 (
			p.transform.GetComponent<BoxCollider2D> ().offset.x,
			p.transform.GetComponent<BoxCollider2D> ().offset.y,
			p.transform.position.z);

		Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (
			p.transform.position + offset);

		Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

		p.setDashAngle(Mathf.Atan2(
			positionOnScreen.y - mouseOnScreen.y,
			positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg);
    }
}
