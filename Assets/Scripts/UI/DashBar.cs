using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    private float cooldownMax;
    private GameObject player;
    private Image dashBar;

    AudioSource source;

    AudioClip dashRecharge;

    bool playSound = true;

	// Use this for initialization
	void Start ()
    {
        player = GameManager.GetPlayer();
        dashBar = this.GetComponent<Image>();
        dashBar.fillAmount = 0;
        source = this.GetComponent<AudioSource>();
        dashRecharge = AudioLibrary.inst.dashRecharge;
	}
	
	// Update is called once per frame
	void Update ()
    {
        dashBar.fillAmount = 1 - player.GetComponent<Player>().getSelf().getAbility(1).cooldownPercentage();

        if(dashBar.fillAmount != 1 && playSound)
        {
            source.clip = dashRecharge;
            source.Play();
            playSound = false;
        }
        else if (dashBar.fillAmount == 1 && !playSound)
        {
            source.Stop();
            playSound = true;
        }
    }
}
