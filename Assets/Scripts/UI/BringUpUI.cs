using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringUpUI : MonoBehaviour {
    public GameObject menu;
	// Use this for initialization
	void Start () {
        menu.SetActive(false);
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Space))
        {
            menu.SetActive(true);
        }
        else
        {
            menu.SetActive(false);
        }
    
	}
}
