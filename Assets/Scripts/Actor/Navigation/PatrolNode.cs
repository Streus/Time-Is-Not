using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MonoBehaviour
{
	[SerializeField]
	private PatrolNode next;

	public void OnDrawGizmos()
	{
		if (next != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine (transform.position, next.transform.position);
		}
	}
}
