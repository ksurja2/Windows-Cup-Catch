using System.IO;
using System.Collections;
using System.Collections.Generic;
using BurtSharp.UnityInterface;
using BurtSharp.UnityInterface.ClassExtensions;
using UnityEngine;
using UnityEngine.UI;

public class MySaveData : MonoBehaviour {

	/*<summary>
	 * Attach this script to the player game object 
	 * </summary> */

	//misc.
	int fileNum = 1;
	float theTime;
	private bool firstrun;
	public InputField mainInputField;
	private PausePlay _pauseStatus;
	private Help _menuStatus;
	private bool breaktime; //is the game paused?
	private string EA_Phrase; //is error augmentation applied? AKA does PS != 1?

	//reference other scripts
	private MissedBall _floorData;
	private BallInGoal _goalData;
	private SimplePlayerController _playerData;
	private ConnectionSetter _robotStates;


	//strings & filepath
	public string path;
	private string subjname, subjname_last;
	private string FileStatusString;
	private string blankString;

	public Text FileStatus;
	public Text Prompt;
	public Text Play;



	//player data
	private float grav_gain0, grav_gain0_last;
	private float PSFactor, PSFactor_last;
	private float flipangle, flipangle_last;
	private float playerRotation;

	private string handedness;
	private Vector3 _currentRobotForces = Vector3.zero;
	private Vector3 _currentRobotVelocity = Vector3.zero;

	//ball data
	private int score;
	private int caughtToFloor;
	private int missedToFloor;

	private Vector3 ballTouchdown; 

	//goal data
	private Vector3 goalPos;

	private TrialNum _trial;

	void Awake ()
	{
		breaktime = true;
		_floorData = GameObject.Find ("Floor").GetComponent<MissedBall> ();  
		_goalData = GameObject.Find ("Goal!").GetComponent<BallInGoal> (); 
		_playerData = GameObject.Find ("Player").GetComponent<SimplePlayerController> ();
		_robotStates = GameObject.Find ("ConnectionSetter").GetComponent<ConnectionSetter> ();

		_menuStatus = GameObject.Find ("? Button").GetComponent<Help> ();
		_pauseStatus = GameObject.Find ("Main Camera").GetComponent<PausePlay> (); 

		_trial = GameObject.Find ("Main Camera").GetComponent<TrialNum> ();  

		string m_Path = Application.dataPath;
	}

	// Use this for initialization
	void Start () {
		blankString = "Blank_SubjId";
		mainInputField.text = blankString;

		//subjname = mainInputField.text;
		firstrun = true;
		handedness = _playerData.hand;
	}

	
	// Update is called once per frame
	void FixedUpdate () {

		if (_menuStatus.breaktime || _pauseStatus.breaktime || _trial.prompt.enabled) {
			breaktime = true;
		} else {
			breaktime = false;
		} 


		//Activate function when detected new filename
		mainInputField.onEndEdit.AddListener(SubjUpdated);

		//player data
		grav_gain0 = _playerData.grav_gain0;
		PSFactor = _playerData.PSFactor;
		flipangle = _playerData.flipangle;
		playerRotation = _playerData.transform.rotation.eulerAngles[2];
		playerRotation = Mathf.Abs (flipangle - playerRotation);

		//Debug.Log ("PLAYER ROTATION: " + playerRotation);

		//if (((subjname != "Blank_SubjId") || !string.IsNullOrEmpty(subjname) && (breaktime==false) && (firstrun==true))){

		if (mainInputField.enabled == false){
			//mainInputField.ActivateInputField();
			SubjUpdated (subjname);
			firstrun = false;

		}  


		//ball data
		score = _goalData.numCaptured;
		caughtToFloor = _floorData.missedCatchCount;
		missedToFloor = _floorData.missedBallCount;

		ballTouchdown = _floorData.fallenPos;

		//goal data
		goalPos = _goalData.thisPos;


		if (grav_gain0 != grav_gain0_last) { 
			SubjUpdated (subjname);
		}

		if (PSFactor != PSFactor_last) {
			SubjUpdated (subjname);
		}

		if (flipangle != flipangle_last) {
			SubjUpdated (subjname);
		}

		grav_gain0_last = grav_gain0;
		PSFactor_last = PSFactor;
		flipangle_last = flipangle;

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		//float q1 = transform.eulerAngles [0];
		//float q2 = transform.eulerAngles [1];
		//float q3 = transform.eulerAngles [2];

		//_currentRobotPosition = GetPosition ();
		_currentRobotVelocity = GetVelocity ();

		float xvel = _currentRobotVelocity [0];
		float yvel = _currentRobotVelocity [1];
		float zvel = _currentRobotVelocity [2];

		_currentRobotForces = _playerData._toolForceQ;
		//float fx = _currentRobotForces [0];
		//float fy = _currentRobotForces [1];
		//float fz = _currentRobotForces [2];
		//float torque =  _playerData._teneoTorqueQ;

		StreamWriter sw = File.AppendText (path);

		if (breaktime == false) {
			
			//mainInputField.DeactivateInputField();



			//FIX ME: add trialNumber, blockNumber, spawnPos
			sw.WriteLine (theTime + "," + x + "," + y + "," + z + ","
			+ xvel + "," + yvel + "," + zvel + ","
			+ playerRotation + "," + flipangle + ","
			+ grav_gain0 + "," + PSFactor + ","
			+ score + "," + caughtToFloor + "," + missedToFloor + ","
			+ ballTouchdown + "," + goalPos);
			sw.Close ();
			//Debug.Log ("write to file");

		} else {
			sw.Close ();
		}
			

		theTime = Time.time;

	}

	void Update ()
	{
		var fileInfo = new System.IO.FileInfo(path);
		FileStatusString = fileInfo.Length.ToString();
		updateFileText ();
	}
		

	private void updateFileText(){
		FileStatus.text = "File Status: " + FileStatusString;


	}


	public void SubjUpdated(string text)
	{

		if (!string.IsNullOrEmpty (subjname) && mainInputField.text != blankString) { 
			
			FileStatus.text = "File Status: " + FileStatusString;
		 
			if (PSFactor != 1.0f) {
				EA_Phrase = "PS_ON";
			} else if (PSFactor == 1.0f) {
				EA_Phrase = "PS_OFF";
			}

			//check if directory doesn't exit
			if (!Directory.Exists ("CupCatch_Data")) {    
				//if it doesn't, create it
				Directory.CreateDirectory ("CupCatch_Data");

			}

			/*if (!Directory.Exists ("CupCatch_Data/" + subjname)) {
				Directory.CreateDirectory ("CupCatch_Data/" + subjname);
			} */
				
			if (!Directory.Exists ("CupCatch_Data/" + subjname + "/" + "TRIAL_" + _trial.trial)) {
				Directory.CreateDirectory ("CupCatch_Data/" + subjname + "/" + "TRIAL_" + _trial.trial);
			}


			Debug.Log ("New Entry Detected " + text);
			System.DateTime theTime = System.DateTime.Now;
			string datetime = theTime.ToString ("yyyy_MM_dd_\\T_HHmm\\Z");
			string pname = string.Concat ("CupCatch_Data/" + subjname + "/" + "TRIAL_" + _trial.trial + "/", 
				subjname + "_" + handedness + "_TRIAL_" + _trial.trial, EA_Phrase, datetime, ".csv");


			/*if(File.Exists(pname)){
				pname = string.Concat("CupCatch_Data/" + subjname + "/", subjname, EA_Phrase, datetime, "(" + fileNum + ")", ".csv");
				fileNum += 1;
			} */
			path = @pname;
		

			// This text is added only once to the file.
			if (!File.Exists (path)) {
				// Create a file to write to.
				using (StreamWriter sw = File.CreateText (path)) {
					//sw.WriteLine ("time," + "xdata," + "ydata," + "zdata," + "q1," + "q2," + "q3," + "ea," + "xref," + "yref," + "zref," + "q1ref," + "q2ref," + "q3ref," + "xvel," + "yvel," + "zvel," + "fx," + "fy," + "fz,"+ "torque");
					sw.WriteLine ("theTime" + "," + "x" + "," + "y" + "," + "z" + ","
					+ "xvel" + "," + "yvel" + "," + "zvel" + ","
					+ "playerRotation" + "," + "flipangle" + ","
					+ "grav_gain0" + "," + "PSFactor" + ","
					+ "score" + "," + "caughtToFloor" + "," + "missedToFloor" + ","
					+ "ballTouchdownX" + "," + "ballTouchdownY" + "," + "ballTouchdownZ" + ","
					+ "goalPosX" + "," + "goalPosY" + "," + "goalPosZ");
				}	
			}
		} else {
			subjname = mainInputField.text;
		}
	
	}

	public Vector3 GetVelocity ()
	{
		return _robotStates.GetToolVelocity ();
	}

	public Vector3 GetPosition ()
	{
		return _robotStates.GetToolPosition ();
	}

	public Vector4 GetJointAngles ()
	{
		return  _robotStates.GetJointPositions ();
	}

	 public void CloseSubj (string text) {
		subjname = mainInputField.text;
		mainInputField.enabled = false;
		FileStatus.enabled = false;
		Prompt.enabled = false;
		Play.enabled = true;
	} 
		
}
