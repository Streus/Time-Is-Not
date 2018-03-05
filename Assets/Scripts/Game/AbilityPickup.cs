using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour 
{
	public enum PickupType
	{
		DASH,
		STASIS
	}; 
		
	[SerializeField] PickupType pickupType;

	// TODO
	// Consider triggering a cutscene after picking up the ability

	void Start () 
	{
		// Warnings for designers to avoid placing a pickup in the scene when the player already has the ability
		#if UNITY_EDITOR
		if (pickupType == PickupType.DASH && GameManager.inst.canUseDash)
		{
			Debug.LogWarning("There is a dash ability pickup in the scene, but canUseDash is already set true at the start of the scene."); 
		}
		else if (pickupType == PickupType.STASIS && GameManager.inst.canUseStasis)
		{
			Debug.LogWarning("There is a stasis ability pickup in the scene, but canUseStasis is already set true at the start of the scene."); 
		}
		#endif
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			switch (pickupType)
			{
				case PickupType.DASH:
					OnDashCollected(); 
					break; 
				case PickupType.STASIS:
					OnStasisCollected(); 
					break; 
			}

			// Temporary: Destroy this gameObject
			// TODO: Figure out animation for destroying this pickup
			Destroy(gameObject); 
		}
	}

	/*
	 * Ability collected functions
	 * These update the GameManager settings and should perform animations for the components being added
	 */ 
		
	void OnDashCollected()
	{
		Debug.Log("Dash ability collected"); 
		GameManager.inst.canUseDash = true; 
		DashUIPanel.inst.UpdateDashPanelActive(); 
	}

	void OnStasisCollected()
	{
		Debug.Log("Stasis ability collected");
		GameManager.inst.canUseStasis = true; 
		StasisUIPanel.inst.UpdateStasisPanelActive(); 
	}
}
