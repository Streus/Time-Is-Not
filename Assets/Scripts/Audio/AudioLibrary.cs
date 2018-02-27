using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : Singleton<AudioLibrary> 
{
    //This is for non spatial sounds only

#region UI Sounds
    // UI //

    public AudioClip tetherSelect; // --  done : Location - TetherManager script

    public static void PlayTetherSelect()
    {
        GlobalAudio.PlaySound(inst.tetherSelect);
    }

    public AudioClip tetherMenuHover; // --  done : Location - TetherManager script

    public static void PlayTetherMenuHover()
    {
        GlobalAudio.PlaySound(inst.tetherMenuHover);
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

    public AudioClip dashForward; // -- done : Location - PlayerInvokeTargeting script

    public static void PlayDashForwardSound()
    {
        GlobalAudio.PlaySound(inst.dashForward);
    }

    public AudioClip dashError; // -- done : Location - PlayerInvokeTargeting script

    public static void PlayDashErrorSound()
    {
        GlobalAudio.PlaySound(inst.dashError);
    }

    public AudioClip dashRecharge; // -- done : Location - DashUIPanel script

    public static void PlayDashRechargeSound()
    {
        GlobalAudio.PlaySound(inst.dashRecharge);
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

    public AudioClip alarmTriggered; //-- question - Will it be constant or just a few moments and will it be location based?

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

    public AudioClip playerWalking1; // -- questions : Need mltiple or change pitch?
    public AudioClip playerWalking2;
    public AudioClip playerWalking3;
    public AudioClip playerWalking4;

    public static void RandomPlayerWalkingSound()
    {
        int sound = Random.Range(0, 4);
        switch(sound)
        {
            case 0:
                GlobalAudio.PlaySound(inst.playerWalking1);
                break;
            case 1:
                GlobalAudio.PlaySound(inst.playerWalking2);
                break;
            case 2:
                GlobalAudio.PlaySound(inst.playerWalking3);
                break;
            case 3:
                GlobalAudio.PlaySound(inst.playerWalking4);
                break;
        }
    }
    #endregion


    //This is for spatial sounds only

#region Mechanic Sounds

     
    public AudioClip stasisHum; // -- done : Location - StasisBubble script

    #endregion

#region Environment Sounds

    public AudioClip pushBlockMoving; // -- done : Location - PushBlock script - Problem - Stop and running into walls sound bad
    public AudioClip pressurePlateOn; // -- done : Location - pressure plate script
    public AudioClip pressurePlateOff; // -- doesn't exist but is probably needed : Location - pressure plate script - currently using pressureplateon sound
    public AudioClip codeDoorUnlock; // -- done : Location - Keycode reader script
    public AudioClip doorFieldClosed; // -- done : Location - Door script
    public AudioClip doorFieldOpen; // -- done : Location - Door script 
    public AudioClip doorMetalClose;
    public AudioClip doorMetalOpen;

    #endregion











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

        //laserSecurity // -- waiting : Location - Manually applied to audiosource on Security Laser Laser child in Prefab - PROBLEM
    //laserDeath // -- waiting : Location - Manually applied to audiosource on Death Laser Prefab - PROBLEM
    */
}