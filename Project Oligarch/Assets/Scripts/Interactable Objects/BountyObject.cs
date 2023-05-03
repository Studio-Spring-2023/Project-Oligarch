using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyObject : Interactable
{
    public string Planet;
    public static Action<BountyObject> BountyInteracted;
    public override void InteractedWith()
    {
        Debug.Log("Touched Bounty Obj");

        BountyInteracted?.Invoke(this);
    }
}
