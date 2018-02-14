using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisUIPanel : MonoBehaviour 
{
	public GameObject stasisPanel; 

	// Use this for initialization
	void Start () 
	{
		UpdateStasisPanelActive(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		// TODO UpdateStasisPanelActive() should probably be called when the stasis ability is collected
	}

	public void UpdateStasisPanelActive()
	{
		if (GameManager.inst.canUseStasis)
		{
			stasisPanel.SetActive(true); 
		}
		else
		{
			stasisPanel.SetActive(false); 
		}
	}
}
