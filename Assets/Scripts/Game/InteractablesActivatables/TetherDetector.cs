using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherDetector : Interactable
{
	[SerializeField]
	private Vector2 _range = new Vector2(1,1);
	[SerializeField]
	private Vector2 _offset = new Vector2(0,0);

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;


	[SerializeField]
	private bool _reusable = false;

	private bool _tetherInRange = false;

	private SpriteRenderer _rend;

	[SerializeField]
	private Sprite _activeSprite;
	[SerializeField]
	private Sprite _inactiveSprite;

	// Use this for initialization
	void Start () 
	{
		_rend = gameObject.GetComponent<SpriteRenderer> ();
		if (_rend != null)
			_rend.sprite = _inactiveSprite;
	}

	// Update is called once per frame
	void Update () 
	{
		if (CheckForTether ()) {
			onInteract ();
		} else
			_tetherInRange = false;
	}

	bool CheckForTether()
	{
		Collider2D[] colsHit = Physics2D.OverlapBoxAll (transform.position + (Vector3)_offset, _range, 0);
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
		if ((_reusable && _tetherInRange)|| !isEnabled())
			return;
		if (_rend != null)
			_rend.sprite = _activeSprite;
		_tetherInRange = true;
		ToggleSecurityDoors ();
		if(!_reusable)
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
		Vector2 center = (transform.position + (Vector3)_offset);
		Gizmos.color = Color.green;
		Gizmos.DrawLine (center + new Vector2(-xdist, ydist), center + new Vector2(xdist, ydist));
		Gizmos.DrawLine (center + new Vector2(xdist, -ydist), center + new Vector2(xdist, ydist));
		Gizmos.DrawLine (center + new Vector2(-xdist, -ydist), center + new Vector2(xdist, -ydist));
		Gizmos.DrawLine (center + new Vector2(-xdist, ydist), center + new Vector2(-xdist, -ydist));
	}
}
