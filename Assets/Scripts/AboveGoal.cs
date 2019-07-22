using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*****FIX ME*****

//This script changes the color of the goal to indicate when the player is in range
public class AboveGoal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
            Debug.Log("blue");

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;

        }
    }
}
