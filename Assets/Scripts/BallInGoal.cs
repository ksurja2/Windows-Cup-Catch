﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //for Text object


//This script destroys captured ball objects and increments the player's score
public class BallInGoal : MonoBehaviour
{
    public Text score;
    public Vector3 thisPos;


    //initialize counter for captured targets
    public int numCaptured;


    private void Start()
    {
        DisplayText();
        numCaptured = 0;
    }

    //Test Goal Movement
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        MoveGoal();
    //    }
    //}

    //if target enters Goal, destroy object and increment counter 
    //BallFSM, CASE #5
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Ball"))
        {
            //Destroy(other.gameObject);
            other.transform.position = new Vector3(Random.Range(SpawnBall.lowerSpawnX, SpawnBall.upperSpawnX),
                SpawnBall.height, Random.Range(SpawnBall.lowerSpawnZ, SpawnBall.upperSpawnZ));

            other.GetComponent<Rigidbody>().velocity = Vector3.zero; //Get Rigidbody and set velocity to (0f, 0f, 0f)

            numCaptured += 1;
        }

        MoveGoal();
        DisplayText();
        
    }

    void DisplayText()
    {
        score.text = "Score: " + numCaptured.ToString();
    }


    void MoveGoal()
    {
        Vector3 posOne = new Vector3(-40, -7.5f, -7);
        Vector3 posTwo = new Vector3(40, -7.5f, -7);
        Vector3 posThree = new Vector3(-40, -7.5f, 14);
        Vector3 posFour = new Vector3(40, -7.5f, 14);
        Vector3 posFive = new Vector3(0, -7.5f, 0);
        

        int posNumber = Random.Range(1, 5);
        //Debug.Log(posNumber);
        if (posNumber == 1  && transform.position != posOne)
        {
            transform.position = posOne;
            thisPos = posOne;
        }

        else if (posNumber == 2 && transform.position != posTwo)
        {
            transform.position = posTwo;
            thisPos = posTwo;
        }

        else if (posNumber == 3 && transform.position != posThree)
        {
            transform.position = posThree;
            thisPos = posThree;
        }

        else if (posNumber == 4 && transform.position != posFour)
        {
            transform.position = posFour;
            thisPos = posFour;
        }

        else
        {
            transform.position = posFive;
            thisPos = posFive;
        }
    }


}
