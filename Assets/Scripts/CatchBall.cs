using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <summary> 
 * Attach this script to the Player gameobject.
 * This script allows ball caught by the player to travel with the player
 * by making the ball a child of the player. It also changes the tag
 * of the ball to indicate a player interaction has occured  before 
 * collision with the goal or floor.
</summary> */

public class CatchBall : MonoBehaviour
{
    public GameObject player;
	//public GameObject ball;

 
    //upon being caught, convert ball into child of player
	//https://answers.unity.com/questions/632792/how-to-make-an-object-a-child-of-another-object-sc.html 
    void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag ("Ball")) {
			other.transform.parent = player.transform;
		
			
			//Debug.Log ("Enter");
		}

    }

    //upon falling out of the player bucket, the ball is no longer a child
    void OnTriggerExit(Collider other)
    {
		if (other.CompareTag ("Ball")) {
			other.transform.parent = null;
			other.transform.tag = "Caught";

			//Debug.Log ("Exit");
		}

    }

	void Update(){ //reset ball to random spawn
		if (Input.GetMouseButtonDown (1)) {

			Debug.Log ("RC");

			Vector3 newPos = new Vector3 (Random.Range (SpawnBall.lowerSpawnX, SpawnBall.upperSpawnX),
				                 SpawnBall.height, Random.Range (SpawnBall.lowerSpawnZ, SpawnBall.upperSpawnZ));

			if (GameObject.FindGameObjectWithTag ("Ball")) {
				GameObject.FindGameObjectWithTag ("Ball").transform.position = newPos;
				GameObject.FindGameObjectWithTag ("Ball").GetComponent<Rigidbody> ().velocity = Vector3.zero; 
			}

			else if(GameObject.FindGameObjectWithTag("Caught")){
					
				GameObject.FindGameObjectWithTag("Caught").transform.position = newPos;
				GameObject.FindGameObjectWithTag("Caught").GetComponent<Rigidbody>().velocity = Vector3.zero;
				GameObject.FindGameObjectWithTag ("Caught").transform.tag = "Ball";

			}
		}

	}

		
}
