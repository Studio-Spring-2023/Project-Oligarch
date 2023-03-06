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
        UIHandler = GetComponent<UIHandler>();
    }

    void Awake()
    {
        InstantiateManager();
    }

	void OnEnable()
	{
        PlayerInputHandler.OnEnable();
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
