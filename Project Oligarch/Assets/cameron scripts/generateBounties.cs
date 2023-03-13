using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;
using UnityEngine.UI;

public class generateBounties : MonoBehaviour
{
    //planets
    public enum planets
    {
        Earth,
        Mars,
        Venus,
        Jupiter
       
    };
    //creating the drop down list
    public planets Planets;

    public enum bountyType
    {

        Kill,
        Capture,
        Rescue
    };
    //creating the drop down list
    public bountyType types;

    public enum basePayout
    {
        OneHundred =   100,
        TwoHundred =   200,
        ThreeHundred = 300,
        FourHundred =  400
    };
    //creating the drop down list
    public basePayout payoutBase;

    public enum bountyRisk
    {
        Low,
        Medium,
        Extreme
    }
    //creating the drop down list
    public bountyRisk risks;
    //---------depreceated but might be useful idk-----bountyRisk LRisk = bountyRisk.Low;    <---this functions and is close to what i want
  


    //time
    public enum timeLimit
    {
        Poor,
        Average,
        Exceptional
    }
    //creating the drop down list
    public timeLimit time;



    //Enemydifficulty 
    public enum enemyDifficulty
    {
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5
    }
    //creating the drop down list
    public enemyDifficulty EnemyDifficulty;

    //turning the base payouts into a method that can be multiplied later
    int scalePayout100 = (int)basePayout.OneHundred;
    int scalePayout200 = (int)basePayout.TwoHundred;
    int scalePayout300 = (int)basePayout.ThreeHundred;
    int scalePayout400 = (int)basePayout.FourHundred;
    //this is the method for bounties completed which is multiplied with the base payout (for late game money buff)
    int bountiesCompleted = 1; //to be increased by a later function


    //the line that creates the item used to randomize things later
    System.Random rnd = new System.Random();
    //the assignment of a new name to use for calling the random version of these enums
    private planets rndPlanets;
    private bountyType rndBounty;
    

    //text creations
        //creating the text element for the exceptional time limit 
    public Text exceptionalMathText;
        //creating the text element for the average time limit 
    public Text averageMathText;
        //creating the text element for the poor time limit 
    public Text poorMathText;

/*
    public Text exceptionalMathText;
    public Text exceptionalMathText;
    public Text exceptionalMathText;
*/  


    public void Start()//for testing if it works // no core mechanics allowe here.
    {
        
        //does the math for the scaling of the payout for the "exceptional" time limit
        int exceptionalMath = (int)(scalePayout100 * 1.5f * bountiesCompleted);
        exceptionalMathText.text = "Exceptional Time! 100*1.5 = " + exceptionalMath;

        //does the math for the average payout of the "average" time limit
        int averageMath = (int)(scalePayout100 * 1f * bountiesCompleted);
        Debug.Log("100 * 1 = " + averageMath);

        //does the math for the "poor" scaling payout time limit
        int poorMath = (int)(scalePayout100 * .5f * bountiesCompleted);
        Debug.Log("100 * .5 = " + poorMath);

        //the array that holds the time limits info in column 0, and the enemy difficulty stage in column 1
        string[,] risk = new string[2, 5];

        risk[0, 0] = "1 min";
        risk[0, 1] = "2 min";
        risk[0, 2] = "3 min";
        risk[0, 3] = "4 min";
        risk[0, 4] = "5 min";

        //--------------

        risk[1, 0] = "stage1";
        risk[1, 1] = "stage2";
        risk[1, 2] = "stage3";
        risk[1, 3] = "stage4";
        risk[1, 4] = "stage5";

        //makes random selections for the enums------------------------------
        rndPlanets = (planets)Random.Range(0, 4);
        rndBounty = (bountyType)Random.Range(0, 3);
    
     //testing the selection of the enums from the drop downs
        Debug.Log("the chosen bounty type is " + types);
        Debug.Log("the chosen base pay is " + payoutBase);
        


       
        //the function that checks what risk is selected and outputs a set of time limits and an enemy stage difficulty
        if (risks == bountyRisk.Low)
        {

            Debug.Log("the time limit is " + risk[0, 0] + "  The enemy difficulty is " + risk[1, 0]);

        }
        else if (risks == bountyRisk.Medium)
        {
            Debug.Log("the time limit is " + risk[0, 3] + "  The enemy difficulty is " + risk[1, 3]);
        }
        else if (risks == bountyRisk.Extreme)
        {
            Debug.Log("the time limit is " + risk[0, 4] + "  The enemy difficulty is " + risk[1, 4]);
        }
        else
        {
            Debug.Log("not a set difficulty, check code, broken");
        }

        //spacing for visuals 
        Debug.Log("==========================");


        //random elements test
        Debug.Log("random planets " + rndPlanets);

        Debug.Log("random bounty is " + rndBounty);
       
    }
}     
