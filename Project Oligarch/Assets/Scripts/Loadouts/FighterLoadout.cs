using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterLoadout : Loadout
{
    public FighterLoadout(PlayerCore Player) : base(Player)
    {
        BaseAttackSpeed = 2;
        BaseHealth = 25;
		BaseJumpCharges = 1;
		BaseHealthRegen = 5;
	}

    public override void Primary()
    {
        Debug.Log("Fighter Primary");
    }

    protected override bool CanCastPrimary()
    {
        throw new System.NotImplementedException();
    }

    public override void Secondary()
    {
        Debug.Log("Fighter Secondary");
    }

    protected override bool CanCastSecondary()
    {
        throw new System.NotImplementedException();
    }

    public override void Special()
    {
        Debug.Log("Fighter Special");
    }

    protected override bool CanCastSpecial()
    {
        throw new System.NotImplementedException();
    }

    public override void Ultimate()
    {
        Debug.Log("Fighter Ultimate");
    }

    protected override bool CanCastUltimate()
    {
        throw new System.NotImplementedException();
    }
}
