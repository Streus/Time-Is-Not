using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherDetector : Interactable
{
	[SerializeField]
	private Vector2 _range = new Vector2(1,1);

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

	// Use this for initialization
	void Start () 
	{
		
	}

	// Update is called once per frame
	void Update () 
	{
		if(CheckForTether())
		{
			onInteract ();
		}
	}

	bool CheckForTether()
	{
		Collider2D[] colsHit = Physics2D.OverlapBoxAll (transform.position, _range, 0);
		for(int i = 0; i < colsHit.Length; i++)
		{
			if(colsHit[i].GetComponentInParent<TetherIndicator>() != null)
			{
				return true;
			}
		}
		return false;
	}

	public override void onInteract ()
	{
		if (!isEnabled())
			return;
		ToggleSecurityDoors ();
		disable ();
		if (_activatables == null || _activatables.Length == 0)
			return;
		foreach(GameObject activatable in _activatables)
		{
			if (activatable != null) 
			{
				if (activatable.GetComponent<IActivatable> () != null)
					activatable.GetComponent<IActivatable> ().onActivate ();
			}
		}
	}

	void OnDrawGizmos()
	{
		float xdist = _range.x / 2;
		float ydist = _range.y / 2;
		Vector2 center = (transform.position);
		Gizmos.color = Color.green;
		Gizmos.DrawLine (center + new Vector2(-xdist, ydist), center + new Vector2(xdist, ydist));
		Gizmos.DrawLine (center + new Vector2(xdist, -ydist), center + new Vector2(xdist, ydist));
		Gizmos.DrawLine (center + new Vector2(-xdist, -ydist), center + new Vector2(xdist, -ydist));
		Gizmos.DrawLine (center + new Vector2(-xdist, ydist), center + new Vector2(-xdist, -ydist));
	}
}
