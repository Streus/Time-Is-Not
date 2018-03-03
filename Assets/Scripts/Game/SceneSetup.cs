using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : Singleton<SceneSetup> 
{
	[Header("Camera Bounds")]
	public Vector2 b_origin = Vector2.zero;
	public Vector2 b_min = Vector2.zero;
	public Vector2 b_max = Vector2.zero;

	[Header("Scene ability settings")]
	public bool canUseStasis; 
	public bool canUseDash; 
}
