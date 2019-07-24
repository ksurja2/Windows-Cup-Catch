using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//attach this code to main camera
//This script spawns the falling target within a workspace and defines workspace boundaries
public class SpawnBall : MonoBehaviour
{
    public float delay = 1.0f;   //change delay
    public GameObject ball; //access this object through unity

    //perimeter of spawn points
    public static float lowerSpawnX = -30; //former vals:  -35 to 35 for x, -16 to 16 for z
    public static float upperSpawnX = 30;
    public static float lowerSpawnZ = -10;
    public static float upperSpawnZ = 10;
    public static float height = 50;

    // Start is called before the first frame update
    void Start()
    {
        //calls method "Method", start time, and repeating time
        //InvokeRepeating("Spawn", delay, delay);
        Spawn();

    }


    public void Spawn()
    {
            //X-Z CODE
        Instantiate(ball, new Vector3(Random.Range(lowerSpawnX, upperSpawnX), height, Random.Range(lowerSpawnZ, upperSpawnZ)), Quaternion.identity);
        
        //spawn only in varying x coordinates
        //Instantiate(smallBall, new Vector3(Random.Range(-35, 35), 50, -8), Quaternion.identity);
    }

}
