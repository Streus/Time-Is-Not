using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class CodePickup : MonoBehaviour
{
	[Header("Use the dropdown to automatically set the code type")] 
	[Tooltip("The number of the current key code, which will add to the inventory. It will automatically update in the Inspector if set in edit mode")]
	public CodeName codeName; 

	[Header("Other (Don't edit)")] 
	[Tooltip("(Drag in) The sprites for each code.")]
	public Sprite[] codeSprites; 

	SpriteRenderer rend; 

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<Entity> () != null) 
		{
			if (col.CompareTag("Player"))
			{
				AddCode();
                AudioLibrary.PlayCodePickupSound();
			}
		}
	}

	// MonoBehaviors

	void Start()
	{
		rend = GetComponent<SpriteRenderer>(); 
		ChooseCodeSprite(); 
	}

	void Update()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			ChooseCodeSprite(); 
		}

		#endif
	}

	public void AddCode()
	{
		GameManager.AddCode(codeName); 
		gameObject.SetActive(false); 
	}

	public void ChooseCodeSprite()
	{
		if (codeName == CodeName.CODE_1)
			rend.sprite = codeSprites[0]; 
		else if (codeName == CodeName.CODE_2)
			rend.sprite = codeSprites[1]; 
		else if (codeName == CodeName.CODE_3)
			rend.sprite = codeSprites[2]; 
		else if (codeName == CodeName.CODE_4)
			rend.sprite = codeSprites[3]; 
		else if (codeName == CodeName.CODE_5)
			rend.sprite = codeSprites[4]; 
		else if (codeName == CodeName.CODE_6)
			rend.sprite = codeSprites[5]; 
		else if (codeName == CodeName.CODE_7)
			rend.sprite = codeSprites[6]; 
		else if (codeName == CodeName.CODE_8)
			rend.sprite = codeSprites[7]; 
	}
}
