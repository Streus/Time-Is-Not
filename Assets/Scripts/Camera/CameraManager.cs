using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[SerializeField]
	private Vector2 min = Vector2.zero;
	[SerializeField]
	private Vector2 max = Vector2.zero;

	[Header("Target Following")]
	[Tooltip("Choose whether the camera should gently follow its target, or stay locked to" +
		" its position.")]
	public bool smoothFollow = false;

	[Tooltip("The seepd at which a smooth camera will adjust its position.")]
	public float smoothSpeed = 5f;

	[Tooltip("The distance teh camera must be from its target for the camera" +
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
				//TODO bounds calculations
			}
		}
	}

	public void LateUpdate()
	{
		//tight follow
		if (target != null && !smoothFollow)
		{
			Vector2 dtt = target.position - transform.position;
			if (dtt.magnitude > followRadius)
				transform.position += (Vector3)(dtt.normalized * (dtt.magnitude - followRadius));
			//TODO bounds calculations
		}
	}

	public Transform getTarget()
	{
		return target;
	}

	public void setTarget(Transform t)
	{
		target = t;
	}

	public void setBounds(Vector2 min, Vector2 max)
	{
		this.min = min;
		this.max = max;
	}

	public void shake(float duration, float intensity, float decayRate = 0f)
	{
		shakeDur = duration;
		shakeInt = intensity;
		shakeDec = decayRate;
	}

	public void zoom(float target, float speed = 0f)
	{
		//TODO camera zoom
		throw new System.NotImplementedException ();
	}

	public void recenter(float speed)
	{
		//TODO camera recenter
		throw new System.NotImplementedException ();
	}
	#endregion
}
