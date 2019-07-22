using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//referencing other scripts:  https://answers.unity.com/questions/451004/how-to-call-a-function-from-one-class-to-another-c.html 

//This script contains the case numbers for various states of both the ball and player components
public class FiniteStateMachine : MonoBehaviour
    {
    public GameObject player;
    public GameObject ball;
    public GameObject floor;
    public static int playerCase;
    public static int ballCase;
    // Start is called before the first frame update
    void Start()
    {
        //Find components by name
        player = GameObject.Find("Player");
        floor = GameObject.Find("Floor");
    }

    // Update is called once per frame
    void Update()
    {
        
        playerCase = PlayerFSM();
        ballCase = BallFSM();
    }

    //Finite State Machine for Player
    public int PlayerFSM()
    {
        int caseNumber = 0;

        //CASE #1:  is still and not rotating

        //CASE #2:  is still and rotating down (pronation)

        //CASE #3:  is still and rotating up (supination)

        //CASE #4:  is moving and not rotating 

        //CASE #5: is moving and rotating down (pronation)

        //CASE #6: is moving and rotating up (supination)

        Debug.Log("Player Case " + caseNumber); 
        return caseNumber;
        
    }

    //Finite State Machine for falling targets (ball)
    public int BallFSM()
    {
        float ballPosition = ball.transform.position[1]; //y-coordinate of ball
        float floorHeight = floor.transform.position[1]; //y-coordinate of floor
        int caseNumber = 0;

        //CASE #1 :  ball is falling
        if (ballPosition - floorHeight > 0.0001)
        {
            caseNumber = 1;
        }

        //CASE #2 :  ball is destroyed
        else if (ball == null)
        {
            caseNumber = 2;
        }

        //CASE #3 :  ball is in cup
        else if (ball.transform.root != transform.root)
        {
            caseNumber = 3;
        }
        //CASE #4 :  ball is rolling
        //else if ()
        //{

        //}
        //CASE #5 :  ball is in goal
        //else if ()
        //{

        //}

        Debug.Log("Target Case " + caseNumber);
        return caseNumber;
    }
}


