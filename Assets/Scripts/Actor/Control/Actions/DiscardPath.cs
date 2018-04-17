using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/General/DiscardPath")]
public class DiscardPath : Action
{
    
	public override void perform (Controller c)
	{
        

        c.discardPath();
    }
}
