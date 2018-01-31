using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class KeyCodeUIElement : MonoBehaviour 
{
	public string keyCode; 
	public Image uiImage; 

	void OnEnable()
	{
		GameManager.inst.codesUpdated += UpdateCodeState; 
	}

	void OnDisable()
	{
		GameManager.inst.codesUpdated -= UpdateCodeState;
	}

	// Use this for initialization
	void Start () 
	{
		uiImage = GetComponent<Image>(); 
		UpdateCodeState(); 
	}

	void UpdateCodeState()
	{
		if (GameManager.HasCode(keyCode))
		{
			uiImage.enabled = true; 
		}
		else
		{
			uiImage.enabled = false; 
		}
	}
}
