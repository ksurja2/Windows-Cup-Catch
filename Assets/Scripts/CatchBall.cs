using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allows balls caught by the player to travel with the player
public class CatchBall : MonoBehaviour
{
    public GameObject player;
 
    //upon being caught, convert ball into child of player
    void OnTriggerEnter(Collider other)
    {
        other.transform.parent = player.transform;
    }

    //upon falling out of the player bucket, the ball is no longer a child
    void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
