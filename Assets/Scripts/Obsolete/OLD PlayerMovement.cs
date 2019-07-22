using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//FORMER PLAYER MOVEMENT SCRIPT (OBSOLETE)
//HERE FOR REFERENCE IN CASE I RUIN THE OTHER SCRIPT

public class OLDPlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float mouseSensitivity = 5.0f;
    //private bool isRotated;
    public float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {

        //isRotated = false;
        rb = GetComponent<Rigidbody>();

        //make cursor invisble upon start of game
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    void MovePlayer()
    {
        //working code
        var mousePos = Input.mousePosition;
        //Vector3 position = new Vector3(mousePos.x, mousePos.y, 10.0f);  // kinda works
        //Vector3 position = new Vector3(mousePos.x, mousePos.y, -10.0f); //result: shoots off player
        //Vector3 position = new Vector3(mousePos.x, mousePos.y, 15.0f);    //works a lil better
        Vector3 position = new Vector3(mousePos.x, mousePos.y, 30.0f);
        var wantedPos = Camera.main.ScreenToWorldPoint(position);
        transform.position = wantedPos;

    }

    void RotatePlayer()
    {

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, degreesPerSecond * Time.deltaTime);



        //float xAngle = 0.0f;
        //float yAngle = 0.0f;
        //float zAngle = 180.0f;
        //Vector3 desiredRotation = new Vector3(xAngle, yAngle, zAngle);

        //if (Input.GetMouseButtonDown(0) && isRotated == false)
        //{
        //transform.Rotate(desiredRotation);

        //isRotated = true;
        //Debug.Log("Turn Down");

        //}

        //else if(Input.GetMouseButtonDown(0) && isRotated == true)
        //{
        //  transform.Rotate(desiredRotation);


        //isRotated = false;

        //Debug.Log("Turn Up");
        //}



        float degreeOfRotation = Input.mouseScrollDelta.y;
        transform.Rotate(Vector3.forward, degreeOfRotation * scrollSpeed);
    }

    int FiniteElementMachine()
    {
        int caseNumber = 0;

        //CASE #1:  is still and not rotating

        //CASE #2:  is still and rotating down (pronation)

        //CASE #3:  is still and rotating up (supination)

        //CASE #4:  is moving and not rotating 

        //CASE #5: is moving and rotating down (pronation)

        //CASE #6: is moving and rotating up (supination)

        return caseNumber;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.attachedRigidbody)
        {
            other.attachedRigidbody.useGravity = false;
        }

    }
    private void OnTriggerExit(Collider other) //FIX ME use gravity when rotated
    {
        if (other.attachedRigidbody)
        {
            other.attachedRigidbody.useGravity = true;
        }
    }
}
