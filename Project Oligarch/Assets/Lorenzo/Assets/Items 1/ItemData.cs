using UnityEngine;
using UnityEngine.UI;

public enum Item
{
	MRE,
	SyntheticWeave,
	TacGloves,
	SteelBoots,
	Supercharger,
	Bandoiler,
	Medkit,
	Trinket,
	Adrenaline,
	IonShielding,
	ArmorPlating,
	HiddenKnife,
	TeslaCoil,
	Soda,
	ThrusterPack,
	Cleaver,

	Max
}

public enum ItemType
{
	UniqueEffect,
	StatBuff,
}

[System.Flags]
public enum StatType
{
	HealthRegen = 1,
	MaxHealthPercent = 1 << 1,
	MaxHealthFlat = 1 << 2,
	AttackSpeed = 1 << 3,
	MovementSpeed = 1 << 4,
	Damage = 1 << 5,
	PercentMitigation = 1 << 6,
	FlatMitigation = 1 << 7,
	FlatMaxShield = 1 << 8,
	PercentMaxShield = 1 << 9,
	PercentCooldown = 1 << 10,
	FlatCooldown = 1 << 11,
	JumpCharge = 1 << 12,
	PercentLifeSteal = 1 << 13,
}

public enum Multiplier
{
	Linear,
	Exponential,
	Hyperbolic,
}

public enum Rarity
{
	Common,
	Uncommon,
	Rare,
	Legendary
}


[CreateAssetMenu(fileName = "Item Data", menuName = "Items/Item Data Template")]
public class ItemData : ScriptableObject
{
	public Item ItemName;
	public ItemType Type;
	public Rarity Rarity;
	public StatType Modifier;
	public float BaseStat;
	public float StackingStat;
	public Multiplier Multiplier;
	public Sprite DisplaySprite;
}
