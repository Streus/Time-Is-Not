using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    private float cooldownMax;
    private GameObject player;
    private Image dashBar;

	// Use this for initialization
	void Start ()
    {
        player = GameManager.GetPlayer();
        dashBar = this.GetComponent<Image>();
        dashBar.fillAmount = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        dashBar.fillAmount = 1 - player.GetComponent<Player>().getSelf().getAbility(1).cooldownPercentage();
    }
}
