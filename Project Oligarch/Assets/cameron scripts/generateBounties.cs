using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;

public class generateBounties : MonoBehaviour
{
    public enum planets
    {
        
        earth,
        mars,
        venus,
        jupiter
    }
    public enum bountyType
    {

        Kill,
        Capture,
        Rescue
    }
    //public enum payout
    //{
    //    100,
    //    200,
    //    300,
    //    400
    //}




    public void Awake()
    {

        Debug.Log((planets)1);
               
        //public string[] planets = {"earth" , "mars" , "venus" , "jupiter"};

        //public string[] bountyType = { "kill", "capture", "rescue" };

        //public int[] bountyRisk = { 10, 20, 30, 50 };

        //public string[] time = { "1min", "2min", "3min", "5min" };

        //public string[] enemyDif = { "easy", "medium", "hard", "impossible" };

        //public int[] payout = { 100, 200, 300, 500 };

        //public string[] subObjectives = { " fastest time", "dont get seen", "dont get hit", "find all loot" };

    }

    


}

//
//wyatt says:
//use enums for all the string parts
//use "get" and "switch" functions to fill said enums
//everything should be in a bounty class that is static, and can be called by other things
//
//the time it takes in the 2d array the lower value would be an arbitrary difficulty and then multiply around to find the formor and latter value
//  to then build the difficulty from the enemies and such






