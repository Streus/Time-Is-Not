using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TimerText : MonoBehaviour {
    private Text myText; 
	// Use this for initialization
	void Start () {
        myText = GetComponent<Text>(); 
	}
	
	// Update is called once per frame
	void Update () {
        myText.text = GameManager.inst.timerString;
	}
}
