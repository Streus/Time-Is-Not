using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StasisUI : MonoBehaviour
{
	[SerializeField] private int stasisUiNum;
	void Update()
	{
		numStasis(stasisUiNum);
	}
	private void numStasis(int stasisUiNum)
	{
		if(LevelStateManager.numStasis >= stasisUiNum)
		{
			this.GetComponent<Image>().CrossFadeAlpha(0.15f, 0.25f, true);
		}
		else
		{
			this.GetComponent<Image>().CrossFadeAlpha(1.0f, 0.25f, true);
		}
	}
}