using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO probably needs to be ISavable
public class StasisBullet : MonoBehaviour
{
	[SerializeField]
	private GameObject stasisBubblePref;

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float duration = 1f;

	public static StasisBullet create(Vector3 position, Quaternion direction)
	{
		GameObject pref = Resources.Load<GameObject> ("Prefabs/StasisBullet");
		GameObject inst = Instantiate<GameObject> (pref, position, direction);
		StasisBullet sb = inst.GetComponent<StasisBullet> ();
		return sb;
	}

	public void Awake()
	{
		GetComponent<Rigidbody2D> ().AddForce (transform.up * speed, ForceMode2D.Impulse);
		LevelStateManager.inst.stateLoaded += cleanUp;
	}

	public void Update()
	{
		duration -= Time.deltaTime;
		if (duration <= 0f)
			OnTriggerEnter2D (null);
	}

	private void cleanUp(bool success)
	{
		// TODO: this is broken
		if (success)
			Destroy (gameObject);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		StasisBubble newStasis = Instantiate<GameObject>(stasisBubblePref, transform.position, transform.rotation).GetComponent<StasisBubble>(); 
		LevelStateManager.addStasisBubble(newStasis); 
		Destroy (gameObject);
	}
}
