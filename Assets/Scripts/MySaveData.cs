using System.IO;
using System.Collections;
using System.Collections.Generic;
using BurtSharp.UnityInterface;
using BurtSharp.UnityInterface.ClassExtensions;
using UnityEngine;
using UnityEngine.UI;

public class MySaveData : MonoBehaviour {

	//misc.
	float theTime;
	private bool firstrun;
	public InputField mainInputField;

	//reference other scripts
	private MissedBall _floorData;
	private BallInGoal _goalData;
	private SimplePlayerController _playerData;
	private ConnectionSetter _robotStates;

	//strings & filepath
	public string path;
	private string subjname, subjname_last;
	private string FileStatusString;

	public Text FileStatus;

	//player data
	private float grav_gain0;
	private float PSFactor;
	private float flipangle;

	private Vector3 _currentRobotVelocity = Vector3.zero;

	//ball data
	private int score;
	private int caughtToFloor;
	private int missedToFloor;

	private Vector3 ballTouchdown; 

	//goal data
	private Vector3 goalPos;



	void Awake ()
	{
		_floorData = GameObject.Find ("floor").GetComponent<MissedBall> ();  
		_goalData = GameObject.Find ("Goal!").GetComponent<BallInGoal> (); 
		_playerData = GameObject.Find ("Player").GetComponent<SimplePlayerController> ();
		_robotStates = GameObject.Find ("ConnectionSetter").GetComponent<ConnectionSetter> ();

		string m_Path = Application.dataPath;
	}

	// Use this for initialization
	void Start () {
		mainInputField.text = "Blank_SubjId";

		subjname = mainInputField.text;
		firstrun = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {


		subjname = mainInputField.text;
		//Activate function when detected new filename
		mainInputField.onEndEdit.AddListener(SubjUpdated);

		//player data
		grav_gain0 = _playerData.grav_gain0;
		PSFactor = _playerData.PSFactor;
		flipangle = _playerData.flipangle;

		//ball data
		score = _goalData.numCaptured;
		caughtToFloor = _floorData.missedCatchCount;
		missedToFloor = _floorData.missedBallCount;

		//goal data
		goalPos = _goalData.MoveGoal();


		//FIX ME:  ADD LAST SUFFIX TO CHANGING DATA
		/*if (EA_gain != EA_gain_last) {
			SubjUpdated (subjname);

		} */

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		float q1 = transform.eulerAngles [0];
		float q2 = transform.eulerAngles [1];
		float q3 = transform.eulerAngles [2];

		//EA_gain_last = EA_gain;

		theTime = Time.time;

	}

	public void SubjUpdated(string text)
	{
		
	}
}
