using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class CodePickup : MonoBehaviour
{
	public CodeName codeName; 

	public Sprite[] codeSprites; 

	SpriteRenderer rend; 

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<Entity> () != null) 
		{
			if (col.CompareTag("Player"))
			{
				AddCode(); 
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
	}
}
