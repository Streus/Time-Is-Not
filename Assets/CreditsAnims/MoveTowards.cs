using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour {
    public Transform StartPos;
    public Transform End; 
    public float speed; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, End.position, speed * Time.deltaTime);
	}
}
