using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTimerTrigger : MonoBehaviour {
    public Text[] WorldCanvses;
    public bool TurnOnTimers; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(TurnOnTimers == true){
            TurnOnTexts();
        }
	}

    public void OnTriggerEnter2D(Collider2D other){ 
        if(other.gameObject.tag == "Player"){ 
            if(GameManager.inst.useEndTimer == false){ 
                GameManager.inst.StartEndTimer();
                TurnOnTimers = true;
            }
        }
    }

    public void TurnOnTexts() 
    {
        for (int i = 0; i < WorldCanvses.Length; i++){
            if (WorldCanvses[i].enabled == false)
            {
                WorldCanvses[i].enabled = true;
            }
        }
        TurnOnTimers = false;
    }
}
