using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrialNum : MonoBehaviour {

	public int ballCount;
	public int trial;
	public int resetNum = 10;

	int missedBalls;
	int caughtBalls;
	int goalBalls;

	public Text trialText;


	private MissedBall _floorData;
	private BallInGoal _goalData;
	private GameObject _ball;
	public Text prompt;
	//public Text calibrate;

	// Use this for initialization
	void Start () {

		_floorData = GameObject.Find ("Floor").GetComponent<MissedBall> ();  
		_goalData = GameObject.Find ("Goal!").GetComponent<BallInGoal> (); 
		_ball = GameObject.FindGameObjectWithTag ("Ball");

		trial = 1;
		ballCount = 0;

		prompt.enabled = false;
		//_ball.SetActive (false);
		
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

		/*if (trial == 2) {
			Time.timeScale = 1.0f;
			prompt.enabled = false;
			calibrate.enabled = true;
			_ball.SetActive(false); 

		} */

		if(Input.GetKeyDown("return") && prompt.enabled){
			//Time.timeScale = 1.0f;
			prompt.enabled = false;
			_ball.SetActive(true);
			}

		/*if (Input.GetKeyDown ("return") && calibrate.enabled) {
			calibrate.enabled = false;
			_ball.SetActive(true);
		} */




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
				_ball.SetActive (false);
				//Time.timeScale = 0;


			}
			Debug.Log ("NEW TRIAL");
		
		} else {
			ballCount = missedBalls + caughtBalls + goalBalls;

		}

		//Debug.Log ("BallCount: " + ballCount);


	}
}
