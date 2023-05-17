using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public List<PointOfIntrest> POIs = new List<PointOfIntrest>();

    public MissionWaypoint waypoint;

    public int currentPOI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (POIs[currentPOI].completed)
        {
            currentPOI += 1;
        }
        waypoint.target = POIs[currentPOI].transform;
    }
}
