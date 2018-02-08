using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : Singleton<PlayerControlManager>
{
    [SerializeField] private KeyCode rH_Up;
    public static KeyCode RH_Up
    {
        get
        {
            return inst.rH_Up;
        }
    }
    [SerializeField] private KeyCode rH_Down;
    public static KeyCode RH_Down
    {
        get
        {
            return inst.rH_Down;
        }
    }
    [SerializeField] private KeyCode rH_Left;
    public static KeyCode RH_Left
    {
        get
        {
            return inst.rH_Left;
        }
    }
    [SerializeField] private KeyCode rH_Right;
    public static KeyCode RH_Right
    {
        get
        {
            return inst.rH_Right;
        }
    }
    [SerializeField] private KeyCode rH_Interact;
    public static KeyCode RH_Interact
    {
        get
        {
            return inst.rH_Interact;
        }
    }
    [SerializeField] private KeyCode rH_DropTether;
    public static KeyCode RH_DropTether
    {
        get
        {
            return inst.rH_DropTether;
        }
    }
    [SerializeField] private KeyCode rH_TetherMenu;
    public static KeyCode RH_TetherMenu
    {
        get
        {
            return inst.rH_TetherMenu;
        }
    }
    [SerializeField] private KeyCode rH_Dash;
    public static KeyCode RH_Dash
    {
        get
        {
            return inst.rH_Dash;
        }
    }
    [SerializeField] private KeyCode rH_FireStasis;
    public static KeyCode RH_FireStasis
    {
        get
        {
            return inst.rH_FireStasis;
        }
    }
    [SerializeField] private KeyCode rH_ZoomOut;
    public static KeyCode RH_ZoomOut
    {
        get
        {
            return inst.rH_ZoomOut;
        }
    }

    [SerializeField] private KeyCode lH_Up;
    public static KeyCode LH_UP
    {
        get
        {
            return inst.lH_Up;
        }
    }
    [SerializeField] private KeyCode lH_Down;
    public static KeyCode LH_Down
    {
        get
        {
            return inst.lH_Down;
        }
    }
    [SerializeField] private KeyCode lH_Left;
    public static KeyCode LH_Left
    {
        get
        {
            return inst.lH_Left;
        }
    }
    [SerializeField] private KeyCode lH_Right;
    public static KeyCode LH_Right
    {
        get
        {
            return inst.lH_Right;
        }
    }
    [SerializeField] private KeyCode lH_Interact;
    public static KeyCode LH_Interact
    {
        get
        {
            return inst.lH_Interact;
        }
    }
    [SerializeField] private KeyCode lH_DropTether;
    public static KeyCode LH_DropTether
    {
        get
        {
            return inst.lH_DropTether;
        }
    }
    [SerializeField] private KeyCode lH_TetherMenu;
    public static KeyCode LH_TetherMenu
    {
        get
        {
            return inst.lH_TetherMenu;
        }
    }
    [SerializeField] private KeyCode lH_Dash;
    public static KeyCode LH_Dash
    {
        get
        {
            return inst.lH_Dash;
        }
    }
    [SerializeField] private KeyCode lH_FireStasis;
    public static KeyCode LH_FireStasis
    {
        get
        {
            return inst.lH_FireStasis;
        }
    }
    [SerializeField] private KeyCode lH_ZoomOut;
    public static KeyCode LH_ZoomOut
    {
        get
        {
            return inst.lH_ZoomOut;
        }
    }
}
