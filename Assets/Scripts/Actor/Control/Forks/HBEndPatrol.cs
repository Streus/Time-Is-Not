using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Patroller/PatrolEnd")]
public class HBEndPatrol : Fork
{
	public override bool check (Controller c)
	{
		PatrollingEnemy pe = State.cast<PatrollingEnemy> (c);

		return pe.getPatrolTarget () == null && pe.getPrevNode() == null;
	}
}
