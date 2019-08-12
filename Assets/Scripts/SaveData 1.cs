using System.IO;
using System.Collections;
using System.Collections.Generic;
//using BurtSharp.UnityInterface;
//using BurtSharp.UnityInterface.ClassExtensions;
using UnityEngine;
using UnityEngine.UI;
 
public class SaveData1 : MonoBehaviour
{

	public string path;
	//int time = 0;
	//int interval = 10;
	float theTime;
	private float EA_gain;
	private Vector3 target_pos, target_angles;
	//private ConnectionSetter _robotstates;
	//private TargetController _target;
	//private PlayerController _playerdata;

	private Vector3 _currentRobotVelocity = Vector3.zero;
	private Vector3 _currentRobotForces = Vector3.zero;
	private float _currentTeneoTorque;
	//private Vector3 _currentRobotPosition;
	private bool breaktime;
	public InputField mainInputField;
	private string subjname, subjname_last, EA_Phrase;
	private bool firstrun;
	private float EA_gain_last=0;
	public Text FileStatus;
	private string FileStatusString;

	/* void Awake ()
	{
		_robotstates = GameObject.Find ("ConnectionSetter").GetComponent<ConnectionSetter> ();
		string m_Path = Application.dataPath;
		_target = GameObject.Find ("TargetObject").GetComponent<TargetController> ();
		_playerdata = GameObject.Find ("Player").GetComponent<PlayerController> (); 


 		 


	}

	// Use this for initialization
	void Start ()
	{
		mainInputField.text = "Blank_SubjId";
		 
		subjname = mainInputField.text;
		firstrun = true;
	}


	private void FixedUpdate ()
	{

		breaktime = _target.breaktime;
		subjname = mainInputField.text;
		//Activate function when detected new filename
		mainInputField.onEndEdit.AddListener(SubjUpdated);
		EA_gain = _playerdata.EA_gain;

		//Also activate function on default name when program first runs
		if ((subjname == "Blank_SubjId") && (breaktime==false) && (firstrun==true) ){
			//mainInputField.ActivateInputField();
			SubjUpdated (subjname);
			firstrun = false;

		}

		if (EA_gain != EA_gain_last) {
			SubjUpdated (subjname);

		}
	

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		float q1 = transform.eulerAngles [0];
		float q2 = transform.eulerAngles [1];
		float q3 = transform.eulerAngles [2];

		target_pos = GameObject.FindGameObjectWithTag ("TargetObject").transform.position;
		target_angles = GameObject.FindGameObjectWithTag ("TargetObject").transform.eulerAngles;

		EA_gain_last = EA_gain;
		float xr = target_pos [0];
		float yr = target_pos [1];
		float zr = target_pos [2];
		float q1r = target_angles [0];
		float q2r = target_angles [1];
		float q3r = target_angles [2];


		theTime = Time.time;

		//_currentRobotPosition = GetPosition ();
		_currentRobotVelocity = GetVelocity ();

		float xvel = _currentRobotVelocity [0];
		float yvel = _currentRobotVelocity [1];
		float zvel = _currentRobotVelocity [2];

		_currentRobotForces = _playerdata._toolForceQ;
		float fx = _currentRobotForces [0];
		float fy = _currentRobotForces [1];
		float fz = _currentRobotForces [2];
		float torque =  _playerdata._teneoTorqueQ;


		if (breaktime == false) {
			mainInputField.DeactivateInputField();

			StreamWriter sw = File.AppendText (path);
			sw.WriteLine (theTime + "," + x + "," + y + "," + z + "," + q1 + "," + q2 + "," + q3 + "," + EA_gain + "," + xr + "," + yr + "," + zr + "," + q1r + "," + q2r + "," + q3r + "," + xvel + "," + yvel + "," + zvel + "," + fx + "," + fy + "," + fz + "," + torque);
			//sw.WriteLine ("I'm facing " + transform.forward);
			sw.Close ();
			//Debug.Log ("write to file");

		}
		else {

		}


		 
 	}




	// Update is called once per frame
	void Update ()
	{
		var fileInfo = new System.IO.FileInfo(path);
		FileStatusString = fileInfo.Length.ToString();
		updateFileText ();
	}


	public Vector3 GetVelocity ()
	{
		return _robotstates.GetToolVelocity ();
	}

	public Vector3 GetPosition ()
	{
		return _robotstates.GetToolPosition ();
	}

	public Vector4 GetJointAngles ()
	{
		return  _robotstates.GetJointPositions ();
	}



	private void updateFileText(){
		FileStatus.text = "File Status: " + FileStatusString;


	}

	public void SubjUpdated(string text) 
	{
		if (EA_gain==0){
			EA_Phrase="_NO_FORCE_";
		}
		if (EA_gain!=0){
			EA_Phrase="_FORCE_ON_";
		}

		//check if directory doesn't exit
		if(!Directory.Exists("data"))
		{    
			//if it doesn't, create it
			Directory.CreateDirectory("data");

		}

		Debug.Log("New Entry Detected "  + text);
		System.DateTime theTime = System.DateTime.Now;
		string datetime = theTime.ToString ("yyyy_MM_dd_\\T_HHmm\\Z");
		string pname = string.Concat ("data/", subjname, EA_Phrase, datetime, ".csv");

		path = @pname;


		// This text is added only once to the file.
		if (!File.Exists (path)) {
			// Create a file to write to.
			using (StreamWriter sw = File.CreateText (path)) {
				sw.WriteLine ("time," + "xdata," + "ydata," + "zdata," + "q1," + "q2," + "q3," + "ea," + "xref," + "yref," + "zref," + "q1ref," + "q2ref," + "q3ref," + "xvel," + "yvel," + "zvel," + "fx," + "fy," + "fz,"+ "torque");
			}	
		}
	}
    */
}
