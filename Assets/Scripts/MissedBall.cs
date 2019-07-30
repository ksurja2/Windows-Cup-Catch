using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*<summary>
* Attach this script to the Floor game object
* Reset all instances of ball prefab which pass through the floor, and count number
* missed by player and caught by player 
* </summary>*/

public class MissedBall : MonoBehaviour
{
    public int missedBallCount = 0;
	public 	int missedCatchCount = 0;

    void OnTriggerEnter(Collider other)
    {

		//Destroy(other.gameObject);
		other.transform.position = new Vector3 (Random.Range (SpawnBall.lowerSpawnX, SpawnBall.upperSpawnX),
			SpawnBall.height, Random.Range (SpawnBall.lowerSpawnZ, SpawnBall.upperSpawnZ));
		other.GetComponent<Rigidbody> ().velocity = Vector3.zero; //Get Rigidbody and set velocity to (0f, 0f, 0f)

		
		if (other.gameObject.CompareTag ("Ball")) {

			missedBallCount += 1;

		} else if (other.gameObject.CompareTag ("Caught")) {
			missedCatchCount += 1;
			other.transform.tag = "Ball";
		}
    }
}
