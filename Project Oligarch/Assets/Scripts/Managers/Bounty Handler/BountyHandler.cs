using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHandler : MonoBehaviour
{
	public GameObject[] BountyObjects;

	enum RiskLevel
	{
		Low,
		Medium,
		Extreme,
		Max
	}

	enum PlanetType
	{
		Decus,
		Olympia,
		Max
	}

	enum BountyType
	{
		Kill,
		Capture,
		Rescue,
		Max
	}

	class Bounty
	{
		public RiskLevel Risk;
		public PlanetType Planet;
		public BountyType Type;

		public Bounty(RiskLevel risk, PlanetType planet, BountyType type)
		{
			Risk = risk;
			Planet = planet;
			Type = type;

			CreateUniqueBountyDetails();
		}
		
		void CreateUniqueBountyDetails()
		{
			switch(Type)
			{
				case BountyType.Kill:
					break;

				case BountyType.Capture:
					break;

				case BountyType.Rescue:
					break;
			}
		}
	}

	private void Awake()
	{
		Bounty[] bounties = GenerateBounties();
		Debug.Log(bounties.Length);

		for (int i = 0; i <= bounties.Length - 1; i++)
		{
			Debug.Log("Bounties index: " + i);

			switch (bounties[i].Type)
			{
				case BountyType.Kill:
					BountyObjects[i].GetComponent<Renderer>().sharedMaterial.color = Color.red;
					continue;

				case BountyType.Capture:
					BountyObjects[i].GetComponent<Renderer>().sharedMaterial.color = Color.blue;
					continue;

				case BountyType.Rescue:
					BountyObjects[i].GetComponent<Renderer>().sharedMaterial.color = Color.green;
					continue;
			}
		}

		for (int i = bounties.Length; i <= (BountyObjects.Length - 1); i++)
		{
			Debug.Log("BountyObj index: " + i);
			BountyObjects[i].GetComponent<Renderer>().sharedMaterial.color = Color.white;
		}
	}

	static Bounty[] GenerateBounties()
	{
		UnityEngine.Random.InitState(System.DateTime.Now.Second);
		int numberOfBounties = Random.Range(2,6);

		Bounty[] Bounties = new Bounty[numberOfBounties];

		for (int i = 0; i < numberOfBounties; i++)
		{
			BountyType type = (BountyType)UnityEngine.Random.Range(0, (int)BountyType.Max);
			RiskLevel risk = (RiskLevel)UnityEngine.Random.Range(0, (int)RiskLevel.Max);
			PlanetType planet = (PlanetType)UnityEngine.Random.Range(0, (int)PlanetType.Max);

			Bounties[i] = new Bounty(risk, planet, type);
		}

		return Bounties;
	}
}
