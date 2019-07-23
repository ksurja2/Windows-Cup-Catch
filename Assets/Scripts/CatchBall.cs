using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allows balls caught by the player to travel with the player
public class CatchBall : MonoBehaviour
{
    public GameObject player;
	public GameObject ball;
	//private bool isInBucket;
 
    //upon being caught, convert ball into child of player
	//https://answers.unity.com/questions/632792/how-to-make-an-object-a-child-of-another-object-sc.html 
    void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag ("Ball")) {
			other.transform.parent = player.transform;
		
			
			Debug.Log ("Enter");
		}
		//isInBucket = true;

		//***ugly fix, please find something better***
		//other.attachedRigidbody.useGravity = false;
		//other.GetComponent<Rigidbody>().velocity = Vector3.zero;

		//***this super does not work***
		//Rigidbody m_Rigidbody = other.GetComponent<Rigidbody>();
		//This locks the RigidBody so that it does not move or rotate in the Z axis.
		//m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
    }

    //upon falling out of the player bucket, the ball is no longer a child
    void OnTriggerExit(Collider other)
    {
		if (other.CompareTag ("Ball")) {
			other.transform.parent = null;
			//other.attachedRigidbody.useGravity = true;
			Debug.Log ("Exit");
		}
		//isInBucket = false;
    }

	void Update(){

	}
}
