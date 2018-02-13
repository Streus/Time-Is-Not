using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class CanLoadButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int state;

	Button button; 

	[Tooltip("The color of ui_tetherPoints when holding a state")]
	public Color ui_pointActiveColor; 

	[Tooltip("The color of ui_tetherPoints when not holding a state")]
	public Color ui_pointInactiveColor;

	Image image; 

	void Start()
	{
		button = GetComponent<Button>(); 
		image = GetComponent<Image>(); 
	}
	
	// Update is called once per frame
	void Update () {
		//if (LevelStateManager.canLoadTetherPoint(state) && GameManager.isPaused())
		if (LevelStateManager.canLoadTetherPoint(state))
        {
            this.GetComponent<Button>().interactable = true;
			image.color = ui_pointActiveColor; 
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
			image.color = ui_pointInactiveColor; 
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
