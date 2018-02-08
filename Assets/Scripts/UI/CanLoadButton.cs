using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class CanLoadButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int state;

	Button button; 

	void Start()
	{
		button = GetComponent<Button>(); 
	}
	
	// Update is called once per frame
	void Update () {
		if (LevelStateManager.canLoadTetherPoint(state) && GameManager.isPaused())
        {
            this.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
	}

	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (button.interactable)
		{
			//Debug.Log("Mouse is over button for state " + state);
			TetherManager.OnPointerEnter(state); 
		}
	}

	//Do this when the cursor exits the rect area of this selectable UI object.
	public void OnPointerExit(PointerEventData eventData)
	{
		if (button.interactable)
		{
			//Debug.Log("Mouse is no longer over button for state " + state);
			TetherManager.OnPointerExit(); 
		}
	}
}
