using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//COPY OF SmBallFall to attempt building a finite element machine

//attach this code to main camera
public class SmBallFallFSM : MonoBehaviour
{
    //public float delay = 1.0f;   //change delay
    public GameObject smallBall; //access this object through unity
    public float floorValue = 7.563957f;

    // Start is called before the first frame update
    void Start()
    {
        //calls method "Method", start time, and repeating time
        //InvokeRepeating("Spawn", delay, delay);

        //spawn first ball
        Spawn();

    }

    void Update()
    {
        if (FiniteStateMachine() == 1)
        {
            //do nothing
        }
        else if (FiniteStateMachine() == 2 || FiniteStateMachine() == 5)
        {
            Spawn();
        }
        else if (FiniteStateMachine() == 3)
        {
            //FIX ME: ball not jumping out ??
        }
        else if (FiniteStateMachine() == 4)
        {
            //do nothing
        }
        else
        {
            Debug.Log("Error:  Case Not Found");
        }
    }

    void Spawn()
    {
        //X-Z CODE
        //Instantiate(smallBall, new Vector3(Random.Range(-35, 35), 50, Random.Range(-20, 20)), Quaternion.identity);

        //spawn only in varying x coordinates
        Instantiate(smallBall, new Vector3(Random.Range(-35, 35), 50, -8), Quaternion.identity);


    }

    int FiniteStateMachine()
    {
        int caseNumber = 0;
        float ballPosition = smallBall.transform.position[2];  //get y coordinate of falling object
        Debug.Log(ballPosition);

        //CASE #1 :  ball is falling
        if (ballPosition - floorValue > 0.001)
        {
            caseNumber = 1;
        }

        //CASE #2 :  ball is destroyed
        else if (ballPosition - floorValue < 0.001)
        {
            caseNumber = 2;
        }
        //CASE #3 :  ball is in cup
        //else if ()
        //{

        //}
        //CASE #4 :  ball is rolling
        //else if ()
        //{

        //}
        //CASE #5 :  ball is captured
        //else if ()
        //{

        //}

        return caseNumber;
    }
}
