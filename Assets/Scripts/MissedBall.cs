using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Destroy all instances of objects tagged "Ball" which pass through the floor
public class MissedBall : MonoBehaviour
{
    public int missedCount = 0;
    public Vector3 fallenPos;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            fallenPos = other.transform.position;
            //Destroy(other.gameObject);
            other.transform.position = new Vector3(Random.Range(SpawnBall.lowerSpawnX, SpawnBall.upperSpawnX),
                SpawnBall.height, Random.Range(SpawnBall.lowerSpawnZ, SpawnBall.upperSpawnZ));

            other.GetComponent<Rigidbody>().velocity = Vector3.zero; //Get Rigidbody and set velocity to (0f, 0f, 0f)

            missedCount += 1;
        }

    }
}
