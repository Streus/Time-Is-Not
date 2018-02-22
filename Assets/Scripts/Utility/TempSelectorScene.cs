using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class TempSelectorScene : MonoBehaviour 
{
	public void LoadScene(string name)
	{
		if (SceneManager.GetSceneByName(name) != null)
		{
			SceneManager.LoadScene(name); 
		}
		else
		{
			Debug.LogError("Scene name " + name + " cannot be loaded"); 
		}
	}
}
