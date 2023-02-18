using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class generateBounties : MonoBehaviour
{
    //TODO---------
    //! Which Planet they venture to.
    //! What type of Bounty it is.
    //! The risk of the bounty, dictating time limit and enemy difficulty.
    //!    ---time
    //!    ---enemy difficulty
    //! The payout at the end of the Bounty.
    //! ----- If there are sub-objectives that increase the end payout 

    //public string Planet;
    //public string bountyType;
    //public string bountyRisk;
    
      //  public string time;
        //public string enemyDif;

    //public string payout;
    //public string subObjectives;

    public void bounties(){
 
    public string[] planets = {"earth" , "mars" , "venus" , "jupiter"};

    public string[] bountyType = {"kill" , "capture" , "rescue"};

    public int[] bountyRisk = {10 , 20 , 30 , 50};

    public string[] time = {"1min" , "2min" , "3min" , "5min"};
    
    public string[] enemyDif = {"easy" , "medium" , "hard" , "impossible"};
   
    public int[] payout = {100 , 200 , 300 , 500};

    public string[] subObjectives = {" fastest time" , "dont get seen" , "dont get hit" , "find all loot"};
   
    };
   
   public void update()
    { 
        Debug.Log(planets[1]);
        Debug.Log(bountyType[1]);
        Debug.Log(bountyRisk[1]);
        Debug.Log(time[1]);
        Debug.Log(enemyDif[1]);
        Debug.Log(payout[1]);
        Debug.Log(subObjectives[1]);

    }


    //!ideas: 
    //?a list for all the bounties
    //?a list of each bounty member with its qualites in it that are public variables
    //? how to make a list a child of another list but have them remain editable?
    //TODO ----way to make the debug work:
    // Debug.Log("Human = " +String.Join("",
    //new List<int>(aHuman)
    //         .ConvertAll(i => i.ToString())
    //         .ToArray()));
   //didnt work
   // public static void Bounties(string[] args)
  //  {
        // maybe ------// List[] bounty = { "alex", "Ben", "Chuck" };
        //var bounty = new List<string>();
     //   var bounty = new List<Bounty>()
      //  {
      //  new Bounty(){Id = 1, Name="Alex"},
      //  new Bounty(){Id = 2, Name="Ben"},
      //  new Bounty(){Id = 3, Name="Chuck"},                                
       // new Bounty(){Id = 4, Name="Dave"},
      //  };
        //bounty.Add("alex");
        //bounty.Add("ben");
        //bounty.Add("chuck");
        //bounty.Add(null);
        //var result = s in Bounty
        //    where s.Name =="Ben"
        //    select s;
       // Debug.Log(bounty.contains("Ben"));
  //  }
}
