using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrialNum : MonoBehaviour {

	public int ballCount;
	public int trial;
	public int resetNum = 2;

	int missedBalls;
	int caughtBalls;
	int goalBalls;

	public Text trialText;

	private MissedBall _floorData;
	private BallInGoal _goalData;
	public Text prompt;

	// Use this for initialization
	void Start () {

		_floorData = GameObject.Find ("Floor").GetComponent<MissedBall> ();  
		_goalData = GameObject.Find ("Goal!").GetComponent<BallInGoal> (); 


		trial = 1;
		ballCount = 0;
		
	}
		

	// Update is called once per frame
	void Update () {

		//if (ballCount < resetNum) {

		/*missedBalls = _floorData.missedBallCount;
		caughtBalls = _floorData.missedCatchCount;
		goalBalls = _goalData.numCaptured;
		ballCount = missedBalls + caughtBalls + goalBalls;
		//Debug.Log (missedBalls);
		//Debug.Log (caughtBalls);
		//Debug.Log (goalBalls);
		Debug.Log ("BallCount: " + ballCount);
		//}*/
			

		UpdateTrialNum ();
		UpdateText ();


			if(Input.GetKeyDown("space") && prompt.enabled){
				Time.timeScale = 1.0f;
				prompt.enabled = false;
				}


	} 

	void UpdateText(){

		trialText.text = "TRIAL: " + trial;
	}

	void UpdateTrialNum(){
		missedBalls = _floorData.missedBallCount;
		caughtBalls = _floorData.missedCatchCount;
		goalBalls = _goalData.numCaptured;


		if (ballCount == resetNum) {
			ballCount = 0;
			missedBalls = 0;
			caughtBalls = 0;
			goalBalls = 0;
			if (ballCount == 0 && missedBalls == 0 && goalBalls == 0) {
				trial += 1;
				prompt.enabled = true;
				Time.timeScale = 0;


			}
			Debug.Log ("NEW TRIAL");
		
		} else {
			ballCount = missedBalls + caughtBalls + goalBalls;

		}

		//Debug.Log ("BallCount: " + ballCount);


	}
}
