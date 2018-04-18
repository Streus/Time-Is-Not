using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StasisUI : MonoBehaviour
{
	[SerializeField] private int stasisUiNum;

	public Sprite onSprite;
	public Sprite offSprite; 

	Image image; 

	void Start()
	{
		image = GetComponent<Image>(); 
	}

	void Update()
	{
		if (StasisUIPanel.inst.transitionState != 0 && StasisUIPanel.inst.transitionState <= stasisUiNum + 1)
		{
			image.sprite = offSprite;
		}
		else
		{
			numStasis(stasisUiNum);
		}
	}
	private void numStasis(int stasisUiNum)
	{
		if(LevelStateManager.numStasis >= stasisUiNum)
		{
			//this.GetComponent<Image>().CrossFadeAlpha(0.15f, 0.25f, true);
			image.sprite = offSprite; 
		}
		else
		{
			//this.GetComponent<Image>().CrossFadeAlpha(1.0f, 0.25f, true);
			image.sprite = onSprite; 
		}
	}
}