using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherScript : MonoBehaviour 
{
	[Tooltip("The collider that the crusher is able to crush the player against")]
	[SerializeField]
	private BoxCollider2D _targetCollider;

	private float boundsAdjustment = 0.02f;


	//This is the collider attached to this object
	private BoxCollider2D _col;


	// Use this for initialization
	void Start () 
	{
		_col = gameObject.GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Crush ();
	}

	void OnDrawGizmos()
	{
		_col = gameObject.GetComponent<BoxCollider2D> ();
		//Gizmos.DrawCube (transform.position, (Vector3)transform.localScale + (new Vector3 (1, 1, 1) * boundsAdjustment));
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine (transform.position + new Vector3 (-1 * (transform.localScale.x / 2 + boundsAdjustment),transform.localScale.y / 2 + boundsAdjustment, 0), transform.position + new Vector3 (transform.localScale.x / 2 + boundsAdjustment, transform.localScale.y / 2 + boundsAdjustment, 0));
		Gizmos.DrawLine (transform.position + new Vector3 (transform.localScale.x / 2 + boundsAdjustment, transform.localScale.y / 2 + boundsAdjustment, 0), transform.position + new Vector3 (transform.localScale.x / 2 + boundsAdjustment, -1 * (transform.localScale.y / 2 + boundsAdjustment), 0));
		Gizmos.DrawLine (transform.position + new Vector3 (transform.localScale.x / 2 + boundsAdjustment, -1 * (transform.localScale.y / 2 + boundsAdjustment), 0), transform.position + new Vector3 (-1 * (transform.localScale.x / 2 + boundsAdjustment), -1 * (transform.localScale.y / 2 + boundsAdjustment), 0));
		Gizmos.DrawLine (transform.position + new Vector3 (-1 * (transform.localScale.x / 2 + boundsAdjustment), -1 * (transform.localScale.y / 2 + boundsAdjustment), 0), transform.position + new Vector3 (-1 * (transform.localScale.x / 2 + boundsAdjustment), transform.localScale.y / 2 + boundsAdjustment, 0));
	}

	bool Crush()
	{
		if (_col == null || _targetCollider == null)
			return false;
		Collider2D[] colsHitByTarget = Physics2D.OverlapBoxAll (_targetCollider.transform.position, _targetCollider.transform.localScale + new Vector3(boundsAdjustment, boundsAdjustment, 0),0f);
		Collider2D[] colsHitByMe = Physics2D.OverlapBoxAll (_col.transform.position, _col.transform.localScale + new Vector3(boundsAdjustment, boundsAdjustment, 0),0f);
		Player p1 = null;
		Player p2 = null;
		bool iFoundPlayer = false;

		Entity ent = null;
		for(int i = 0; i < colsHitByMe.Length; i++)
		{
			p1 = colsHitByMe [i].GetComponent<Player> ();
			if (p1 != null) 
			{
				ent = p1.GetComponent<Entity> ();
				iFoundPlayer = true;
			}
		}
		bool theyFoundPlayer = false;
		for(int i = 0; i< colsHitByTarget.Length; i++)
		{
			p2 = colsHitByTarget [i].GetComponent<Player> ();
			if (p2 != null)
				theyFoundPlayer = true;
		}
			
		if(iFoundPlayer && theyFoundPlayer)
		{
			if(ent != null)
			{
				ent.onDeath ();
			}
			else
			{
				Debug.Log("couldnt find entity");
			}
		}
		return (theyFoundPlayer && iFoundPlayer);
	}


}
