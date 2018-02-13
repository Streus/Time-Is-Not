using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Forks/Player/Targeting")]
public class PlayerInvokeTargeting : Fork
{
    public override bool check(Controller c)
    {
        return true;
    }
}
