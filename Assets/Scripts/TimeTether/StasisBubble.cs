using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisBubble : MonoBehaviour 
{
	[Tooltip("If true, the bubble will be destroyed after a period of time determined by bubbleAliveTime")]
	public bool useBubbleAliveTimer; 

	[Tooltip("If true, right clicking within the bubble collider bounds will destroy it.")]
	public bool canRightClickDestroy;

	// If this is a player stasis bubble, this index determines the order of the bubble, from 0 - 2. 
	// If -1, this is not a player stasis bubble
	public int stasisIndex = -1; 

	bool mouseIsOver; 

	[Header("Variables Affected by Timer")]
	[Tooltip("How long the bubble lasts")]
	public float bubbleAliveTime; 
	float bubbleAliveTimer;
	[Tooltip("Should the bubble shrink as it disappears? This has a functional effect.")]
	public bool shrinkWithTimer; 
	[Tooltip("Should the bubble fade as it disappears? This only has an aesthetic effect")]
	public bool fadeWithTimer; 

	[Tooltip("A gameobject to spawn that contains the effect to play when a stasis bubble is destroyed")]
	public GameObject bubblePopEffect; 

	// Temporary
	private Vector3 initScale; 

	private Collider2D bubbleCollider; 

	SpriteRenderer spriteRend; 
	float maxBubbleAlpha; 

	ParticleSystem stasisParticles;

    AudioSource source;

    AudioClip stasisForming;

    AudioClip stasisHum;

	// Use this for initialization
	void Start () 
	{
		bubbleAliveTimer = bubbleAliveTime; 
		initScale = transform.localScale; 
		bubbleCollider = GetComponent<Collider2D>(); 
		spriteRend = GetComponent<SpriteRenderer>(); 
		if (spriteRend != null)
		{
			maxBubbleAlpha = spriteRend.color.a; 
		}
		stasisParticles = GetComponent<ParticleSystem>();

        source = this.GetComponent<AudioSource>();
        stasisHum = AudioLibrary.inst.stasisHum;
        stasisForming = AudioLibrary.inst.stasisForming;

        if (source != null)
        {
            source.outputAudioMixerGroup = UIManager.inst.mixer.FindMatchingGroups("SFX")[0];
            source.clip = stasisForming;
            source.loop = false;
            source.Play();
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Test
		//Debug.Log("CursorIsOverATetherPoint: " + TetherManager.CursorIsOverATetherPoint() + "; CursorIsOverAStasisBubble: " + LevelStateManager.CursorIsOverAStasisBubble()); 
		

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

		if (canRightClickDestroy)
		{
			UpdateMouseIsOver(); 
		}

		// Temporary: You can right click to remove a stasis bubble
		if (canRightClickDestroy && !TetherManager.CursorIsOverATetherPoint())
		{
			if (MouseIsOver() && PlayerControlManager.GetKeyDown(ControlInput.REMOVAL))
			{
				RemoveBubble();
			}
		}

        if(source != null && !source.isPlaying)
        {
            source.loop = true;
            source.clip = stasisHum;
            source.Play();
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
        Instantiate(bubblePopEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

	}

	public bool ColliderContainsPos(Vector3 pos)
	{
		if (bubbleCollider.bounds.Contains(pos))
		{
			return true; 
		}

		return false; 
	}

	/// <summary>
	/// Returns true if the mouse is hovering over this bubble
	/// </summary>
	public bool MouseIsOver()
	{
		return mouseIsOver; 
	}

	void UpdateMouseIsOver()
	{
		mouseIsOver = false; 

		if (GameManager.isPaused())
		{
			return; 
		}

		Vector2 mouseWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

		if (bubbleCollider.bounds.Contains(mouseWorldPos))
		{
			// Check for a stasis bubble overlapping another stasis bubble
			Collider2D[] hits = Physics2D.OverlapCircleAll(bubbleCollider.bounds.center, bubbleCollider.bounds.extents.x); 

			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].GetComponent<StasisBubble>() != null)
				{
					StasisBubble curBubble = hits[i].GetComponent<StasisBubble>(); 
					if (curBubble.canRightClickDestroy)
					{
						// TODO- if this stasis bubble is below the hit stasis bubble, return false
						if (this.stasisIndex < curBubble.stasisIndex && curBubble.mouseIsOver)
						{
							return; 
						}
					}
				}
			}

			mouseIsOver = true; 
		}

		return; 
	}
}
