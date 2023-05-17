using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fovNudge : MonoBehaviour
{
    public Camera cam;
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        StartCoroutine(cameranudge());
    }

    // Update is called once per frame
    public IEnumerator cameranudge()
    {
        cam.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        cam.fieldOfView += 5;
        cam.gameObject.SetActive(true);
    }
}
