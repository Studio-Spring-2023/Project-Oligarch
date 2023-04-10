using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaManager : MonoBehaviour
{

    public Dictionary<string, int> MetaDict = new Dictionary<string, int>()
    {
        {"Radar Capabilities", 0 },
        {"High Reputation", 0 },
        {"Satellite Pulse", 0 },
        {"Shop Stock", 0 },
        {"Coin Reserves", 0 },
        {"Underground Connections", 0 },
    };
    public List<int> metaInt = new List<int>(); 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
