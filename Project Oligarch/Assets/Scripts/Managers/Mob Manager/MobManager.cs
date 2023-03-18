using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldStateKeys
{

}

public class MobManager : MonoBehaviour
{
	private static HashSet<KeyValuePair<string, object>> WorldProperties;

	private void Awake()
	{
		GenerateWorldProperties();
	}

	private void GenerateWorldProperties()
	{
		WorldProperties = new HashSet<KeyValuePair<string, object>>();

		WorldProperties.Add(new KeyValuePair<string, object>("playerAlive", true));
	}

	public static void UpdateWorldProperties()
	{

	}

	public static void AddWorldProperty()
	{

	}

	public static void RemoveWorldProperty()
	{

	}

	public static HashSet<KeyValuePair<string, object>> GetWorldProperties()
	{
		return WorldProperties;
	}
}
