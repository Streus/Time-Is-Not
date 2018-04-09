using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFollower : MonoBehaviour 
{
	[SerializeField]
	private LineRenderer _lineRenderer;

	[SerializeField]
	private float _moveSpeed = 1;

	[SerializeField]
	bool hideOnZoom; 

	SpriteRenderer rend; 

	private int _curPath = 1;

	private float _curProgress = 0;

	private float _curLength;

	// Use this for initialization
	void Awake () 
	{
		int _curPath = 1;

		float _curProgress = 0;
		//_curLength = Vector3.Distance (_lineRenderer.GetPosition (_curPath), _lineRenderer.GetPosition (_curPath - 1));

		rend = GetComponent<SpriteRenderer>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		_curProgress += _moveSpeed * Time.deltaTime / _curLength;
		if(_curProgress >= 1)
		{
			_curProgress = 0;
			_curPath++;
			if (_curPath >= _lineRenderer.positionCount) 
			{
				Destroy (gameObject);
				return;
			}
			_curLength = Vector3.Distance (_lineRenderer.GetPosition (_curPath), _lineRenderer.GetPosition (_curPath - 1));
		}
		transform.position = _lineRenderer.GetPosition (_curPath - 1) + (_lineRenderer.GetPosition (_curPath) - _lineRenderer.GetPosition (_curPath - 1)) * _curProgress;

		if (hideOnZoom)
		{
			if (GameManager.CameraIsZoomedOut())
			{
				rend.enabled = true; 
			}
			else
			{
				rend.enabled = false; 
			}
		}
	}

	public LineRenderer Renderer
	{
		get{return _lineRenderer;}
		set
		{
			_lineRenderer = value;
			_curLength = Vector3.Distance (_lineRenderer.GetPosition (_curPath), _lineRenderer.GetPosition (_curPath - 1));
		}
	}

	public float MoveSpeed
	{
		get{return _moveSpeed;}
		set
		{
			if (value < 0)
				_moveSpeed = 0;
			else
				_moveSpeed = value;
		}
	}


}
