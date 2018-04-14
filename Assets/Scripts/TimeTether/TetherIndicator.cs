using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TetherIndicator : MonoBehaviour 
{
	public int tetherIndex; 

	Transform moveParent; 
	Vector3 offsetFromMoveParent; 

	[Tooltip("How large is the radius that tests whether the tether point should attach to a moving platform?")]
	public float moveOverlapRadius = 0.1f; 
	[Tooltip("The origin position fromr which moveOverlapRadius is tested for overlapping moving platforms")]
	public GameObject platformAttachPoint; 

	[Tooltip("If true, the player can walk up to a tether point, see a prompt icon, and press e to remove the tether point")] 
	public bool allowRadiusRemoval; 
	public float removeRadius = 1; 
	bool playerInRemoveRadius; 

	[Tooltip("If true, using an input (right click) removes a tether point")] 
	public bool allowKeyRemoval; 
	public Collider2D clickCollider; 

	public GameObject tetherPointSprite; 
	public GameObject removePrompt; 

	[Header("Indicator Sprites")] 
	public SpriteRenderer goldSprite; 
	public SpriteRenderer silverSprite; 

	Animator spriteAnimator; 

	public GameObject returnObjPrefab; 
	public bool spawnReturnObj; 

	// Screenshot-only sprites
	[Header("Screenshot Sprites")] 
	[Tooltip("(Drag in) The sprite renderer for the arrow that should appear in the screenshot")]
	[SerializeField] SpriteRenderer screenshotArrow; 
	[Tooltip("(Drag in) The sprite renderer for a tether point that doesn't animate and only appears in the screenshot")]
	[SerializeField] SpriteRenderer screenshotTetherRend; 

	[SerializeField] Sprite screenshotGoldTether; 
	[SerializeField] Sprite screenshotSilverTether; 

	[SerializeField] ParticleSystem zoomParticles; 

	void OnEnable()
	{
		ScreenshotManager.inst.takeScreenshot += OnScreenshot; 
	}

	void OnDisable()
	{
		if (ScreenshotManager.inst != null)
			ScreenshotManager.inst.takeScreenshot -= OnScreenshot; 
	}

	// Use this for initialization
	void Start () 
	{
		if (removePrompt != null)
			removePrompt.SetActive(false);

		UpdateTetherSprite(); 

		screenshotArrow.enabled = false;
		screenshotTetherRend.enabled = false;

		CheckAttachParent(); 
	}
		
	
	// Update is called once per frame
	void Update () 
	{
		Collider2D[] hits;

		if (moveParent != null)
		{
			transform.position = moveParent.position + offsetFromMoveParent; 
		}


		/*
		 * Radius Tether point removal
		 * Possibly deprecated
		 */ 

		playerInRemoveRadius = false; 

		if (allowRadiusRemoval)
		{
			hits = Physics2D.OverlapCircleAll(tetherPointSprite.transform.position, removeRadius); 

			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].CompareTag("Player"))
				{
					playerInRemoveRadius = true; 
				}
			}
		}

		// Don't allow the remove prompt/action for the first tether point (when tetherIndex == 0)
		if (playerInRemoveRadius && tetherIndex != 0)
		{
			// Display the remove prompt
			if (removePrompt != null)
				removePrompt.SetActive(true); 

			// Test for remove action
			// TODO- might want to have this be a hold action
			if (PlayerControlManager.GetKeyDown(ControlInput.INTERACT))
			{
				TetherManager.inst.RemoveTetherPoint(tetherIndex); 
			}
		}
		else
		{
			if (removePrompt != null)
				removePrompt.SetActive(false); 
		}

		/*
		 * Input (right click) Tether point removal
		 */ 
		//if (allowKeyRemoval && tetherIndex != 0 && MouseIsOver() && PlayerControlManager.GetKeyDown(ControlInput.FIRE_STASIS) && !LevelStateManager.CursorIsOverAStasisBubble())
		if (allowKeyRemoval && tetherIndex != 0 && MouseIsOver() && PlayerControlManager.GetKeyDown(ControlInput.REMOVAL))
		{
			TetherManager.inst.RemoveTetherPoint(tetherIndex); 
		}

		// Check for Destroy
		if (spriteAnimator != null && spriteAnimator.GetCurrentAnimatorStateInfo(0).IsName("WaitForDestroy"))
		{
			// TODO- spawn object that flies toward the player
			if (spawnReturnObj)
			{
				GameObject returnObj = (GameObject) Instantiate(returnObjPrefab, platformAttachPoint.transform.position, transform.rotation);
			}

			Destroy(this.gameObject); 
		}

		if (zoomParticles != null)
		{
			if (GameManager.inst.pauseType == PauseType.ZOOM)
			{
				zoomParticles.gameObject.SetActive(true); 
			}
			else
			{
				zoomParticles.gameObject.SetActive(false); 
			}
		}

	}

	void CheckAttachParent()
	{
		// Overlap circle all 
		// Check to see if any colliders are a moving platform (layer or tag or script)
		// Save a transform ref to the moving platform
		// Vector2 offset
		// Update position of point every frame to be moving platform position + offset
		Collider2D[] hits = Physics2D.OverlapCircleAll(platformAttachPoint.transform.position, moveOverlapRadius); 

		if (moveParent == null)
		{
			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].CompareTag("MovingPlatform"))
				{
					moveParent = hits[i].transform; 
					offsetFromMoveParent = transform.position - moveParent.transform.position; 
				}
			}
		}
	}

	public void UpdateTetherSprite()
	{
		if (tetherIndex == 0)
		{
			goldSprite.enabled = true; 
			silverSprite.enabled = false; 
			spriteAnimator = goldSprite.GetComponent<Animator>(); 
			screenshotTetherRend.sprite = screenshotGoldTether; 
		}
		else
		{
			goldSprite.enabled = false; 
			silverSprite.enabled = true; 
			spriteAnimator = silverSprite.GetComponent<Animator>(); 
			screenshotTetherRend.sprite = screenshotSilverTether; 
		}
	}

	// Called during the load process to update the position of any tether indicators with moveParents, whose RegisteredObjects have just sowed new data
	public void UpdatePosition()
	{
		if (moveParent != null)
		{
			transform.position = moveParent.position + offsetFromMoveParent; 
		}
	}

	/// <summary>
	/// Returns true if the mouse is hovering over this object
	/// </summary>
	public bool MouseIsOver()
	{
		if (GameManager.isPaused())
		{
			return false; 
		}

		Vector2 mouseWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

		if (clickCollider != null && clickCollider.bounds.Contains(mouseWorldPos) && tetherIndex != 0)
		{
			return true; 
		}

		return false; 
	}

	public void StartDestroy()
	{
		//goldSprite.GetComponent<Animator>().
		spriteAnimator.SetTrigger("TetherAnchorStop"); 
	}

	void OnScreenshot(bool startShot)
	{
		//Debug.Log("On Screenshot: " + startShot + "; tetherIndex: " + tetherIndex);

		// Only enable the arrow if it's the current tether
		if (LevelStateManager.curState == tetherIndex)
		{
			screenshotArrow.enabled = startShot; 
		}

		// For all sprites, change them to use a specific tether sprite
		screenshotTetherRend.enabled = startShot; 

		if (tetherIndex == 0)
		{
			goldSprite.enabled = !startShot; 
		}
		else
		{
			silverSprite.enabled = !startShot; 
		}
	}
}
