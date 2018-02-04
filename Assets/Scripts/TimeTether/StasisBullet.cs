using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisBullet : MonoBehaviour
{
	[SerializeField]
	private GameObject stasisBubblePref;

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float tolerance = 0.1f;

	private Vector3 targetPos;

	public static StasisBullet create(Vector3 position, Quaternion direction, Vector3 targetPos)
	{
		GameObject pref = Resources.Load<GameObject> ("Prefabs/StasisBullet");
		GameObject inst = Instantiate<GameObject> (pref, position, direction);
		StasisBullet sb = inst.GetComponent<StasisBullet> ();
		sb.targetPos = targetPos;
		return sb;
	}

	public void Awake()
	{
		GetComponent<Rigidbody2D> ().AddForce (transform.up * speed, ForceMode2D.Impulse);
		LevelStateManager.inst.stateLoaded += cleanUp;
	}

	public void Update()
	{
		GetComponent<Rigidbody2D> ().simulated = !GameManager.isPaused ();

		if (Vector3.Distance (transform.position, targetPos) < tolerance)
			OnTriggerEnter2D (null);
	}

	private void cleanUp(bool success)
	{
		if (success)
			Destroy (gameObject);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col != null && col.isTrigger)
			return;
		
		StasisBubble newStasis = Instantiate<GameObject>(stasisBubblePref, transform.position, transform.rotation).GetComponent<StasisBubble>(); 
		LevelStateManager.addStasisBubble(newStasis);
		LevelStateManager.inst.stateLoaded -= cleanUp;
		Destroy (gameObject);
	}
}
