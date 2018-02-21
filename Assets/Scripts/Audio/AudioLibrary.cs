using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : Singleton<AudioLibrary> 
{
    //This is for non spatial sounds only

#region UI Sounds
    // UI //

    public AudioClip tetherSelect; // -- waiting

    public static void PlayTetherSelect()
    {
        GlobalAudio.PlaySound(inst.tetherSelect);
    }

    public AudioClip tetherMenuOpen; // --  waiting

    public static void PlayTetherMenuOpen()
    {
        GlobalAudio.PlaySound(inst.tetherMenuOpen);
    }

    public AudioClip tetherMenuClose; // -- question : repeat open sound or new sound?

    public static void PlayTetherMenuClose()
    {
        GlobalAudio.PlaySound(inst.tetherMenuClose);
    }

    //public AudioClip gameOver; // -- question : Do we have a sound for this?
#endregion

#region Mechanic Sounds
    // Mechanics //

    public AudioClip tetherPlacement; // -- done : Location - TetherManager script

    public static void PlayTetherPlacementSound()
    {
        GlobalAudio.PlaySound(inst.tetherPlacement);
    }

    public AudioClip tetherRewind; // -- done : Location - TetherManager script

    public static void PlayTetherRewindSound()
    {
        GlobalAudio.PlaySound(inst.tetherRewind);
    }

    public AudioClip tetherError; // -- done : Location - TetherManger script

    public static void PlayTetherErrorSound()
    {
        GlobalAudio.PlaySound(inst.tetherError);
    }

    public AudioClip stasisShoot; // -- done : Location - StasisBullet script

    public static void PlayStasisShootSound()
    {
        GlobalAudio.PlaySound(inst.stasisShoot);
    }

    public AudioClip stasisError; // -- done : Location - LevelState Manager script

    public static void PlayStasisErrorSound()
    {
        GlobalAudio.PlaySound(inst.stasisError);
    }

    public AudioClip dashForward; // -- done : Location - PlayerTargeting script

    public static void PlayDashForwardSound()
    {
        GlobalAudio.PlaySound(inst.dashForward);
    }

    public AudioClip dashError; // -- waiting : question - Not sure where to apply

    public static void PlayDashErrorSound()
    {
        GlobalAudio.PlaySound(inst.dashError);
    }
    #endregion

#region Environment Sounds
    // Environment // 

    public AudioClip laserSecurityCollisiion; // -- done : Location - Laser script

    public static void PlayLaserSecurityCollisionSound()
    {
        GlobalAudio.PlaySound(inst.laserSecurityCollisiion);
    }

    public AudioClip laserDeathCollisiion; // -- done :  Location - laser script // PROBLEM WITH COLLISION AND TETHERING

    public static void PlayLaserDeathCollisionSound()
    {
        GlobalAudio.PlaySound(inst.laserDeathCollisiion);
    }

    public AudioClip alarmTriggered; //-- question - WIll it be constant or just a few moments?

    public AudioClip codePickup; // -- done : Location - CodePickup script

    public static void PlayCodePickupSound()
    {
        GlobalAudio.PlaySound(inst.codePickup);
    }

    public AudioClip normalSwitch; // -- done : ButtonSwitch Script

    public static void PlayNormalSwitchSound()
    {
        GlobalAudio.PlaySound(inst.normalSwitch);
    }
    #endregion

#region Character Sounds
    // Character //

    public AudioClip playerWalking; // -- questions : Need mltiple or change pitch?

    public static void PlayPlayerWalkingSound()
    {
        GlobalAudio.PlaySound(inst.playerWalking);
    }
    #endregion








    // Spatial sounds - could hold the clips here and reference them in the locations

    //pressurePlateOn // -- done : Location - pressure plate script
    //pressurePlateOff // -- doesn't exist but is probably needed : Location - pressure plate script - currently using pressureplateon sound
    //stasisHum // -- done : Location - StasisBubble script
    //codeDoorUnlock // -- done : Location - Keycode reader script
    //doorClosed // -- done : Location - Door script
    //doorOpen // -- done : Location - Door script 
    //pushBlockMoving // -- done : Location - PushBlock script - Problem 
    //laserSecurity // -- waiting : Location - Manually applied to audiosource on Security Laser Laser child in Prefab - PROBLEM
    //laserDeath // -- waiting : Location - Manually applied to audiosource on Death Laser Prefab - PROBLEM
    //dashRecharge // -- done : Location - DashBar script

    // Not yet implemented
    /*
    public AudioClip hermitCrabDigging; // -- waiting
    public AudioClip hermitCrabField; // -- waiting

    public AudioClip gulperEelMoving; // -- waiting
    public AudioClip gulperEelEating; // -- waiting

    public AudioClip hummingBirdMoving; // -- waiting
    public AudioClip hummingBirdSpotting; // -- waiting
    public AudioClip hummingBirdAttacking; // -- waiting

    public AudioClip wires; //-- question
    public AudioClip timedSwitch;
    */
}