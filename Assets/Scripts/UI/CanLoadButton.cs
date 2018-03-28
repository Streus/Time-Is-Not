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

    bool hover = false;

	[Tooltip("Drag in the next tether point button, if it exists")] 
	[SerializeField] CanLoadButton nextButton; 

	bool beingRemoved; 
	public Color beingRemovedColor; 

	[SerializeField] Button removeButton; 
	public bool hideRemoveButton; 

	void Start()
	{
		button = GetComponent<Button>(); 
		image = GetComponent<Image>(); 

		if (hideRemoveButton && removeButton != null)
			removeButton.gameObject.SetActive(false); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (LevelStateManager.canLoadTetherPoint(state))
        {
            button.interactable = true;
			//image.sprite = activeSprite; 

			if (removeButton != null && !hideRemoveButton)
				removeButton.gameObject.SetActive(true); 
        }
        else
        {
            button.interactable = false;
			//image.sprite = inactiveSprite; 

			if (removeButton != null)
				removeButton.gameObject.SetActive(false); 
        }
	}

	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (button.interactable)
		{
			//Debug.Log("Mouse is over button for state " + state);
			TetherManager.OnPointerEnter(state);
            if (!hover)
            {
                //if (!GlobalAudio.ClipIsPlaying(AudioLibrary.inst.tetherMenuHover))
                //{
                    AudioLibrary.PlayTetherMenuHover();
                //}
                hover = true;

				StartBeingRemovedSequence(true); 
            }
		}
	}

	//Do this when the cursor exits the rect area of this selectable UI object.
	public void OnPointerExit(PointerEventData eventData)
	{
		if (button.interactable)
		{
			//Debug.Log("Mouse is no longer over button for state " + state);
			TetherManager.OnPointerExit();
            hover = false;

			StartBeingRemovedSequence(false); 
		}
	}

	void StartBeingRemovedSequence(bool isBeingRemoved)
	{
		if (nextButton != null)
		{
			nextButton.SetToBeingRemoved(isBeingRemoved); 
		}
	}
		
	public void SetToBeingRemoved(bool isBeingRemoved)
	{
		beingRemoved = isBeingRemoved; 

		if (isBeingRemoved && button.interactable)
		{
			image.color = beingRemovedColor;
		}
		else
		{
			image.color = Color.white; 
		}

		if (nextButton != null)
		{
			nextButton.SetToBeingRemoved(isBeingRemoved); 
		}
	}

	public void OnRemoveButton()
	{
		Debug.Log("On Remove Button: " + state); 

		TetherManager.inst.RemoveTetherPoint(state); 

		TetherManager.inst.tetherMenuDisableTimer = 0.2f; 
	}
}
