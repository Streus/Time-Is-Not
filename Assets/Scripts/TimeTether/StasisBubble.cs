using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisBubble : MonoBehaviour 
{
	[Tooltip("If true, the bubble will be destroyed after a period of time determined by bubbleAliveTime")]
	public bool useBubbleAliveTimer; 

	[Header("Variables Affected by Timer")]
	[Tooltip("How long the bubble lasts")]
	public float bubbleAliveTime; 
	float bubbleAliveTimer;
	[Tooltip("Should the bubble shrink as it disappears? This has a functional effect.")]
	public bool shrinkWithTimer; 
	[Tooltip("Should the bubble fade as it disappears? This only has an aesthetic effect")]
	public bool fadeWithTimer; 

	// Temporary
	private Vector3 initScale; 

	SpriteRenderer spriteRend; 
	float maxBubbleAlpha; 

	ParticleSystem stasisParticles;

	// Use this for initialization
	void Start () 
	{
		bubbleAliveTimer = bubbleAliveTime; 
		initScale = transform.localScale; 
		spriteRend = GetComponent<SpriteRenderer>(); 
		if (spriteRend != null)
		{
			maxBubbleAlpha = spriteRend.color.a; 
		}
		stasisParticles = GetComponent<ParticleSystem>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (useBubbleAliveTimer && !GameManager.isPaused())
		{
			bubbleAliveTimer -= Time.deltaTime; 
			if (bubbleAliveTimer <= 0)
			{
				RemoveBubble(); 
			}

			// Temporary: Make the bubble get smaller over time
			if (shrinkWithTimer)
			{
				transform.localScale = new Vector3 ((bubbleAliveTimer / bubbleAliveTime) * initScale.x, (bubbleAliveTimer / bubbleAliveTime) * initScale.y, (bubbleAliveTimer / bubbleAliveTime) * initScale.z); 
			}

			// Temporary: Make the bubble fade over time
			if (fadeWithTimer)
			{
				if (spriteRend != null)
				{
					spriteRend.color = new Color (spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, (bubbleAliveTimer / bubbleAliveTime) * maxBubbleAlpha); 
				}
			}
		}
	}

	/// <summary>
	/// Tell the LevelStateMangager to remove this bubble
	/// </summary>
	public void RemoveBubble()
	{
		if (!LevelStateManager.removeStasisBubble(this))
		{
			DestroyBubble(); 
		}
	}

	/// <summary>
	/// Destroy this bubble. This should be called by the LevelStateManager
	/// </summary>
	public void DestroyBubble()
	{
		Destroy(gameObject); 
	}
}
