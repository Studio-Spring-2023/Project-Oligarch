using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The global manager that handles the games state and executes certain state conditions when prompted.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    #region Handler Variables
    public static InputHandler PlayerInputHandler { get; private set; }
    public static UIHandler UIHandler { get; private set; }
    #endregion

    private void InstantiateManager()
    {
        if (GM != null)
            return;

        DontDestroyOnLoad(this);
        GM = this;
        PlayerInputHandler = new InputHandler();
		UIHandler = new UIHandler();
    }

    void Awake()
    {
		InstantiateManager();

		//Don't want left click to be registering as an input when we start in the main menu.
		PlayerInputHandler.DisableAbilityInputs();
	}

	void OnEnable()
	{
        PlayerInputHandler.OnEnable();
	}

    public void MenuStartButtonPressed()
    {
        UIHandler.MenuFunctions.StartGame();
    }

    public void AssignActiveBounty()
    {
        //Right now we're skipping a UI screen that would give us more details on a bounty
        //And just immediately assigning the bounty to the game manager so when we attempt
        //to launch the game, it takes us where we need to go

    }

	void Start()
    {
        
    }

    void Update()
    {
        
    }

    

	void OnDisable()
	{
        PlayerInputHandler.OnDisable();
	}
}
