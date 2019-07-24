using System.Collections;
using System.Collections.Generic;
using BurtSharp.UnityInterface;
using BurtSharp.UnityInterface.ClassExtensions;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SimplePlayerController : MonoBehaviour {

	private const float kPositionScale = 95.0f;
	public float EA_gain;
	public float objectScale = 40F;

	private float EAMin = -5.0f, EAMax = 5.0f;
	private float EA_gain0=2.0f, grav_gain0=0f;
	private Vector3 _currentVelocity = Vector3.zero;
	private Vector3 _currentPosition = Vector3.zero;

	private Vector3 _toolForce = Vector3.zero;
	private float _teneoTorque = 0f;
	private float _teneoTorque0 = 0f;


	private ConnectionSetter _robot;


	private Vector3 target_pos, target_angles, handangles;
	private Vector4 robotangles;

	public Text forceText;
	public bool breaktime;

	public Vector4 _toolForceQ = Vector4.zero;
	public float _teneoTorqueQ = 0f;
	private float flipangle, CalibAngle1=90, CalibTenoAngle;
	private float MasterForce;

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
		if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Left) {
			transform.localScale = new Vector3(-objectScale, objectScale, objectScale);
			flipangle = -90f;

		}
		if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Right) {
			transform.localScale = new Vector3(objectScale, objectScale, objectScale);
			flipangle = 90f;
		}
	}

	private void FixedUpdate ()    {
		_currentPosition = GetPosition ();
		_currentVelocity = GetVelocity ();
		transform.position = _currentPosition;
		robotangles = GetJointAngles ();
		transform.eulerAngles = new Vector3 (robotangles [0] * 180 / Mathf.PI, -robotangles [1] * 180 / Mathf.PI, flipangle - 90 - robotangles [3] * 180 / Mathf.PI);
		handangles = transform.eulerAngles;
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
}
