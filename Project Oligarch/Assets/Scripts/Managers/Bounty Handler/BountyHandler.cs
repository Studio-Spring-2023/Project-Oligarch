using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Bounty Enums
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
	Snow,
	Magma,
    Max
}

enum BountyType
{
    Kill,
    Capture,
    Rescue,
    Max
}

enum BossType
{
	one,
	Max
}
#endregion

public class BountyHandler : MonoBehaviour
{
	public GameObject CockpitConsole;

	public GameObject BountyOrbPrefab;

	private BountyOrb ActiveBounty;

	[Range(0f, 1f)]
	public float RotationSmoothness;
	[Range(0.25f, 3.5f)]
	public float RotationIncrement;

	//This might not even be necessary
	[Range(1, 7)]
	public int MaxBountyCount;

	[Range(1f, 2.25f)]
	public float BountyOrbAnchorOffset;
	private Vector3 BountyOrbAnchorPos => (transform.up * BountyOrbAnchorOffset);

	public Quaternion BountyOrbOrientation;

	public Vector3[] BountyOrbPos;

	private static BountyOrb[] BountyOrbs;

#if UNITY_EDITOR
	//Debug Variables. Trying to use the actual orientations causes compiler errors
	[Range(0.25f, 3.5f)]
	public float DebugIncrement;
	public Quaternion DebugOrbOrientation;
	public bool DebugInstantiated;

#endif

	struct BountyOrb
	{
		public GameObject GameObject;
		public BountyObject Instance;
		public GeneratedBounty Bounty;
	}
	
	//This will probably become a struct
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

	//For Debug Purposes
	public void Awake()
	{
		BountyOrbOrientation = Quaternion.identity;

		PopulateHolotable();
	}

	private void OnEnable()
	{
		BountyObject.BountyInteracted += AssignActiveBounty;
	}

	private void AssignActiveBounty(BountyObject orbInstance)
	{
		foreach (BountyOrb orb in BountyOrbs)
		{
			if (orb.Instance == orbInstance)
			{
				if (ActiveBounty.Instance != null)
				{
					ActiveBounty.GameObject.SetActive(true);
				}

				ActiveBounty = orb;

				break;
			}
		}

		ActiveBounty.GameObject.SetActive(false);
		Instantiate(BountyOrbPrefab).transform.position = CockpitConsole.transform.position;
	}

	public void PopulateHolotable()
	{
		BountyOrbs = GenerateBountyOrbObjects(MaxBountyCount);
	}

	private BountyOrb[] GenerateBountyOrbObjects(int numberOfOrbs)
	{
		BountyOrb[] bountyOrbs = new BountyOrb[numberOfOrbs];

		for (int i = 0; i < numberOfOrbs; i++)
		{
			BountyOrb orb = new BountyOrb();

			orb.GameObject = Instantiate(BountyOrbPrefab);
			orb.GameObject.transform.parent = transform;
			orb.GameObject.transform.localPosition = BountyOrbAnchorPos + BountyOrbPos[i];

			orb.Instance = orb.GameObject.AddComponent<BountyObject>();

			orb.Bounty = GenerateBountyClass();

			MeshRenderer orbRenderer = orb.GameObject.GetComponent<MeshRenderer>();

			switch (orb.Bounty.Planet)
			{
				case PlanetType.Decus:
					orbRenderer.material = Resources.Load<Material>("Materials/Bounty Decus");
					break;

				case PlanetType.Snow:
					orbRenderer.material = Resources.Load<Material>("Materials/Bounty Snow");
					break;

				case PlanetType.Magma:
					orbRenderer.material = Resources.Load<Material>("Materials/Bounty Magma");
					break;
			}

			bountyOrbs[i] = orb;
		}

		return bountyOrbs;
	}

	private	GeneratedBounty GenerateBountyClass()
	{
		BountyType type = (BountyType)UnityEngine.Random.Range(0, (int)BountyType.Max);
		RiskLevel risk = (RiskLevel)UnityEngine.Random.Range(0, (int)RiskLevel.Max);
		PlanetType planet = (PlanetType)UnityEngine.Random.Range(0, (int)PlanetType.Max);
		return new GeneratedBounty(risk, planet, type);
	}

	public void Update()
	{
		Quaternion incrementalRotation = Quaternion.Euler(0, RotationIncrement * Time.deltaTime, 0);

		Quaternion desiredRotation = incrementalRotation * incrementalRotation;
		
		Quaternion slerpedRotation = Quaternion.Slerp(incrementalRotation, desiredRotation, RotationSmoothness);

		for (int i = 0; i < BountyOrbs.Length; i++)
		{
			BountyOrbs[i].GameObject.transform.localPosition = slerpedRotation * BountyOrbs[i].GameObject.transform.localPosition;
		}
	}

	private void OnDrawGizmos()
	{
		if (!DebugInstantiated)
		{
			DebugOrbOrientation = transform.rotation;
			DebugInstantiated = true;
		}

		Quaternion incrementalRotation = Quaternion.Euler(0, DebugIncrement * Time.deltaTime, 0);

		Quaternion desiredRotation = DebugOrbOrientation * incrementalRotation;

		Quaternion slerpedRotation = Quaternion.Slerp(DebugOrbOrientation, desiredRotation, RotationSmoothness);

		DebugOrbOrientation = slerpedRotation;

		Matrix4x4 BountyOrbMatrix = Matrix4x4.TRS(transform.position + BountyOrbAnchorPos, slerpedRotation, Vector3.one);

		Gizmos.color = Color.red;
		Vector3 xAxis = BountyOrbMatrix.GetColumn(0);
		Gizmos.DrawRay(transform.position + BountyOrbAnchorPos, xAxis.normalized);
		Gizmos.color = Color.green;
		Vector3 yAxis = BountyOrbMatrix.GetColumn(1);
		Gizmos.DrawRay(transform.position + BountyOrbAnchorPos, yAxis.normalized);
		Gizmos.color = Color.blue;
		Vector3 zAxis = BountyOrbMatrix.GetColumn(2);
		Gizmos.DrawRay(transform.position + BountyOrbAnchorPos, zAxis.normalized);

		Gizmos.color = Color.white;
		foreach (Vector3 t in BountyOrbPos)
		{
			Vector4 t4 = t;
			t4.w = 1;

			Gizmos.DrawWireSphere(BountyOrbMatrix * t4, 0.5f);
		}
	}
}
