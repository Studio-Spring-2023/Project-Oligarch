using UnityEngine;

public class RangerLoadout : Loadout
{
    public RangerLoadout(PlayerCore Player) : base(Player)
    {
        
    }

    public override void Primary()
    {
        Debug.Log("Ranger Primary");

        //Physics.Raycast(Player.transform.position, Player.transform.forward, 15);
    }

    public override void Secondary()
    {
        Debug.Log("Ranger Secondary");
    }

    public override void Special()
    {
        Debug.Log("Ranger Special");
    }

    public override void Ultimate()
    {
        Debug.Log("Ranger Ultimate");
    }

    protected override bool CanCastPrimary()
    {
        throw new System.NotImplementedException();
    }

    protected override bool CanCastSecondary()
    {
        throw new System.NotImplementedException();
    }

    protected override bool CanCastSpecial()
    {
        throw new System.NotImplementedException();
    }

    protected override bool CanCastUltimate()
    {
        throw new System.NotImplementedException();
    }
}
