using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GulperEel : PatrollingEnemy
{

    private Animator gulperAnim;
    #region INSTANCE_METHODS
    public override void Awake()
    {
        base.Awake();

        Transform spriteChild = transform.Find("Sprite");
        if (spriteChild != null)
            gulperAnim = spriteChild.GetComponent<Animator>();
    }
    public override void Update ()
	{
		base.Update ();

        if (gulperAnim == null)
            return;

		gulperAnim.enabled = !GameManager.CheckPause ((int)PauseType.GAME | (int)PauseType.TETHER_MENU | (int)PauseType.TETHER_TRANSITION);

        //TODO gulper eel movement animations
        if (transform.eulerAngles.z > 315 || transform.eulerAngles.z < 45)
		{
            //up
            gulperAnim.SetInteger("Direction", 1);
        }
		if(transform.eulerAngles.z > 45 && transform.eulerAngles.z < 135)
		{
            //left
            gulperAnim.SetInteger("Direction", 4);
        }
		if(transform.eulerAngles.z > 135 && transform.eulerAngles.z < 225)
		{
            //down
            gulperAnim.SetInteger("Direction", 3);
        }
		if(transform.eulerAngles.z > 225 && transform.eulerAngles.z < 315)
		{
            //right
            gulperAnim.SetInteger("Direction", 2);
        }
	}

	#region ISAVABLE_METHODS

	public override SeedBase saveData ()
	{
		GSeed s = new GSeed (this);
		return s;
	}

	public override void loadData (SeedBase seed)
	{
		base.loadData (seed);
		GSeed h = (GSeed)seed;
		patrolStart = h.currentNode;
	}
	#endregion
	#endregion

	#region INTERNAL_TYPES

	private class GSeed : Seed
	{
		public PatrolNode currentNode;

		public GSeed(Controller c) : base(c)
		{
			GulperEel g = State.cast<GulperEel>(c);
			currentNode = g.patrolStart;
		}
	}
	#endregion
}
