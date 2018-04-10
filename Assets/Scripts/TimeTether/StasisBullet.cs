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

	private Vector3 startPos;
	private Vector3 dir;
	private float travelDist;

	private bool hit = false;

	public static StasisBullet create(Vector3 position, Quaternion direction, Vector3 targetPos)
	{
		GameObject pref = Resources.Load<GameObject> ("Prefabs/StasisBullet");
		GameObject inst = Instantiate<GameObject> (pref, position, Quaternion.identity);
		inst.transform.GetChild (0).rotation = Quaternion.Euler(0f, 0f, direction.eulerAngles.z - 180f);
		StasisBullet sb = inst.GetComponent<StasisBullet> ();
		sb.travelDist = Vector2.Distance(targetPos, sb.transform.position);
		sb.startPos = sb.transform.position;
		sb.dir = (direction * Vector3.up).normalized;
		return sb;
	}

	public void Awake()
	{
        AudioLibrary.PlayStasisShootSound();
		LevelStateManager.inst.stateLoaded += cleanUp;
	}

	public void Update()
	{
		if (GameManager.CheckPause ((int)PauseType.GAME))
			return;
		
		if (Vector3.Distance (transform.position, startPos) > travelDist)
			doHit ();
		else
		{
			transform.Translate (dir * speed * Time.deltaTime);
		}
	}

	private void cleanUp(bool success)
	{
		hit = true;

		if (success)
		{
			LevelStateManager.inst.stateLoaded -= cleanUp;
			Destroy (gameObject);
		}
	}

	private void doHit()
	{
		StasisBubble newStasis = Instantiate<GameObject>(stasisBubblePref, transform.position, transform.rotation).GetComponent<StasisBubble>(); 
		LevelStateManager.addStasisBubble(newStasis);
		cleanUp (true);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (hit || (col != null && col.isTrigger))
			return;

		doHit ();
	}
}
