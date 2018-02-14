using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class CanLoadButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int state;

	Button button; 

	//public Sprite activeSprite;
	//public Sprite inactiveSprite; 

	Image image; 

	void Start()
	{
		button = GetComponent<Button>(); 
		image = GetComponent<Image>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (LevelStateManager.canLoadTetherPoint(state))
        {
            button.interactable = true;
			//image.sprite = activeSprite;  
        }
        else
        {
            button.interactable = false;
			//image.sprite = inactiveSprite; 
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
