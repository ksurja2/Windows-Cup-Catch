using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* <summary>
 * Attach this script to the Main Camera.
 * This script pauses and resumes the game 
 * upon pressing the spacebar 
 </summary> */

public class PausePlay : MonoBehaviour {

	bool isPaused;
	public Text pause;


	// Use this for initialization
	void Start () {
		isPaused = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("space") && !isPaused) {
			Time.timeScale = 0.0f;
			isPaused = true;
			pause.enabled = true;
		} else if (Input.GetKeyDown ("space") && isPaused) {
			Time.timeScale = 1.0f;
			isPaused = false;
			pause.enabled = false;
		}
	}
}
