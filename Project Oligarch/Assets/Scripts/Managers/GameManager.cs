using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The global manager that handles the games state and executes certain state conditions when prompted.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static InputHandler PlayerInputHandler { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(this);

		PlayerInputHandler = new InputHandler();
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
