using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{

    [SerializeField]
    List<GameObject> enableOnEnd = new List<GameObject>();
    [SerializeField]
    List<GameObject> disableOnEnd = new List<GameObject>();
    [SerializeField]
    float time;
    void Awake()
    {
        foreach (GameObject obj in enableOnEnd)
        {
            obj.SetActive(false);
        }
        StartCoroutine(cutsceneTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator cutsceneTimer()
    {
        yield return new WaitForSeconds(time);
        foreach(GameObject obj in enableOnEnd)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in disableOnEnd)
        {
            Destroy(obj);
            //obj.SetActive(false);
        }

    }
}
