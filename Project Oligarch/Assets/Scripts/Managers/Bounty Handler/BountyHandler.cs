using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class BountyHandler : MonoBehaviour
{
	//For debug purposes
	public Vector3[] HolotableBountyTransforms;

    class GeneratedBounty
	{
        public RiskLevel Risk;
        public PlanetType Planet;
        public BountyType Type;

		public GameObject AttachedObject;

		public GeneratedBounty(RiskLevel risk, PlanetType planet, BountyType type)
		{
			Risk = risk;
			Planet = planet;
			Type = type;

			GenerateUniqueBountyDetails();
		}

		void GenerateUniqueBountyDetails()
        {
            switch (Type)
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
		GeneratedBounty[] bounties = GenerateBounties();
		Debug.Log(bounties.Length);

		for (int i = 0; i <= bounties.Length - 1; i++)
		{
			Debug.Log("Bounties index: " + i);

			switch (bounties[i].Type)
			{
				case BountyType.Kill:
					continue;

				case BountyType.Capture:
					continue;

				case BountyType.Rescue:
					continue;
			}
		}
	}

	GeneratedBounty[] GenerateBounties()
	{
		UnityEngine.Random.InitState(System.DateTime.Now.Second);
		int numberOfBounties = Random.Range(2, 6);

		GeneratedBounty[] Bounties = new GeneratedBounty[numberOfBounties];

		for (int i = 0; i < numberOfBounties; i++)
		{
			BountyType type = (BountyType)UnityEngine.Random.Range(0, (int)BountyType.Max);
			RiskLevel risk = (RiskLevel)UnityEngine.Random.Range(0, (int)RiskLevel.Max);
			PlanetType planet = (PlanetType)UnityEngine.Random.Range(0, (int)PlanetType.Max);

			Bounties[i] = new GeneratedBounty(risk, planet, type);
		}

		return Bounties;
	}

	private void OnDrawGizmos()
	{
		Matrix4x4 translate = Matrix4x4.Translate(transform.position);
		foreach(Vector3 t in HolotableBountyTransforms)
		{
			Gizmos.DrawWireSphere(translate * t, 0.25f);
		}
	}
}
