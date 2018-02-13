using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : Singleton<AudioLibrary> 
{
    // UI //

	public AudioClip uiButtonClick;
    public AudioClip tetherMenuOpen;
    public AudioClip tetherMenuClose;
    public AudioClip gameOver; // ---

    // Mechanics //

    public AudioClip tetherPlacement;
    public AudioClip tetherRewind;
    public AudioClip tetherError;

    public AudioClip stasisShoot;
    public AudioClip stasisCollision;
    public AudioClip stasisError;

    public AudioClip dashForward;
    public AudioClip dashError;

    // Environment // 

    public AudioClip pushBlockMoving;

    public AudioClip normalSwitch;
    public AudioClip timedSwitch;

    public AudioClip laserSecurityMoving;
    public AudioClip laserSecurityCollisiion;

    public AudioClip laserDeathMoving;
    public AudioClip laserDeathCollisiion;

    public AudioClip alarmTriggered; //---

    public AudioClip codePickup;

    public AudioClip codeDoorUnlock;
    public AudioClip DoorUnlock;

    // Character and Enemies //

    public AudioClip playerWalking;

    public AudioClip hermitCrabDigging;
    public AudioClip hermitCrabField;

    public AudioClip gulperEelMoving;
    public AudioClip gulperEelEating;

    public AudioClip hummingBirdMoving;
    public AudioClip hummingBirdAttacking;

    // Methods to play the audio clips // 

	public static void PlayUIButtonClick()
	{
		GlobalAudio.PlaySound(inst.uiButtonClick);
	}

    public static void PlayTetherMenuOpen()
    {
        GlobalAudio.PlaySound(inst.tetherMenuOpen);
    }

    public static void PlayTetherMenuClose()
    {
        GlobalAudio.PlaySound(inst.tetherMenuClose);
    }

    public static void PlayTetherPlacementSound()
    {
        GlobalAudio.PlaySound(inst.tetherPlacement);
    }

    public static void PlayTetherRewindSound()
    {
        GlobalAudio.PlaySound(inst.tetherRewind);
    }

    public static void PlayTetherErrorSound()
    {
        GlobalAudio.PlaySound(inst.tetherError);
    }

    public static void PlayStasisShootSound()
    {
        GlobalAudio.PlaySound(inst.stasisShoot);
    }

    public static void PlayStasisCollisionSound()
    {
        GlobalAudio.PlaySound(inst.stasisCollision);
    }

    public static void PlayStasisErrorSound()
    {
        GlobalAudio.PlaySound(inst.stasisError);
    }

    public static void PlayDashForwardSound()
    {
        GlobalAudio.PlaySound(inst.dashForward);
    }

    public static void PlayDashErrorSound()
    {
        GlobalAudio.PlaySound(inst.dashError);
    }

    public static void PlayPushBlockMovingSound()
    {
        GlobalAudio.PlaySound(inst.pushBlockMoving);
    }

    public static void PlayNormalSwitchSound()
    {
        GlobalAudio.PlaySound(inst.normalSwitch);
    }

    public static void PlayTimedSwitchSound()
    {
        GlobalAudio.PlaySound(inst.timedSwitch);
    }

    public static void PlayLaserSecurityMovingSound()
    {
        GlobalAudio.PlaySound(inst.laserSecurityMoving);
    }

    public static void PlayLaserSecurityCollisionSound()
    {
        GlobalAudio.PlaySound(inst.laserSecurityCollisiion);
    }

    public static void PlayLaserDeathMovingSound()
    {
        GlobalAudio.PlaySound(inst.laserDeathMoving);
    }

    public static void PlayLaserDeathCollisionSound()
    {
        GlobalAudio.PlaySound(inst.laserDeathCollisiion);
    }

    public static void PlayCodePickupSound()
    {
        GlobalAudio.PlaySound(inst.codePickup);
    }

    public static void PlayCodeDoorUnlockSound()
    {
        GlobalAudio.PlaySound(inst.codeDoorUnlock);
    }

    public static void PlayKeyDoorUnlockSound()
    {
        GlobalAudio.PlaySound(inst.DoorUnlock);
    }

    public static void PlayPlayerWalkingSound()
    {
        GlobalAudio.PlaySound(inst.playerWalking);
    }

    public static void PlayHermitCrabDiggingSound()
    {
        GlobalAudio.PlaySound(inst.hermitCrabDigging);
    }

    public static void PlayHermitCrabFieldSound()
    {
        GlobalAudio.PlaySound(inst.hermitCrabField);
    }

    public static void PlayGulperEelMovingSound()
    {
        GlobalAudio.PlaySound(inst.gulperEelMoving);
    }

    public static void PlayGulperEelEatingSound()
    {
        GlobalAudio.PlaySound(inst.gulperEelEating);
    }

    public static void PlayHummingBirdMovingSound()
    {
        GlobalAudio.PlaySound(inst.hummingBirdMoving);
    }

    public static void PlayHummingBirdAttackingSound()
    {
        GlobalAudio.PlaySound(inst.hummingBirdAttacking);
    }
}