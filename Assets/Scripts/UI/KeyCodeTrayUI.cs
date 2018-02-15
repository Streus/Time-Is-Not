using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 



public class KeyCodeTrayUI : MonoBehaviour 
{
	RectTransform rectTransform; 
	float targetXPos; 

	[SerializeField] Image[] keycodeSlots; 

	public float startPosX; 
	public float slotSpacingMultiplier; 
	public float lerpSpeed = 1; 

	[System.Serializable]
	struct KeyCodeSpriteMapping
	{
		public CodeName keyCode; 
		public Sprite sprite; 
	}

	[SerializeField] KeyCodeSpriteMapping[] codeMapping; 

	// Use this for initialization
	void Start () 
	{
		rectTransform = GetComponent<RectTransform>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateSlots(); 
	}

	void UpdateSlots()
	{
		targetXPos = startPosX + (GameManager.NumCodesFound() * slotSpacingMultiplier); 
		rectTransform.anchoredPosition = new Vector2 (Mathf.Lerp(rectTransform.anchoredPosition.x, targetXPos, lerpSpeed * Time.deltaTime), rectTransform.anchoredPosition.y); 

		// Update images
		for (int i = 0; i < keycodeSlots.Length; i++)
		{
			// Images that should show keycodes
			if (i < GameManager.NumCodesFound())
			{
				keycodeSlots[i].sprite = GetSpriteFromCode(GameManager.inst.codes[i]); 
			}
			// Images that shouldn't
			else
			{
				keycodeSlots[i].sprite = null; 
			}
		}
	}

	public Sprite GetSpriteFromCode(CodeName code)
	{
		for (int i = 0; i < codeMapping.Length; i++)
		{
			if (codeMapping[i].keyCode == code)
			{
				return codeMapping[i].sprite; 
			}
		}

		return null; 
	}
}
