using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor.AddressableAssets.BuildReportVisualizer;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	public static ItemManager Instance;

    public ItemData[] CommonItems;
	public ItemData[] UncommonItems;
	public ItemData[] RareItems;
	public ItemData[] LegendaryItems;

	private Dictionary<Item, int> PlayerInventory;

	[Header("Stat Modifiers")]
	public float AttackSpeed;
	public float FlatHealth;
	public float PercentHealth;
	public float PercentMovespeed;
	public float JumpCharge;
	public float HealthRegen;
	public float FlatMitigation;
	public float PercentMitigation;
	public float PercentMaxShield;
	public float FlatMaxShield;
	public float PercentCooldown;
	public float FlatCooldown;

	private void Awake()
	{
		Instance = this;

		ResetInventory();

		PrintInventory();
	}

	#region Inventory Helper Functions
	public bool AddItemToInventory(Item itemToAdd)
	{
		PlayerInventory[itemToAdd] += 1;
		RefreshStatModifier(itemToAdd);

		return true;
	}

	public bool RemoveItemFromInventory(Item itemToRemove)
	{
		if (PlayerInventory[itemToRemove] == 0)
		{
			Debug.Log("<color=red[ItemManager]</color>: Tried to remove an item when you had none in the first place.");
			return false;
		}
		else
		{
			PlayerInventory[itemToRemove] -= 1;
			RefreshStatModifier(itemToRemove);
		}

		return true;
	}

	public void RefreshStatModifier(Item itemToRefresh)
	{
		switch (itemToRefresh)
		{
			case Item.Adrenaline:
				break;

			case Item.ArmorPlating:
				break;

			case Item.Bandoiler:
				break;

			case Item.Cleaver:
				break;

			case Item.HiddenKnife:
				break;

			case Item.IonShielding:
				break;

			case Item.Medkit:
				break;

			case Item.MRE:
				break;

			case Item.Soda:
				break;

			case Item.SteelBoots:
				break;

			case Item.Supercharger:
				break;

			case Item.SyntheticWeave:
				break;

			case Item.TacGloves:
				break;

			case Item.TeslaCoil:
				break;

			case Item.ThrusterPack:
				break;

			case Item.Trinket:
				break;

			default:
				Debug.Log($"<color=red>[ItemManager]</color>: Reached default case when trying to refresh item modifiers. Did you pass the wrong integer?");
				break;
		}		
	}

	public void ResetInventory()
	{
		PlayerInventory.Clear();

		for (int i = 0; i < (int)Item.Max; i++)
			PlayerInventory.Add((Item)i, 0);
	}

	public void PrintInventory()
	{
		string s = "";
		foreach (KeyValuePair<Item, int> item in PlayerInventory)
		{
			s += $"{item.Key}: {item.Value}; ";
		}

		Debug.Log(s);
	}
	#endregion

	#region Runtime Helper Functions
	public void OnEnemyHit()
	{

	}

	public void OnPlayerPrimary()
	{

	}

	public void OnPlayerHit()
	{

	}
	#endregion
}
