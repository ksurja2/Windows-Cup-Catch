using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this script locks player movement into the transverse (x-z) plane and controls rotation via scroll wheel
public class PlayerMovement : MonoBehaviour
{
    //floats are initialized to placeholders and can be changed from the unity interface for optimal values
    public float mouseSensitivity = 5.0f;
    public float scrollSpeed = 5.0f;
    public GameObject player;

    public float upperXBound = 50f;
    public float lowerXBound = -50f;
    public float upperZBound = 60f;
    public float lowerZBound = -20f;

    // Start is called before the first frame update
    void Start()
    {

        //make cursor invisble upon start of game
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    void MovePlayer()
    {



        float planeY = 0;
        Transform draggingObject = transform;

        Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance; // the distance from the ray origin to the ray intersection of the plane
        if (plane.Raycast(ray, out distance))
        {
            draggingObject.position = ray.GetPoint(distance); // distance along the ray
        }

        OutOfBounds();

    }


    //FIX ME:  overly sensitive/jumpy
    void OutOfBounds()
    {
        float playerPositionX = player.transform.position.x;
        float playerPositionZ = player.transform.position.z;
        if (playerPositionX > upperXBound || playerPositionX < lowerXBound)
        {
            playerPositionX = Mathf.Clamp(transform.position.x, lowerXBound, upperXBound);
            player.transform.position = new Vector3(playerPositionX, 0, playerPositionZ);

        }

        if (playerPositionZ > upperZBound || playerPositionZ < lowerZBound)
        {
            playerPositionZ = Mathf.Clamp(transform.position.z, lowerZBound, upperZBound);
            player.transform.position = new Vector3(playerPositionX, 0, playerPositionZ);

        }
    }

    void RotatePlayer()
    {
        float degreeOfRotation = Input.mouseScrollDelta.y;
        transform.Rotate(Vector3.forward, degreeOfRotation * scrollSpeed);
    }


}