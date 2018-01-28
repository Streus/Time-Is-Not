using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/BeginBasicNav")]
public class BeginBasicNav : Action
{
	public Vector3 targetPostion;

	public override void perform (Controller c)
	{
		c.setPath (targetPostion);
		Debug.Log ("Navigating to " + targetPostion.ToString ());
	}
}
