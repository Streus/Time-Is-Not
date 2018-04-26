using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables; 

public class CredsNav : MonoBehaviour {
    public float Timer;
    public PlayableDirector myDirector;
    public GameObject[] DestroyThemAll; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Timer += Time.deltaTime; 
        if(myDirector.time > 55f) {
            for (int i = 0; i < DestroyThemAll.Length; i++){
                Destroy(DestroyThemAll[i]);
            }
            myDirector.Pause();

        
        }
	}

    public void LoadScene(string name){ 
        SceneManager.LoadScene(name);
    }
}
