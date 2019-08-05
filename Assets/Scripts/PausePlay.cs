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
	public bool breaktime;
	public Text pause;
	public Text Play;
	public GameObject HelpMenu;
	public InputField UserInput;

	// Use this for initialization
	void Start () {
		isPaused = true;
		Time.timeScale = 0.0f; //game is paused at start
 
	}
	
	// Update is called once per frame
	void Update () {

		if (!HelpMenu.activeSelf && UserInput.enabled == false) { //only do this if the help menu is closed
			

			if (Input.GetKeyDown ("space") && !isPaused) {
				Time.timeScale = 0.0f;
				isPaused = true;
				pause.enabled = true;
			} else if (Input.GetKeyDown ("space") && isPaused) {
				if (Play.enabled) {
					Play.enabled = false;
				}
				Time.timeScale = 1.0f;
				isPaused = false;
				pause.enabled = false;
			} else if (HelpMenu.activeSelf)
			{
				isPaused = false;
			}

			breaktime = isPaused;
		}
		}

}
