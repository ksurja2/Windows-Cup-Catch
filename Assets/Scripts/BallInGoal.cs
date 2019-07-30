using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //for Text object


/*<summary> 
* Attach this script to the Goal! gameobject.
* This script increments the player's score when a "caught" ball is successfully
* placed in the goal. It then resets the ball prefab to a random spawn
* point upon collision. The goal then moves to a random spot within a radius,
* but after a set number of movements will return to its "home" position.
</summary> */


public class BallInGoal : MonoBehaviour
{
    public Text score;
	public float radiusOfWalk = 25.0f;
	private Vector3 homePos = new Vector3 (0.0f, -7.5f, 0.0f);

    public int numCaptured;
	public int numToHome = 7; //number of balls caught before resetting goal
	private int backToHome;


    private void Start()
    {
		backToHome = numToHome;
        DisplayText();
		numCaptured = 0;     //initialize counter for captured targets
    }

    //Test Goal Movement
    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            MoveGoal();
        }*/


		//reset goal to home
		if (Input.GetMouseButtonDown (1)) {
			transform.position = homePos;
		}

    }

    //if target enters Goal, destroy object and increment counter 
    //BallFSM, CASE #5
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Caught"))
        {
            //Destroy(other.gameObject);
            other.transform.position = new Vector3(Random.Range(SpawnBall.lowerSpawnX, SpawnBall.upperSpawnX),
                SpawnBall.height, Random.Range(SpawnBall.lowerSpawnZ, SpawnBall.upperSpawnZ));

            other.GetComponent<Rigidbody>().velocity = Vector3.zero; //Get Rigidbody and set velocity to (0f, 0f, 0f)

            numCaptured += 1;
			backToHome -= 1;
			other.transform.tag = "Ball"; //reset tag
        }

		if (backToHome == 0) {
			transform.position = homePos;
			backToHome = numToHome;  //return goal to center after certain amount of movement
		} else {
			MoveGoal ();
		}
        
		DisplayText();
        
    }

    void DisplayText()
    {
        score.text = "Score: " + numCaptured.ToString();
    }
		
	//create random walk of radius x around previous goal position (within workspace)
	public Vector3 MoveGoal()
    {

		//Vector2 newPosCoordinates = Random.insideUnitCircle * radiusOfWalk; //varying radii
		float directionAngle = OutOfBounds();
		Vector2 newPosCoordinates = new Vector2 (Mathf.Cos(directionAngle), Mathf.Sin(directionAngle)) * radiusOfWalk;
		float newXPos = newPosCoordinates [0];
		float newZPos = newPosCoordinates [1];

		Vector3 thisPos = transform.position; //get current position of goal
		Vector3 newPos = new Vector3 (newXPos, 0.0f, newZPos) + thisPos; //move to a point around current position
		transform.position = newPos;
		return newPos;

	
    }

	float OutOfBounds(){

		float xPos = transform.position.x;
		float zPos = transform.position.z;
		float minX = SpawnBall.lowerSpawnX; 
		float maxX = SpawnBall.upperSpawnX;
		float minZ = SpawnBall.lowerSpawnZ;
		float maxZ = SpawnBall.upperSpawnZ;

		float pi = Mathf.PI;
		float directionAngle = Random.Range(0, pi);

		//corner cases
		if (xPos > maxX && zPos > maxZ) { //top right
			directionAngle = Random.Range (pi, (3 * pi / 2));
		} else if (xPos > maxX && zPos < minZ) { //bottom right
			directionAngle = Random.Range (pi / 2, pi);
		} else if (xPos < minX && zPos > maxZ) { //top left
			directionAngle = Random.Range (0, -pi / 2);
		} else if (xPos < minX && zPos < minZ) { //bottom left
			directionAngle = Random.Range (0, pi / 2);
		} else {
			//edge cases
			if (xPos > maxX) { //right
				directionAngle = Random.Range (pi / 2, (3 * pi / 2));	
			} else if (xPos < minX) { //left
				directionAngle = Random.Range (-pi / 2, pi / 2);
			}
			if (zPos > maxZ) { //top
				directionAngle = Random.Range (-pi, 0);
			} else if (zPos > minZ) { //bottom
				directionAngle = Random.Range (0, pi);
			}
		}


		return directionAngle;
	} 

	/*Vector3 posOne = new Vector3(-40, -7.5f, -7);
        Vector3 posTwo = new Vector3(40, -7.5f, -7);
        Vector3 posThree = new Vector3(-40, -7.5f, 14);
        Vector3 posFour = new Vector3(40, -7.5f, 14);
        Vector3 posFive = new Vector3(0, -7.5f, 0);

        int posNumber = Random.Range(1, 5);
        //Debug.Log(posNumber);
        if (posNumber == 1  && transform.position != posOne)
        {
            transform.position = posOne;
        }

        else if (posNumber == 2 && transform.position != posTwo)
        {
            transform.position = posTwo;
        }

        else if (posNumber == 3 && transform.position != posThree)
        {
            transform.position = posThree;
        }

        else if (posNumber == 4 && transform.position != posFour)
        {
            transform.position = posFour;
        }

        else
        {
            transform.position = posFive;
        } */

}
