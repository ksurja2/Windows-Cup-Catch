using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Destroy all instances of objects tagged "Ball" which pass through the floor
public class MissedBall : MonoBehaviour
{
    int missedBallCount = 0;
	int missedCatchCount = 0;

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
