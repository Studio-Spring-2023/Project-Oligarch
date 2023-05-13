using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The core class that all entities within the world will inherit from. Inherits from Monobehavior. 
/// </summary>
public abstract class Core : MonoBehaviour
{
	[Header("Movement Variables")]
	[Range(1f, 10f)]
	public static float MoveSpeed = 8f;
	[HideInInspector]
	public float StartSpeed;
	[Range(0f, 3f)]
	public float SprintSpeed;

	[Header("Combat Variables")]
	[Range(25, 100)]
	public static int Health = 100;

	[Range(0,100)]
	public int Shield;
    

}
