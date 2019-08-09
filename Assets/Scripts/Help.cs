using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour {

	public GameObject HelpMenu;
	public bool isActive;
	public bool breaktime;
	public Text pause;
	private TrialNum _trial;
	private MySaveData _input;

	void Start(){
		_trial = GameObject.Find ("Main Camera").GetComponent<TrialNum> ();
		_input = GameObject.Find ("Main Camera").GetComponent<MySaveData > ();
	}


	public void OpenHelpMenu(){

		if(!_trial.prompt.enabled && !_input.mainInputField.enabled){

		if (HelpMenu != null) { //check if panel is already open

			isActive = HelpMenu.activeSelf; //set bool to menu status
			HelpMenu.SetActive (!isActive);
			Time.timeScale = 0.0f;
			pause.enabled = false;
			breaktime = true;
		}

		if (isActive) {
			//Time.timeScale = 1.0f;
			//breaktime = false;
			pause.enabled = true;
		}
		}
	}

}
