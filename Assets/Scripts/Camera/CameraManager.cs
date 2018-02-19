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

	[Header("Basic Fields")]
	[SerializeField]
	private Camera cam;

	[SerializeField]
	private Transform target;

	[Header("Bounds")]
	[SerializeField] private Vector2 b_origin = Vector2.zero;
	[SerializeField] private Vector2 b_min = Vector2.zero;
	[SerializeField] private Vector2 b_max = Vector2.zero;

	// Calculated bounds, with origin offset
	private Vector2 min = Vector2.zero;
	private Vector2 max = Vector2.zero;

	// Calculated camera width and height
	float cHeight;
	float cWidth;


	[Header("Target Following")]
	[Tooltip("Choose whether the camera should gently follow its target, or stay locked to" +
		" its position.")]
	public bool smoothFollow = false;

	[Tooltip("The speed at which a smooth camera will adjust its position.")]
	public float smoothSpeed = 5f;

	[Tooltip("The distance the camera must be from its target for the camera" +
		" to begin updating its position.")]
	public float followRadius = 0f;

	[Header("Zoom/pan properties")]
	public float zoomOutLerpSpeed; 
	public float zoomInLerpSpeed; 
	float regularSize; 
	public float zoomOutSize; 

	// True if the camera is zooming out or has finished zooming out; false if zooming back in or not zoomed
	private bool m_zoomState;
	public bool zoomState
	{
		get{
			return m_zoomState; 
		}
	}

	public float vertScrollPercDivisor;
	public float horizScrollPercDivisor; 
	public float panSpeed; 

	private Vector2 panOffset;

	// Camera at bounds booleans
	[Header("Camera at bounds? (Read only)")]
	public bool atLeftBound; 
	public bool atRightBound; 
	public bool atTopBound;
	public bool atBottomBound;

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
				fitToBounds (transform);
			}
		}

		if (Application.isPlaying)
		{
			if (Input.GetKey(PlayerControlManager.LH_ZoomOut) || Input.GetKey(PlayerControlManager.RH_ZoomOut))
			{
				m_zoomState = true; 
				zoomTo(zoomOutSize, zoomOutLerpSpeed);
				updateZoomPan(false);
			}
			else
			{
				m_zoomState = false; 
				zoomTo(regularSize, zoomInLerpSpeed);
				updateZoomPan(true);
			}
		}

		updateAtBounds(); 
	}

	public void LateUpdate()
	{
		//tight follow
		if (target != null && !smoothFollow)
		{
			// dtt = distance to target?
			Vector2 dtt = target.position - transform.position;
			if (dtt.magnitude > followRadius)
				transform.position += (Vector3)(dtt.normalized * (dtt.magnitude - followRadius));
			fitToBounds (transform);
		}

		fitToBounds(cam.transform); 
	}

	void updateCamWidthHeight()
	{
		cHeight = cam.orthographicSize * 2;
		cWidth = cHeight * cam.aspect;
	}

	// sideIndex values:
	// 0 = cMinX
	// 1 = cMaxX
	// 2 = cMinY
	// 3 = cMaxY
	private float getCamSidePos(int sideIndex, Transform t)
	{
		switch (sideIndex)
		{
			// cMinX
			case 0:
				return t.position.x - (cWidth / 2f);
				break; 
			// cMaxX
			case 1:
				return t.position.x + (cWidth / 2f);
				break;
			// cMinY
			case 2:
				return t.position.y - (cHeight / 2f);
				break;
			// cMaxY
			case 3:
				return t.position.y + (cHeight / 2f);
				break;
			default:
				return 0;
				break;
		}
	}

	void updateAtBounds()
	{
		//if min and max are not set, do not apply bounds
		if (b_min == Vector2.zero && b_max == Vector2.zero)
		{
			atLeftBound = false;
			atRightBound = false;
			atTopBound = false; 
			atBottomBound = false; 
			return; 
		}

		//camera dimensions
		updateCamWidthHeight(); 

		//sides of the camera view
		float cMinX = getCamSidePos(0, cam.transform);
		float cMaxX = getCamSidePos(1, cam.transform);
		float cMinY = getCamSidePos(2, cam.transform);
		float cMaxY = getCamSidePos(3, cam.transform);

		// Update at bounds
		// Left bound
		if (cam.transform.position.x - (cWidth / 2f) <= min.x)
			atLeftBound = true;
		else
			atLeftBound = false; 

		// Right bound
		if (cam.transform.position.x + (cWidth / 2f) >= max.x)
			atRightBound = true;
		else
			atRightBound = false; 

		// Top bound
		if (cam.transform.position.y + (cHeight / 2f) >= max.y)
			atTopBound = true;
		else
			atTopBound = false; 

		// Bottom bound
		if (cam.transform.position.y - (cHeight / 2f) <= min.y)
			atBottomBound = true;
		else
			atBottomBound = false; 
	}

	private void fitToBounds(Transform t)
	{
		// Update calculated bounds
		min = b_min + b_origin; 
		max = b_max + b_origin; 

		//if min and max are not set, do not apply bounds
		if (b_min == Vector2.zero && b_max == Vector2.zero)
			return;

		//camera dimensions
		updateCamWidthHeight(); 

		//sides of the camera view
		float cMinX = getCamSidePos(0, t);
		float cMaxX = getCamSidePos(1, t);
		float cMinY = getCamSidePos(2, t);
		float cMaxY = getCamSidePos(3, t);

		//x clamping
		// Reached the left boundary
		if (cMinX < min.x)
		{
			// If the right boundary doesn't fit within the max
			if (cMaxX > max.x)
				t.position = new Vector3 ((min.x + max.x) / 2f, t.position.y, t.position.z);
			else
				t.position = new Vector3(t.position.x + min.x - cMinX, t.position.y, t.position.z);
		}
		else if (cMaxX > max.x)
			t.position = new Vector3(t.position.x + max.x - cMaxX, t.position.y, t.position.z);

		//y claming
		if (cMinY < min.y)
		{
			if (cMaxY > max.y)
				t.position = new Vector3 ((min.y + max.y) / 2f, t.position.y, t.position.z);
			else
				t.position = new Vector3(
					t.position.x, t.position.y + min.y - cMinY, t.position.z);
		}
		else if (cMaxY > max.y)
			t.position = new Vector3(t.position.x, t.position.y + max.y - cMaxY, t.position.z);
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
	public bool setBounds(Vector2 min, Vector2 max, Vector2 origin)
	{
		if (min.x > max.x || min.y > max.y)
			return false;

		this.b_min = min; 
		this.b_max = max; 
		this.b_origin = origin; 

		this.min = b_min + origin;
		this.max = b_max + origin;
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

		//zoom (profile.zoomLevel);
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
		

	void updateZoomPan(bool panToTarget)
	{
		if (panToTarget)
		{
			panOffset = Vector2.Lerp(panOffset, Vector2.zero, 0.2f); 
		}
		else
		{
			// Horizontal pan
			// Right side
			if (!atRightBound && Input.mousePosition.x > Screen.width - Screen.width / horizScrollPercDivisor)
			{
				panOffset += new Vector2 (panSpeed * Time.deltaTime, 0);
			}
			// Left side
			else if (!atLeftBound && Input.mousePosition.x < 0 + Screen.width / horizScrollPercDivisor)
			{
				panOffset -= new Vector2 (panSpeed * Time.deltaTime, 0); 
			} 

			// Vertical pan
			if (!atTopBound && Input.mousePosition.y > Screen.height - Screen.height / vertScrollPercDivisor)
			{
				panOffset += new Vector2 (0, panSpeed * Time.deltaTime); 
			}
			else if (!atBottomBound && Input.mousePosition.y < 0 + Screen.width / vertScrollPercDivisor)
			{
				panOffset -= new Vector2 (0, panSpeed * Time.deltaTime); 
			} 
		}

		Debug.Log("panOffset: " + panOffset); 
		//cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3 (panOffset.x, panOffset.y, cam.transform.localPosition.z), 10 * Time.deltaTime);
		cam.transform.localPosition = new Vector3 (panOffset.x, panOffset.y, cam.transform.localPosition.z); 
	}
	#endregion
}
