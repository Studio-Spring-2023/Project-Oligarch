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
		GenerateBaseWorldProperties();
	}

	#region World Property Functions
	private void GenerateBaseWorldProperties()
	{
		WorldProperties = new HashSet<KeyValuePair<string, object>>();

		WorldProperties.Add(new KeyValuePair<string, object>("playerAlive", true));
	}

	public static bool UpdateWorldProperty(KeyValuePair<string, object> propToUpdate)
	{
		if (WorldProperties.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(propToUpdate.Key); }) > 0)
		{
			return WorldProperties.Add(propToUpdate);
		}
		else return false;
	}

	public static bool AddWorldProperty(KeyValuePair<string, object> propToAdd)
	{
		return WorldProperties.Add(propToAdd);
	}

	public static bool RemoveWorldProperty(KeyValuePair<string, object> propToRemove)
	{
		return WorldProperties.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(propToRemove.Key); }) > 0;
	}

	public static bool GetWorldProperty(KeyValuePair<string, object> propToFind, out KeyValuePair<string, object> foundProp)
	{
		foundProp = default(KeyValuePair<string, object>);

		foreach (KeyValuePair<string, object> prop in WorldProperties)
		{
			if (prop.Key.Equals(propToFind.Key))
			{
				foundProp = prop;
				return true;
			}
		}

		return false;
	}

	public static HashSet<KeyValuePair<string, object>> GetMatchingWorldProperties(HashSet<KeyValuePair<string, object>> entityBeliefs)
	{
		HashSet<KeyValuePair<string, object>> matchingProperties = new HashSet<KeyValuePair<string, object>>();
		foreach (KeyValuePair<string, object> prop in WorldProperties)
		{
			foreach (KeyValuePair<string, object> belief in entityBeliefs)
			{
				if (prop.Key.Equals(belief.Key))
				{
					matchingProperties.Add(prop);
					break;
				}
			}
		}

		return matchingProperties;
	}

	public static HashSet<KeyValuePair<string, object>> GetAllWorldProperties()
	{
		return WorldProperties;
	}
	#endregion
}
