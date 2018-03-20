﻿using System.Collections;
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

    //public AudioClip gameOver; // -- question : Do we have a sound for this?
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
                GlobalAudio.PlaySound(inst.playerWalking1, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
                break;
            case 1:
                GlobalAudio.PlaySound(inst.playerWalking2, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
                break;
            case 2:
                GlobalAudio.PlaySound(inst.playerWalking3, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
                break;
            case 3:
                GlobalAudio.PlaySound(inst.playerWalking4, UIManager.inst.mixer.FindMatchingGroups("SFX")[0]);
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
    public AudioClip doorMetalClose; // -- done : Location - Door script 
    public AudioClip doorMetalOpen; // -- done : Location - Door script 
    public AudioClip laserSecurityHum; // -- waiting
    public AudioClip laserDeathHum; // -- waiting
    public AudioClip floatingPlatform; // -- waiting
    #endregion

    #region Enemy Sounds

    public AudioClip hermitCrabDown; // -- waiting
    public AudioClip hermitCrabStand; // -- waiting
    public AudioClip hermitCrabMoving; // -- waiting
    public AudioClip hermitCrabField; // -- waiting
    public AudioClip hermitCrabIdleNoise; // -- waiting
    public AudioClip gulperEelMoving; // -- waiting
    public AudioClip hummingBirdMoving; // -- waiting
    public AudioClip hummingBirdSpotting; // -- waiting
    public AudioClip hummingBirdAttacking; // -- waiting

    #endregion


    //Ambient Sounds
    public AudioClip wireSparks; // -- waiting
    public AudioClip backgroundDrone; // -- waiting
    public AudioClip hum; // -- waiting
    public AudioClip electricHum; // -- waiting
    public AudioClip labMachines; // -- waiting
    public AudioClip lowElectricHum; // -- waiting
    public AudioClip computers; // -- waiting
}