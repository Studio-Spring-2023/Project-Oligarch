using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ItemInfo
{
	public Item type;
	public Multiplier scaling;
}

public class ItemManager : MonoBehaviour
{
	public static ItemManager Instance;

    public ItemData[] CommonItems;
	public ItemData[] UncommonItems;
	public ItemData[] RareItems;
	public ItemData[] LegendaryItems;

	public Transform OriginalInventoryPos;
	private float rightOff;
	public GameObject ImagePrefab;

	private Dictionary<ItemData, int> PlayerInventory;

	[Header("Stat Modifiers")]
	public float PercentAttackSpeedBonus;
	public float PercentDamageBonus;
	public float PercentLifeSteal;
	public float FlatHealthBonus;
	public float PercentHealthBonus;
	public float PercentMovespeedBonus;
	public float JumpChargeBonus;
	public float HealthRegenBonus;
	public float FlatDamageMitigation;
	public float PercentMitigationChance;
	public float FlatMaxShield;
	public float PercentMaxShieldBonus;
	public float FlatCooldownReduction;
	public float PercentCooldownReduction;

	private void Awake()
	{
		Instance = this;

		PlayerInventory = new Dictionary<ItemData, int>();

		ResetInventory();
	}

	#region Inventory Helper Functions
	public bool AddItemToInventory(ItemData itemToAdd)
	{
		PlayerInventory[itemToAdd] += 1;
		Debug.Log($"<color=green>[ItemManager]</color>: Successfully added {itemToAdd.ItemName}.");
		CalculateModifiers(itemToAdd);
		Vector3 inventoryPos = OriginalInventoryPos.position;
		inventoryPos.x += rightOff;
		Image inventoryDisplay = Instantiate(ImagePrefab, inventoryPos, Quaternion.identity).GetComponent<Image>();
		inventoryDisplay.sprite = itemToAdd.DisplaySprite;
		//Debug.Log(PrintInventory());

		return true;
	}

	public bool RemoveItemFromInventory(ItemData itemToRemove)
	{
		if (PlayerInventory[itemToRemove] == 0)
		{
			Debug.Log("<color=red>[ItemManager]</color>: Tried to remove an item when you had none in the first place.");
			return false;
		}
		else
		{
			Debug.Log($"<color=green>[ItemManager]</color>: Successfully removed {itemToRemove.ItemName}.");
			PlayerInventory[itemToRemove] -= 1;
			CalculateModifiers(itemToRemove);
		}

		//Debug.Log(PrintInventory());

		return true;
	}

	private float GetStatModifier(ItemData itemToRefresh)
	{
		if (PlayerInventory[itemToRefresh] == 0)
			return 0;

		if (PlayerInventory[itemToRefresh] == 1)
			return itemToRefresh.BaseStat;

		switch (itemToRefresh.Multiplier)
		{
			case Multiplier.Linear:
				return itemToRefresh.BaseStat + (itemToRefresh.StackingStat * PlayerInventory[itemToRefresh]);

			case Multiplier.Exponential:
				return itemToRefresh.BaseStat + (Mathf.Pow(1 + itemToRefresh.StackingStat, PlayerInventory[itemToRefresh] - 1) - 1);

			case Multiplier.Hyperbolic:
				Debug.Log("Hyperbolic not implemented");
				return 0;

			default:
				Debug.Log($"<color=red>[ItemManager]</color>: Reached default case when trying to refresh item modifiers. What type of multiplier did you pass?");
				return 0;
		}
	}

	private void CalculateModifiers(ItemData itemToCalculate)
	{
		float totalModifier = 0;

		foreach (KeyValuePair<ItemData, int> item in PlayerInventory)
			if (item.Key.Modifier == itemToCalculate.Modifier)
				totalModifier += GetStatModifier(item.Key);

		switch (itemToCalculate.Modifier)
		{
			case StatType.AttackSpeed:
				PercentAttackSpeedBonus = (1 + totalModifier);
				break;

			case StatType.Damage:
				PercentDamageBonus = (1 + totalModifier);
				break;

			case StatType.FlatCooldown:
				FlatCooldownReduction = totalModifier;
				break;

			case StatType.PercentCooldown:
				PercentCooldownReduction = (1 + totalModifier);
				break;

			case StatType.FlatMaxShield:
				FlatMaxShield = totalModifier;
				break;

			case StatType.PercentMaxShield:
				PercentMaxShieldBonus = (1 + totalModifier);
				break;

			case StatType.FlatMitigation:
				FlatDamageMitigation = totalModifier;
				break;

			case StatType.PercentMitigation:
				PercentMitigationChance = (1 + totalModifier);
				break;

			case StatType.HealthRegen:
				HealthRegenBonus = totalModifier;
				break;

			case StatType.JumpCharge:
				JumpChargeBonus = totalModifier;
				break;

			case StatType.MaxHealthFlat:
				FlatHealthBonus = totalModifier;
				break;

			case StatType.MaxHealthPercent:
				PercentHealthBonus = (1 + totalModifier);
				break;

			case StatType.MovementSpeed:
				PercentMovespeedBonus = (1 + totalModifier);
				break;

			case StatType.PercentLifeSteal:
				PercentLifeSteal = totalModifier;
				break;

			default:
				break;
		}

		Debug.Log($"<color=orange>[ItemManager]</color>: Updated item modifiers based on {itemToCalculate.ItemName}.");
		//Debug.Log(PrintModifiers());
	}

	public void ResetInventory()
	{
		PlayerInventory.Clear();

		for (int i = 0; i < CommonItems.Length; i++)
			PlayerInventory.Add(CommonItems[i], 0);

		for (int i = 0; i < UncommonItems.Length; i++)
			PlayerInventory.Add(UncommonItems[i], 0);

		for (int i = 0; i < RareItems.Length; i++)
			PlayerInventory.Add(RareItems[i], 0);

		for (int i = 0; i < LegendaryItems.Length; i++)
			PlayerInventory.Add(LegendaryItems[i], 0);

		Debug.Log("<color=red>[Reset Inventory]</color>: " + PrintInventory());
	}

	public string PrintInventory()
	{
		string s = "";
		foreach (KeyValuePair<ItemData, int> item in PlayerInventory)
		{
			s += $"{item.Key.ItemName}: {item.Value}; ";
		}

		return s;
	}

	public string PrintModifiers()
	{
		return $"PercentAttackSpeedBonus: {PercentAttackSpeedBonus}; PercentDamageBonus: {PercentDamageBonus}; PercentLifeSteal: {PercentLifeSteal}; " +
			$"FlatHealthBonus: {FlatHealthBonus}; PercentHealthBonus: {PercentHealthBonus}; PercentMovespeedBonus: {PercentMovespeedBonus}; " +
			$"JumpChargeBonus: {JumpChargeBonus}; HealthRegenBonus: {HealthRegenBonus}; FlatDamageMitigation: {FlatDamageMitigation}; " +
			$"PercentMitigationChance: {PercentMitigationChance}; FlatMaxShield: {FlatMaxShield}; PercentMaxShieldBonus: {PercentMaxShieldBonus}; " +
			$"FlatCooldownReduction: {FlatCooldownReduction}; PercentCooldownReduction: {PercentCooldownReduction};";
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
