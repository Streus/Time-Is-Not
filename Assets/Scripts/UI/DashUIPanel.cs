using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DashUIPanel : Singleton<DashUIPanel> 
{
	[Header("References")]
	[Tooltip("(Drag In) The dash UI piece of the time tether that displays dash UI")]
	public GameObject dashPanel; 

	[Tooltip("(Drag In) The Image of the dash icon (not the backdrop)")]
	public Image dashImg; 

	[Header("Settings")]
	[Tooltip("The color of the dashImg when charging (partially transparent)")]
	public Color chargingSpriteColor;

	[Tooltip("The speed that the color of the dashImg changes")]
	public float colorLerpSpeed = 1; 

	[Tooltip("The speed that the fill of the dashImg changes. NOT the same as dash recharge speed.")]
	public float fillLerpSpeed = 1;

    bool charged = true;

	// Use this for initialization
	void Start () 
	{
		UpdateDashPanelActive(); 
	}

	// Update is called once per frame
	void Update () 
	{
		// TODO UpdateDashPanelActive() should probably be called when the dash ability is collected

		if (GameManager.dashIsCharged())
		{
			dashImg.color = Color.Lerp(dashImg.color, Color.white, colorLerpSpeed * Time.deltaTime);
            if (!charged)
            {
                if (!GlobalAudio.ClipIsPlaying(AudioLibrary.inst.dashRecharge))
                {
                    AudioLibrary.PlayDashRechargeSound();
                }
                charged = true;
            }
        }
		// If charging
		else
		{
			dashImg.color = Color.Lerp(dashImg.color, chargingSpriteColor, colorLerpSpeed * Time.deltaTime); 

			// TODO set image fill
			float fillTarget = 1 - GameManager.dashChargePercNormalized(); 

			dashImg.fillAmount = Mathf.Lerp(dashImg.fillAmount, fillTarget, fillLerpSpeed * Time.deltaTime);
            //dashImg.fillAmount = 1 - GameManager.dashChargePercNormalized(); 
            charged = false;
		}

		/*
		// Turn off dash image (make transparent) while dashing
		if (GameManager.isPlayerDashing())
		{
			dashImg.color = Color.Lerp(dashImg.color, Color.clear, 0.2f); 
		}
		// If charged
		else if (GameManager.dashIsCharged())
		{
			dashImg.color = Color.Lerp(dashImg.color, Color.white, 0.2f); 
		}
		// If charging
		else
		{
			dashImg.color = Color.Lerp(dashImg.color, chargingSpriteColor, 0.2f); 

			// TODO set image fill
		}
		*/ 
	}

	public void UpdateDashPanelActive()
	{
		if (GameManager.inst.canUseDash)
		{
			dashPanel.SetActive(true); 
		}
		else
		{
			dashPanel.SetActive(false); 
		}
	}
}
