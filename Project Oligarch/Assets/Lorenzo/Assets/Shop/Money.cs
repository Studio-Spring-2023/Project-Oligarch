using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public int Credits;
    [SerializeField] TextMeshProUGUI Text;

    void Update()
    {
        Text.text = Credits.ToString();
    }


    // Update is called once per frame

}
