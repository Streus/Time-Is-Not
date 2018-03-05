using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameActive : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		if(SaveManager.HasData())
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
	}
}
