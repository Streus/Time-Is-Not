using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProfile", menuName = "Camera/Profile")]
public class CameraProfile : ScriptableObject
{
	#region INSTANCE_VARS
	public Vector3 position = Vector3.zero;
	public Vector3 rotation = Vector3.zero;
	public bool smoothFollow = false;
	public float smoothSpeed = 5f;
	public float followRadius = 0f;
	public float zoomLevel = 5f;
	#endregion
}
