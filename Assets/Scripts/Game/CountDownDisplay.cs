using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountDownDisplay : MonoBehaviour 
{
	private Text screenText;

	// Use this for initialization
	void Start () 
	{
		screenText = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameManager.inst.useEndTimer)
			screenText.text = GameManager.inst.timerString;
		else
			screenText.text = "";

	}
}
