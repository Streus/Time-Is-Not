using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
	#region STATIC_VARS

	private static CameraManager _instance;
	public static CameraManager instance { get { return _instance; } }
	#endregion

	#region INSTANCE_VARS
	[Header("Zoom properties")]
	[Tooltip("Hold this key to zoom out")]
	public KeyCode zoomOutKey; 

	public float zoomOutLerpSpeed; 
	public float zoomInLerpSpeed; 
	float regularSize; 
	public float zoomOutSize; 

	bool zoomOut; 

	Vector2 panOffset; 

	[Header("Basic Fields")]
	[SerializeField]
	private Camera cam;

	[SerializeField]
	private Transform target;

	[Header("Bounds")]
	[SerializeField]
	private Vector2 min = Vector2.zero;
	[SerializeField]
	private Vector2 max = Vector2.zero;

	[Header("Target Following")]
	[Tooltip("Choose whether the camera should gently follow its target, or stay locked to" +
		" its position.")]
	public bool smoothFollow = false;

	[Tooltip("The speed at which a smooth camera will adjust its position.")]
	public float smoothSpeed = 5f;

	[Tooltip("The distance the camera must be from its target for the camera" +
		" to begin updating its position.")]
	public float followRadius = 0f;

	private float shakeDur, shakeInt, shakeDec;
	#endregion

	#region STATIC_METHODS

	#endregion

	#region INSTANCE_METHODS

	public void Awake()
	{
		if (_instance == null)
			_instance = this;
		else
		{
			Destroy (gameObject);
			Debug.LogWarning ("More than on CameraManager in scene! Extra CM on " +
				gameObject.name + ".");
			return;
		}

		regularSize = cam.orthographicSize; 

		shakeDur = shakeInt = shakeDec = 0f;
	}

	public void Update()
	{
		//shake the camera
		if (shakeDur > 0f)
		{
			cam.transform.localPosition = (Vector3)Random.insideUnitCircle * shakeInt;

			shakeInt -= shakeDec * Time.deltaTime;
			if (shakeInt <= 0f)
				shakeDur = 0f;

			shakeDur -= Time.deltaTime;
			if (shakeDur <= 0f)
			{
				shake (0, 0);
				cam.transform.localPosition = Vector2.zero;
			}
		}

		//smooth follow
		if (target != null && smoothFollow)
		{
			Vector3 tarPos = transform.position;
			Vector2 dtt = target.position - transform.position;
			if (dtt.magnitude > followRadius)
			{
				tarPos += (Vector3)(dtt.normalized * (dtt.magnitude - followRadius));
				transform.position = Vector3.Lerp (transform.position, tarPos, Time.deltaTime * smoothSpeed);
				fitToBounds ();
			}
		}

		updateZoom(); 
	}

	public void LateUpdate()
	{
		//tight follow
		if (target != null && !smoothFollow)
		{
			Vector2 dtt = target.position - transform.position;
			if (dtt.magnitude > followRadius)
				transform.position += (Vector3)(dtt.normalized * (dtt.magnitude - followRadius));
			fitToBounds ();
		}
	}

	private void fitToBounds()
	{
		//if min and max are not set, do not apply bounds
		if (min == Vector2.zero && max == Vector2.zero)
			return;

		//camera dimensions
		float cHeight = cam.orthographicSize * 2;
		float cWidth = cHeight * cam.aspect;

		//corners of the camera view
		float cMinX = transform.position.x - (cWidth / 2f);
		float cMaxX = transform.position.x + (cWidth / 2f);
		float cMinY = transform.position.y - (cHeight / 2f);
		float cMaxY = transform.position.y + (cHeight / 2f);

		//x clamping
		if (cMinX < min.x)
		{
			if (cMaxX > max.x)
				transform.position = new Vector3 ((min.x + max.x) / 2f, transform.position.y);
			else
				transform.position = new Vector3(
					transform.position.x + min.x - cMinX,
					transform.position.y);
		}
		else if (cMaxX > max.x)
			transform.position = new Vector3(
				transform.position.x + max.x - cMaxX,
				transform.position.y);

		//y claming
		if (cMinY < min.y)
		{
			if (cMaxY > max.y)
				transform.position = new Vector3 ((min.y + max.y) / 2f, transform.position.y);
			else
				transform.position = new Vector3(
					transform.position.x,
					transform.position.y + min.y - cMinY);
		}
		else if (cMaxY > max.y)
			transform.position = new Vector3(
				transform.position.x,
				transform.position.y + max.y - cMaxY);
	}

	public Transform getTarget()
	{
		return target;
	}

	public void setTarget(Transform t)
	{
		target = t;
	}

	/// <summary>
	/// Sets the bounds for this camera. If the bounds were valid, returns true, else
	/// returns false.
	/// </summary>
	public bool setBounds(Vector2 min, Vector2 max)
	{
		if (min.x > max.x || min.y > max.y)
			return false;
		this.min = min;
		this.max = max;
		return true;
	}

	public void shake(float duration, float intensity, float decayRate = 0f)
	{
		shakeDur = duration;
		shakeInt = intensity;
		shakeDec = decayRate;
	}

	/*
	public void zoom(float target) //DEBUG for testing. remove this
	{
		zoom (target, 10);
	}

	public void zoom(float target, float speed = 0f)
	{
		if (speed == 0)
			cam.orthographicSize = target;
		else
			StartCoroutine (doZoom (target, speed));
	}
	private IEnumerator doZoom(float target, float speed)
	{
		speed *= Mathf.Sign (target - cam.orthographicSize);
		while (Mathf.Abs (cam.orthographicSize - target) > 0.01)
		{
			cam.orthographicSize += speed * Time.deltaTime;
			yield return null;
		}
	}
	*/

	public void zoomTo(float targetSize, float speed)
	{
		if (speed == 0)
		{
			cam.orthographicSize = targetSize; 
			return; 
		}

		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, speed); 
	}

	public void recenter(float speed)
	{
		//TODO camera recenter
		throw new System.NotImplementedException ();
	}

	public void setProfile(CameraProfile profile)
	{
		if (profile == null)
			return;

		transform.position = profile.position;
		transform.rotation = Quaternion.Euler(profile.rotation);
		smoothFollow = profile.smoothFollow;
		smoothSpeed = profile.smoothSpeed;
		followRadius = profile.followRadius;

		// TODO see if this needs to be added back
		//zoom (profile.zoomLevel);
		zoomTo(profile.zoomLevel, 0); 
	}

	public void OnDrawGizmos()
	{
		//bounds
		Gizmos.color = Color.red;
		Vector3 boundsCenter = (Vector3)((min + max) / 2);
		Vector3 boundsSize = (Vector3)(max - min);
		Gizmos.DrawWireCube (boundsCenter, boundsSize);

		//follow radius
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position, followRadius);
	}

	/*
	 * Zoom controls
	 */ 

	void updateZoom()
	{
		if (Input.GetKey(zoomOutKey))
		{
			zoomTo(zoomOutSize, zoomOutLerpSpeed);
			zoomOut = true; 
			updateZoomPan(false); 
		}
		else
		{
			zoomTo(regularSize, zoomInLerpSpeed); 
			zoomOut = false; 
			updateZoomPan(true);
		}
	}

	void updateZoomPan(bool panToPlayer)
	{
		if (panToPlayer)
		{
			panOffset = Vector2.Lerp(panOffset, Vector2.zero, 0.2f); 
		}
		else
		{
			// Horizontal pan
			if (Input.mousePosition.x > Screen.width - Screen.width / 8)
			{
				panOffset += new Vector2 (20 * Time.deltaTime, 0); 
			}
			else if (Input.mousePosition.x < 0 + Screen.width / 8)
			{
				panOffset -= new Vector2 (20 * Time.deltaTime, 0); 
			} 

			// Vertical pan
			if (Input.mousePosition.y > Screen.height - Screen.height / 6)
			{
				panOffset += new Vector2 (0, 20 * Time.deltaTime); 
			}
			else if (Input.mousePosition.y < 0 + Screen.width / 6)
			{
				panOffset -= new Vector2 (0, 20 * Time.deltaTime); 
			} 
		}

		cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3 (panOffset.x, panOffset.y, cam.transform.localPosition.z), 10 * Time.deltaTime);
	}

	#endregion
}
