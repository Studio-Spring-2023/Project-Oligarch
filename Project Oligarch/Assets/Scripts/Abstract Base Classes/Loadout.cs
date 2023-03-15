public enum LoadoutType
{
    Ranger,
    Fighter
}

/// <summary>
/// An abstract class that contains all the basic functions for a unique loadout.
/// All Loadouts inherit from this class.
/// </summary>
public abstract class Loadout
{
    public PlayerCore Player;

    public Loadout(PlayerCore Player)
    {
        this.Player = Player;
    }

    protected float PrimaryCooldown;
    protected float SeocndaryCooldown;
    protected float SpecialCooldown;
    protected float UltimateCooldown;

    public abstract void Primary();
    protected abstract bool CanCastPrimary();

    public abstract void Secondary();
    protected abstract bool CanCastSecondary();

    public abstract void Special();
    protected abstract bool CanCastSpecial();

    public abstract void Ultimate();
    protected abstract bool CanCastUltimate();
}
