using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Player/Dashing")]
public class PlayerReachJumpTarget : Fork
{
    public override bool check(Controller c)
    {
        return true;
    }
}
