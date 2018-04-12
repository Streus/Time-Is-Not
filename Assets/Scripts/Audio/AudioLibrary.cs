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
        GlobalAudio.PlaySound(inst.tetherSelect, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip tetherMenuHover; // --  done : Location - TetherManager script

    public static void PlayTetherMenuHover()
    {
        GlobalAudio.PlaySound(inst.tetherMenuHover, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

#endregion

#region Mechanic Sounds
    // Mechanics //

    public AudioClip tetherPlacement; // -- done : Location - TetherManager script

    public static void PlayTetherPlacementSound()
    {
        GlobalAudio.PlaySound(inst.tetherPlacement, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip tetherRewind; // -- done : Location - TetherManager script

    public static void PlayTetherRewindSound()
    {
        GlobalAudio.PlaySound(inst.tetherRewind, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip tetherError; // -- done : Location - TetherManger script

    public static void PlayTetherErrorSound()
    {
        GlobalAudio.PlaySound(inst.tetherError, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip stasisShoot; // -- done : Location - StasisBullet script

    public static void PlayStasisShootSound()
    {
        GlobalAudio.PlaySound(inst.stasisShoot, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip stasisError; // -- done : Location - LevelState Manager script

    public static void PlayStasisErrorSound()
    {
        GlobalAudio.PlaySound(inst.stasisError, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip dashForward; // -- done : Location - PlayerInvokeTargeting script

    public static void PlayDashForwardSound()
    {
        GlobalAudio.PlaySound(inst.dashForward, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip dashError; // -- done : Location - PlayerInvokeTargeting script

    public static void PlayDashErrorSound()
    {
        GlobalAudio.PlaySound(inst.dashError, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip dashRecharge; // -- done : Location - DashUIPanel script

    public static void PlayDashRechargeSound()
    {
        GlobalAudio.PlaySound(inst.dashRecharge, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }
    #endregion

    #region Environment Sounds
    // Environment // 

    public AudioClip laserSecurityCollisiion; // -- done : Location - Laser script

    public static void PlayLaserSecurityCollisionSound()
    {
        GlobalAudio.PlaySound(inst.laserSecurityCollisiion, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip laserDeathCollisiion; // -- done :  Location - laser script // PROBLEM WITH COLLISION AND TETHERING

    public static void PlayLaserDeathCollisionSound()
    {
        GlobalAudio.PlaySound(inst.laserDeathCollisiion, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip codePickup; // -- done : Location - CodePickup script

    public static void PlayCodePickupSound()
    {
        GlobalAudio.PlaySound(inst.codePickup, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }

    public AudioClip normalSwitch; // -- done : ButtonSwitch Script

    public static void PlayNormalSwitchSound()
    {
        GlobalAudio.PlaySound(inst.normalSwitch, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }
    #endregion

    #region Character Sounds
    // Character //

    /*public AudioClip playerWalking;

    public static void PlayerWalking()
    {
        GlobalAudio.PlaySound(inst.playerWalking, 128, 0.0f, 1.0f, 1.25f, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
    }*/
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
    public AudioClip doorMetalClose; // -- done : Location - Door script 
    public AudioClip doorMetalOpen; // -- done : Location - Door script 
    public AudioClip laserSecurityHum; // -- done : Location - Laser script
    public AudioClip laserDeathHum; // -- done : Location - Laser script
    public AudioClip floatingPlatform; // -- done : Location - manual application
    #endregion

    #region Enemy Sounds

    public AudioClip hermitCrabDown; // -- Location : HCResetStandDur script
    public AudioClip hermitCrabStand; // -- Location : HCResetSitDur script
    public AudioClip hermitCrabMoving; // -- Location : HCResetReturnTimer script
    //public AudioClip hermitCrabField; // -- waiting
    //public AudioClip hermitCrabIdleNoise; // -- waiting
    public AudioClip gulperEelMoving; // -- Location : same as stasis
    public AudioClip hummingBirdMoving; // -- Location : DiscardPath script
    public AudioClip hummingBirdSpotting; // -- Location : HBEnterPursue script
    public AudioClip hummingBirdAttacking; // -- Location : HBPursue script
    #endregion

#region Ambient

    //Ambient Sounds
    public AudioClip wireSparks; // -- waiting
    public AudioClip backgroundDrone; // -- waiting
    public AudioClip hum; // -- waiting
    public AudioClip electricHum; // -- waiting
    public AudioClip labMachines; // -- waiting
    public AudioClip lowElectricHum; // -- waiting
    public AudioClip computers; // -- waiting
#endregion
}