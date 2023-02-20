using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;

public class generateBounties : MonoBehaviour
{
    //
    // this is a little spagetti but i couldnt get them to nest, however i did in a round about way get them to randomize,
    // now i just need to bind these to an object and see if they work....maybe? 
    //
    //?DONE---------
    //! Which Planet they venture to.
    //! What type of Bounty it is.
    //! The risk of the bounty, dictating time limit and enemy difficulty.
    //!    ---time
    //!    ---enemy difficulty
    //! The payout at the end of the Bounty.
    //! ----- If there are sub-objectives that increase the end payout 

    public string[] planets = {"earth" , "mars" , "venus" , "jupiter"};
    
    public string[] bountyType = { "kill", "capture", "rescue" };

    public int[] bountyRisk = { 10, 20, 30, 50 };

    public string[] time = { "1min", "2min", "3min", "5min" };

    public string[] enemyDif = { "easy", "medium", "hard", "impossible" };

    public int[] payout = { 100, 200, 300, 500 };

    public string[] subObjectives = { " fastest time", "dont get seen", "dont get hit", "find all loot" };

    System.Random rnd = new System.Random();

        
    public void Awake()
    {
        
        int planetLength = planets.Length;
        int bountTypeLength = bountyType.Length;
        int bountRiskLength = bountyRisk.Length;
        int timeLenght = time.Length;
        int enDifLength = enemyDif.Length;
        int payLength = payout.Length;
        int subObjLength = subObjectives.Length;

        //! this grabs random array number (following max number of list elements) and then prints the number
       
        int rndPlanet = rnd.Next( 0, planetLength );
        int rndBType = rnd.Next( 0, bountTypeLength );
        int rndBRisk= rnd.Next( 0, bountRiskLength );
        int rndTime= rnd.Next( 0, timeLenght );
        int rndEDif= rnd.Next( 0, enDifLength );
        int rndPay= rnd.Next( 0, payLength );
        int rndSubObj= rnd.Next( 0, subObjLength );

        //! grabs the new number and throws it into the list 
        Debug.Log( "planet: " + planets[rndPlanet] );
        Debug.Log( "bounty type: " + bountyType[rndBType] );
        Debug.Log( "bounty risk: " + bountyRisk[rndBRisk] );
        Debug.Log( "time: " + time[rndTime] );
        Debug.Log( "difficulty: " + enemyDif[rndEDif] );
        Debug.Log( "payout: " + payout[rndPay] );
        Debug.Log( "sub objectives: " + subObjectives[rndSubObj] );

        Debug.Log( "-----------------------------" );

        
        //! prints the whole string finally
        foreach (var name in bountyRisk)
        {
            Debug.Log("bounty risk = " +  name );
        }
        
    }


    //!ideas: 
    //?a list for all the bounties
    //?a list of each bounty member with its qualites in it that are public variables
    //? how to make a list a child of another list but have them remain editable?
    //TODO ----way to make the debug work:
   
}
