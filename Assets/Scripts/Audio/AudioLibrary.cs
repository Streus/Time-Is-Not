using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : Singleton<AudioLibrary> 
{

    // UI //

	public AudioClip tetherSelect; // -- waiting
    public AudioClip tetherMenuOpen; // --  waiting
    public AudioClip tetherMenuClose; // -- waiting
    public AudioClip gameOver; // -- question

    // Mechanics //

    public AudioClip tetherPlacement; // -- done : Location - TetherManager script
    public AudioClip tetherRewind; // -- done : Location - TetherManager script
    public AudioClip tetherError; // -- done : Location - TetherManger script

    public AudioClip stasisShoot; // -- done : Location - StasisBullet script
    public AudioClip stasisHum; // -- done : Location - StasisBubble script
    public AudioClip stasisError; // -- done : Location - LevelState Manager script

    public AudioClip dashForward; // -- waiting
    public AudioClip dashRecharge; // -- waiting
    public AudioClip dashError; // -- waiting

    // Environment // 
    public AudioClip wires; //-- question
    public AudioClip pressurePlate; // -- done : Location - pressure plate script
    public AudioClip pressurePlateOff; // -- doesn't exist but is probably needed

    public AudioClip pushBlockMoving; // -- waiting

    public AudioClip normalSwitch;
    public AudioClip timedSwitch;

    public AudioClip laserSecurity; 
    public AudioClip laserSecurityCollisiion; // -- done : Location - Laser script

    public AudioClip laserDeath;
    public AudioClip laserDeathCollisiion; // -- done :  Location - laser script // PROBLEM

    public AudioClip alarmTriggered; //-- question

    public AudioClip codePickup; // -- done : Location - CodePickup script

    public AudioClip codeDoorUnlock; // -- done : Location - Keycode reader script
    public AudioClip doorClosed; // -- done : Location - Door script
    public AudioClip doorOpen; // -- done : Location - Door script


    // Character and Enemies //

    public AudioClip playerWalking; // -- questions

    public AudioClip hermitCrabDigging; // -- waiting
    public AudioClip hermitCrabField; // -- waiting

    public AudioClip gulperEelMoving; // -- waiting
    public AudioClip gulperEelEating; // -- waiting

    public AudioClip hummingBirdMoving; // -- waiting
    public AudioClip hummingBirdSpotting; // -- waiting
    public AudioClip hummingBirdAttacking; // -- waiting

    // Methods to play the audio clips // 

	public static void PlayTetherSelect()
	{
		GlobalAudio.PlaySound(inst.tetherSelect);
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

    public static void PlayStasisHumSound()
    {
        GlobalAudio.PlaySound(inst.stasisHum);
    }

    public static void PlayStasisErrorSound()
    {
        GlobalAudio.PlaySound(inst.stasisError);
    }

    public static void PlayDashForwardSound()
    {
        GlobalAudio.PlaySound(inst.dashForward);
    }

    public static void PlayDashRechargeSound()
    {
        GlobalAudio.PlaySound(inst.dashRecharge);
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

    public static void PlayLaserSecuritySound()
    {
        GlobalAudio.PlaySound(inst.laserSecurity);
    }

    public static void PlayLaserSecurityCollisionSound()
    {
        GlobalAudio.PlaySound(inst.laserSecurityCollisiion);
    }

    public static void PlayLaserDeathSound()
    {
        GlobalAudio.PlaySound(inst.laserDeath);
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

    public static void PlayPressurePlateSound()
    {
        GlobalAudio.PlaySound(inst.pressurePlate);
    }

    public static void PlayDoorOpenSound()
    {
        GlobalAudio.PlaySound(inst.doorOpen);
    }

    public static void PlayDoorClosedSound()
    {
        GlobalAudio.PlaySound(inst.doorClosed);
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

    public static void PlayHummingBirdSpottingSound()
    {
        GlobalAudio.PlaySound(inst.hummingBirdSpotting);
    }

    public static void PlayHummingBirdAttackingSound()
    {
        GlobalAudio.PlaySound(inst.hummingBirdAttacking);
    }
}