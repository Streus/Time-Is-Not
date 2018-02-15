using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUIPanel : MonoBehaviour 
{
	[Tooltip("(Drag In) The dash UI piece of the time tether that displays dash UI")]
	public GameObject dashPanel; 

	// Use this for initialization
	void Start () 
	{
		UpdateDashPanelActive(); 
	}

	// Update is called once per frame
	void Update () 
	{
		// TODO UpdateDashPanelActive() should probably be called when the dash ability is collected
	}

	public void UpdateDashPanelActive()
	{
		if (GameManager.inst.canUseDash)
		{
			dashPanel.SetActive(true); 
		}
		else
		{
			dashPanel.SetActive(false); 
		}
	}
}
