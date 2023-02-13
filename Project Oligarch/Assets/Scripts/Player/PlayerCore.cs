using UnityEngine;

public class PlayerCore : Core
{
    private static Loadout AssignedLoadout;

    //TODO: Temporary debug bools
    bool castingPrimary;
    bool castingSecondary;
    bool castingSpecial;
    bool castingUltimate;

    public Loadout AssignLoadout(LoadoutType SelectedLoadout) => SelectedLoadout switch
    {
        LoadoutType.Ranger => new RangerLoadout(this),
        LoadoutType.Fighter => new FighterLoadout(this),
        _ => throw new System.Exception("Invalid Loadout Enum Value")
    };

    public void Awake()
    {
        AssignedLoadout = AssignLoadout(LoadoutType.Ranger);
    }

    private void Start()
    {
        AssignedLoadout.Primary();

        AssignedLoadout.Secondary();

        AssignedLoadout.Special();

        AssignedLoadout.Ultimate();
    }

    //Temporary for Debug Display of Abilities
    private void OnDrawGizmos()
    {
        if (castingPrimary)
        {

        }
    }
}
