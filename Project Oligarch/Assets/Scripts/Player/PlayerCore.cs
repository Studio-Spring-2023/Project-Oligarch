using UnityEngine;

public enum PlayerStates
{
    Idle,
    Ability,
}
public class PlayerCore : Core
{
    public static Loadout AssignedLoadout { get; private set; }

    [Header("Finite State Machine Variables")]
    private FiniteStateMachine PlayerSM;

	//Temporary Debug Bools
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

    public void ConstructPlayerStateMachine()
    {
        if (PlayerSM != null)
            return;

        PlayerSM = new FiniteStateMachine();

        PlayerSM.AddState(new PlayerIdleState(PlayerSM, this));
		PlayerSM.AddState(new PlayerAbilityState(PlayerSM, this));

        PlayerSM.SwitchState(PlayerSM.States[(int)PlayerStates.Idle]);
	}

    public void Awake()
    {
        //Will be moved to the Game Manager so when the Player "starts" the game, this is constructed once and only once.
        ConstructPlayerStateMachine();

        //Temporary to avoid Null Reference errors
        AssignedLoadout = AssignLoadout(LoadoutType.Ranger);
	}

    private void Start()
    {
        
	}

    private void Update()
    {
        PlayerSM.CurrentStateUpdate();
    }

	private void FixedUpdate()
	{
		PlayerSM.CurrentStateFixedUpdate();
	}

	//Temporary for Debug Display of Abilities
	private void OnDrawGizmos()
    {
        if (castingPrimary)
        {

        }

        if (castingSecondary)
        {

        }

        if (castingSpecial)
        {

        }

        if (castingUltimate)
        {

        }
    }
}
