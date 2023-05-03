using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmBounty : Interactable
{
	public string Planet;
	public override void InteractedWith()
	{
		SceneManager.LoadScene(Planet);
	}
}
