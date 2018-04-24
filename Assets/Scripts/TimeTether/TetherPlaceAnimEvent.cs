using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherPlaceAnimEvent : MonoBehaviour 
{
	public void OnTetherPlace()
	{
		TetherManager.inst.OnCreateTetherAnimEvent(); 
	}
}
