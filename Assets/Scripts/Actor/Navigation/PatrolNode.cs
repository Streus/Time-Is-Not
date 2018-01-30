using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MonoBehaviour
{
	[SerializeField]
	private PatrolNode next;

	public PatrolNode getNext()
	{
		return next;
	}

	public void OnDrawGizmos()
	{
		if (next != null)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawLine (transform.position, next.transform.position);
		}
	}
}
