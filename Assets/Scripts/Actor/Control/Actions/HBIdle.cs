using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Hummingbird/HBIdle")]
public class HBIdle : Action
{
	public float turnTolerance = 0.1f;

	public float originDistTolerance = 0.01f;

	public override void perform (Controller c)
	{
		PatrollingEnemy pe = State.cast<PatrollingEnemy> (c);

		//move to origin
		if (Vector3.Distance (pe.transform.position, pe.getOrigin ()) > originDistTolerance)
		{
			Vector3 navPos;
			if (c.getMap () != null)
			{
				if (!c.currentPosition (out navPos))
				{
					c.setPath (pe.getOrigin());

					//throw out the first node in the path
					Vector3 trash;
					c.nextPosition (out trash);
				}
			}
			else
				navPos = pe.getOrigin();

			//turn to face move direction; wait until turning is done
			pe.facePoint (navPos, pe.getTurnSpeed () * Time.deltaTime);
			if (Vector3.Angle (pe.transform.position - navPos, -pe.transform.up) < turnTolerance)
			{
				float dist = Vector2.Distance (c.transform.position, navPos);
				float moveDist = c.getSelf ().getMovespeed () * Time.deltaTime;
				if (moveDist > dist)
					moveDist = dist;

				//move
				c.transform.Translate (
					(navPos - c.transform.position).normalized *
					moveDist, Space.World);
			}
		}
		//reorienting idle behavior
		else
		{
			//TODO HB idle reorient behavior
			#if FALSE
			if (pe.updateReorientTimer (Time.deltaTime))
			{
				pe.transform.rotation += Quaternion.Euler (0f, 0f, pe.getReorientAngle () * Time.deltaTime);
				if(
			}
			#endif
			pe.transform.Rotate(0f, 0f, pe.getTurnSpeed () * Time.deltaTime);
		}
	}
}
