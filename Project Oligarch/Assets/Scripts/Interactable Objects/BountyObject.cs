using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyObject : Interactable
{
    public override void InteractedWith()
    {
        Debug.Log("Touched Bounty Obj");
        //Tell the game manager that we touched a bounty and this is the one we want to pursue
    }
}
