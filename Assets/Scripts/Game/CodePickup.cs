using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class CodePickup : MonoBehaviour
{
	public enum CodeName
	{
		CODE_1, 
		CODE_2,
		CODE_3,
		CODE_4,
		CODE_5,
		CODE_6,
		CODE_7
	};

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
		GameManager.AddCode(GetCodeName(codeName)); 
		gameObject.SetActive(false); 
	}

	// Kind of a hack way of making this easier for sophomores using an enum
	// TODO get rid of this later
	public string GetCodeName(CodeName code)
	{
		if (code == CodeName.CODE_1)
			return "1"; 
		else if (code == CodeName.CODE_2)
			return "2"; 
		else if (code == CodeName.CODE_3)
			return "3"; 
		else if (code == CodeName.CODE_4)
			return "4"; 
		else if (code == CodeName.CODE_5)
			return "5"; 
		else if (code == CodeName.CODE_6)
			return "6"; 
		else if (code == CodeName.CODE_7)
			return "7"; 
		return ""; 
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
