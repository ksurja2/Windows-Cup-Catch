using System.Collections;
using System.Collections.Generic;
using BurtSharp.UnityInterface;
using BurtSharp.UnityInterface.ClassExtensions;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SimplePlayerController : MonoBehaviour {

	private const float kPositionScale = 95.0f;
	public float objectScale = 40F;

	private Vector3 _currentVelocity = Vector3.zero;
	private Vector3 _currentPosition = Vector3.zero;

	private Vector3 _toolForce = Vector3.zero;
	private float _teneoTorque = 0f;
	private float _teneoTorque0 = 0f;


	private ConnectionSetter _robot;


	private Vector3 target_pos, target_angles, handangles;
	private Vector4 robotangles;

	public Text PSText;
	public Text FlipAngleText;
	public Text GravText;
	public bool breaktime;

	public Vector4 _toolForceQ = Vector4.zero;
	public float _teneoTorqueQ = 0f;
	private float flipangle = 180;
	private float CalibAngle1=90, CalibTenoAngle;
	private float grav_gain0 = 0f; 
	private float MasterForce = 0; //figure out what this does

	private float PSFactor = 1.0f; //determines how much the player must pronate to flip bucket
	private float minPSFactor = 1.0f;
	private float maxPSFactor = 4.0f;

	public GameObject player;

	public float upperXBound = 50f;
	public float lowerXBound = -50f;
	public float upperZBound = 60f;
	public float lowerZBound = -20f;
	public float upperYBound = 0f;
	public float lowerYBound = -3.5f;

	private void Awake () {
		_robot = GameObject.Find ("ConnectionSetter").GetComponent<ConnectionSetter> ();
	}

	// Use this for initialization
	void Start () {
		if (_robot != null) {
			// register control function
			_robot.RegisterControlFunction ("haptics", CalcForces);
			_robot.SetActiveControlFunction ("haptics");

			_robot.EnableRobot ();

			//_robot.UgcGain = 1;
			Debug.Log("Control Loop Time: " + _robot.ControlUpdateTime.ToString ());

		} else {
			BurtSharp.SystemLogger.Error ("RobotConnection not found.");

		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("]")) {
			grav_gain0 = grav_gain0 + 0.1f;

		}

		if (Input.GetKeyDown ("[")) {
			grav_gain0 = grav_gain0 - 0.1f;
		}
		grav_gain0 = Mathf.Clamp (Mathf.Round (grav_gain0 * 10) / 10, -5, 5);
		_robot.UgcGain = grav_gain0;

		PSFactor = PSFactor + Input.GetAxis("Vertical") * 0.1f;

		//breaktime = _target.breaktime;
		PSFactor = Mathf.Clamp (Mathf.Round (PSFactor * 10) / 10, minPSFactor, maxPSFactor);

		flipangle = flipangle + Input.GetAxis("Horizontal") * 5.0f ;
		flipangle = Mathf.Clamp (Mathf.Round (flipangle * 10) / 10, 0, 360);


		if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Left) {
			transform.localScale = new Vector3(-objectScale, objectScale, objectScale);
			//flipangle = -180f; //fully supinate to have upright bucket
			flipangle = -flipangle;

		}
		if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Right) {
			transform.localScale = new Vector3(objectScale, objectScale, objectScale);
			//flipangle = 180f;
			//flipangle = flipangle;
		}

		UpdatePSText ();
		UpdateFlipAngleText ();
		UpdateGravText ();
	}

	private void FixedUpdate ()    {
		_currentPosition = GetPosition ();
		_currentVelocity = GetVelocity ();
		transform.position = _currentPosition;
		robotangles = GetJointAngles ();


		//transform.eulerAngles = new Vector3 (robotangles [0] * 180 / Mathf.PI, -robotangles [1] * 180 / Mathf.PI, flipangle - 90 - robotangles [3] * 180 / Mathf.PI);

		//0 out X
		//Results: 0 rotation on Y axis, tilted to X axis
		//transform.eulerAngles = new Vector3 (0.0f, -robotangles [1] * 180 / Mathf.PI, flipangle - 90 - robotangles [3] * 180 / Mathf.PI);

		//0 out Y
		//Results: still tilted
		//transform.eulerAngles = new Vector3 (robotangles [0] * 180 / Mathf.PI, 0.0f, flipangle - 90 - robotangles [3] * 180 / Mathf.PI);

		//0 out Z
		//Results: cannot pronate/supinate
		//transform.eulerAngles = new Vector3 (robotangles [0] * 180 / Mathf.PI, -robotangles [1] * 180 / Mathf.PI, 0.0f);

		//0 out X and Y
		//Results: Yes!!!
		//transform.eulerAngles = new Vector3 (0, 0, (flipangle - 90 - robotangles [3] * 180 / Mathf.PI) * 1.5f);
		transform.eulerAngles = new Vector3 (0, 0, (flipangle - 90 - robotangles [3] * 180 / Mathf.PI) * PSFactor);


		handangles = transform.eulerAngles;
		OutOfBounds ();
	}

	void OutOfBounds()
	{
		float playerPositionX = player.transform.position.x;
		float playerPositionZ = player.transform.position.z;
		float playerPositionY = player.transform.position.y;

		if (playerPositionX > upperXBound || playerPositionX < lowerXBound)
		{
			playerPositionX = Mathf.Clamp(transform.position.x, lowerXBound, upperXBound);
			player.transform.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

		}

		if (playerPositionZ > upperZBound || playerPositionZ < lowerZBound)
		{
			playerPositionZ = Mathf.Clamp(transform.position.z, lowerZBound, upperZBound);
			player.transform.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

		}

		if (playerPositionY > upperYBound || playerPositionY < lowerYBound)
		{
			playerPositionY = Mathf.Clamp(transform.position.y, lowerYBound, upperYBound);
			player.transform.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

		}
	}

	public Vector3 GetVelocity () {
		return kPositionScale * _robot.GetToolVelocity ();
	}
	public Vector3 GetPosition () {
		return kPositionScale * _robot.GetToolPosition ();
	}

	public Vector4 GetJointAngles () {
		return  _robot.GetJointPositions ();
	}

	public BurtSharp.CoAP.MsgTypes.RobotCommand CalcForces ()
	{

		Vector3 vel = _currentVelocity;
		Vector3 pos = _currentPosition;



		Vector3 vel_norm = vel.normalized;
		float vel_mag = vel.magnitude;

		Vector3 coreTorque = Vector3.zero;



		//_teneoTorque0 = 1.0f;
		CalibTenoAngle = -Mathf.DeltaAngle ( handangles [2], CalibAngle1)+180.0f;

		if (CalibTenoAngle > 160+180) {
			Debug.Log ("Teneo Limit high: "  );

			_teneoTorque = _teneoTorque0*Mathf.Clamp(Mathf.Cos(5.0f*(CalibTenoAngle-160 +180)*Mathf.PI/180), 0, 1);
			//_teneoTorque = 0f;
		}

		if (CalibTenoAngle < 20+180) {
			Debug.Log ("Teneo Limit low: "  );
			//Mathf.Cos((handangles[2]-180+90))
			_teneoTorque = _teneoTorque0*Mathf.Clamp(Mathf.Cos(5.0f*(CalibTenoAngle-20 +180)*Mathf.PI/180), 0, 1);
			//_teneoTorque = 0f;
		}


		_toolForceQ=_toolForce;
		_teneoTorqueQ=_teneoTorque;

		return RobotCommandExtensions.RobotCommandForceTorque (_toolForce*MasterForce, coreTorque, _teneoTorque*MasterForce);

	}

	private void UpdatePSText(){
		PSText.text = "PS Factor ↑↓: " + PSFactor.ToString ();
	}

	private void UpdateGravText(){
		GravText.text = " Grav []: " + grav_gain0.ToString ();
	}

	private void UpdateFlipAngleText(){
		FlipAngleText.text = "Flip Angle ← →: " + flipangle.ToString ();
	}
}
