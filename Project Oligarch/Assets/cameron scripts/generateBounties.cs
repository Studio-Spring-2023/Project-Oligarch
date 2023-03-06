using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class generateBounties : MonoBehaviour
{
    public enum planets
    {
        Earth,
        Mars,
        Venus,
        Jupiter
    };
    public enum bountyType
    {

        Kill,
        Capture,
        Rescue
    };
  
    public enum basePayout
    {
        money1,
        money2,
         money3,
         money4
    };
    public enum bountyRisk
    {
        low,
        medium,
        extreme
    }
    
    //time
    public enum timeLimit
    {
        Poor,
        Average,
        Exceptional
    }
   
    //Enemydifficulty 
    public enum enemyDifficulty
    {
        stage1,
        stage2,
        stage3,
        stage4,
        stage5
    }
    string[,] risk = new string[2, 5];

    risk[0, 0] = "1 min";
    risk[0, 1] = "2 min";
    risk[0, 2] = "3 min";
    risk[0, 3] = "4 min";
    risk[0, 4] = "5 min";
    risk[0, 5] = "6 min";
    //--------------
    risk[1, 0] = "stage1"
    risk[1, 1] = "stage1"
    risk[1, 2] = "stage1"
    risk[1, 3] = "stage1"    
    risk[1, 4] = "stage1"
    risk[1, 5] = "stage1"



        public void Awake()//for testing if it works // no core mechanics allowe here.
    {
        
        //Debug.Log("planet type is " + (planets)Random.Range(0, 4));
        //Debug.Log("bounty type is " + (bountyType)Random.Range(0, 3));       
        //Debug.Log("base payout is " + (basePayout)Random.Range(0, 4));
        
       // bountyRisk risk = bountyRisk.low; //for testing if the swapping works
        
        if (risk == bountyRisk.low)
        {
            //        timeLimit.Exceptional <= 4f; ----wrote this before the time enum
            Debug.Log(timeLimit.Exceptional + " 4min cutoff");
            Debug.Log("--------");//spacing
            Debug.Log(enemyDifficulty.stage1);
        }
        else
        {
            Debug.Log("fail");
        }

        //public int[] payout = { 100, 200, 300, 500 }; ---multiplied by how fast complete,
        //tiers
        //extreme risk
        //stronger enemy tier
        //except..1-4min
        //avg...4-8min
        //poor...8min+
        //medium risk
        //except..1-5min
        //avg...5-10min
        //poor...10min+
        //low risk
        ////except..1-6min
        //avg...6-12min
        //poor...12min+

        //        //payout is calculated when the boss is killed (the timer stops then) payout = amount of coins on the reward multiplied by the tiers
        //exceptional = 1.5x
        //average = 1x
        //poor = .2x


        //public string[] subObjectives = { " fastest time", "dont get seen", "dont get hit", "find all loot" };

        //optional
        //kill 100 enemies
        //random item rewards
        //dont get hit
        // 50% more rewards
        //dont open shop
        //2x rewards and random chest
    }

    


}

//
//wyatt says:
//use enums for all the string parts
//use "get" and "switch" functions to fill said enums
//everything should be in a bounty class that is static, and can be called by other things
//
//the time it takes in the 2d array the lower value would be an arbitrary difficulty and then multiply around to find the formor and latter
//value
//  to then build the difficulty from the enemies and such






