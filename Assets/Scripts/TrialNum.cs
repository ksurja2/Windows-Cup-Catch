using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrialNum : MonoBehaviour {

	public int ballCount;
	int trial = 1;
	public int resetNum = 20;

	public Text trialText;

	private MissedBall _floorData;
	private BallInGoal _goalData;

	// Use this for initialization
	void Start () {

		_floorData = GameObject.Find ("Floor").GetComponent<MissedBall> ();  
		_goalData = GameObject.Find ("Goal!").GetComponent<BallInGoal> (); 

		ballCount = 0;
		
	}


	//FIX ME

	// Update is called once per frame
	void Update () {

		if (ballCount < resetNum) {
			ballCount = _floorData.missedBallCount + _floorData.missedCatchCount + _goalData.numCaptured;
		}
			
		if (ballCount == resetNum) {
			ballCount = 0;
			trial += 1;
		}
			
		UpdateText ();


	}

	void UpdateText(){

		trialText.text = "TRIAL: " + trial;
	}
}
