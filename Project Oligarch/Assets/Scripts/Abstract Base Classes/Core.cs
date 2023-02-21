using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The core class that all entities within the world will inherit from. Inherits from Monobehavior. 
/// </summary>
public abstract class Core : MonoBehaviour
{
	[Header("Movement Variables")]
	public float MoveSpeed;

	[Header("Combat Variables")]
	public int Health;
    

}
