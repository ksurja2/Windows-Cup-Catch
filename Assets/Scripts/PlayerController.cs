using System.Collections;
using System.Collections.Generic;
using BurtSharp.UnityInterface;
using BurtSharp.UnityInterface.ClassExtensions;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


/// <summary>
/// Player controller class. This script is attached to the Player object in the cube-sphere
/// Unity example. This class derives from MonoBehaviour, which defines some standard functions
/// that are called by Unity at specified times. Do not change the names of these functions.
///
/// Useful references:
///   https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
///   https://docs.unity3d.com/Manual/ExecutionOrder.html
/// </summary>
public class PlayerController : MonoBehaviour {

    // Scales the robot position to more closely appoximate the Unity workspace for this game.
    private const float kPositionScale = 15.0f;
	public float EA_gain;

	private float EAMin = -5.0f, EAMax = 5.0f;
	private float EA_gain0=2.0f, grav_gain0=0f;
	private Vector3 _currentVelocity = Vector3.zero;
	private Vector3 _currentPosition = Vector3.zero;

    private Vector3 _toolForce = Vector3.zero;
	private float _teneoTorque = 0f;
	private float _teneoTorque0 = 0f;

    // Handles communication with the robot. Make sure to include a RobotConnection
    // in your Unity program, and make sure to set it up in the first scene that
    // will be active.
	private ConnectionSetter _robot;
	//private TargetController _target;

    // The collider associated with this player object
    //private Collider _playerCollider;

	private Vector3 target_pos, target_angles, handangles;
	private Vector4 robotangles;
 
	public Text forceText;
	public bool breaktime;

	public Vector4 _toolForceQ = Vector4.zero;
	public float _teneoTorqueQ = 0f;
	private float flipangle, CalibAngle1=0, CalibTenoAngle;
	private float MasterForce;
	////////////////////////////////////////
    /// Standard MonoBehaviour functions ///
    ////////////////////////////////////////

    /// <summary>
    /// Runs when the scene is entered. This is the first thing that happens, and it
    /// happens only once. Use this function for initializing objects and for setting
    /// values that don't matter to other objects. Don't use this to communicate with
    /// other objects because you don't know which objects have already had their
    /// Awake() functions called.
    /// 
    /// The stiffness and damping of a haptic object can be set here because they are
    /// properties of the object and setting them doesn't require any action.
    /// 
    /// Haptic cursors should also be registered here. This is required for proper
    /// functioning of the haptic objects.
    /// </summary>
    private void Awake () {
		_robot = GameObject.Find ("ConnectionSetter").GetComponent<ConnectionSetter> ();
		//_target = GameObject.Find ("TargetObject").GetComponent<TargetController> ();

 
        
		//target_pos = GameObject.FindGameObjectWithTag("TargetObject").transform.position;
        
        //_playerCollider = this.GetComponent<Collider> ();

		 

		updateForceText ();
	
    }

    /// <summary>
    /// Runs when the object is enabled (and thus the scene has already been entered).
    /// This can happen multiple times, since objects can be enabled and disabled.
    /// All of the other objects in the scene have already had their Awake() functions
    /// called, so communication between objects can happen here.
    /// </summary>
    private void OnEnable () {}

    /// <summary>
    /// Runs after OnEnable, but only occurs once (the first time the object is enabled).
    /// Communication between objects can also happen here.
    /// 
    /// In this case, registering the control function and enabling the robot happen here
    /// because they only need to happen once, but cannot happen in Awake() because the
    /// RobotConnection needs to be initialized first.
    /// </summary>
    private void Start () {
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

    /// <summary>
    /// Runs during the physics loop. The physics loop is responsible for handling collisions
    /// and triggers. transform.position is the position of the yellow sphere that the user
    /// controls, so it should be updated here in order for Unity to have the most up-to-date
    /// information possible for handling collisions.
    /// </summary>
	/// 
	void Update ()
	{
		

		EA_gain0 = EA_gain0 + Input.GetAxis("Vertical") * 0.1f;
		 
		//breaktime = _target.breaktime;
			EA_gain = Mathf.Clamp (Mathf.Round (EA_gain0 * 10) / 10, EAMin, EAMax);

		if (breaktime == true) {
			MasterForce = 0;


	


		}
		if (breaktime == false) {
			MasterForce = 1.0f;
		}

		if (Input.GetKeyDown ("0")) {
			EA_gain0=0;
			EA_gain=0;

		}
		if (Input.GetKeyDown ("2")) {
			EA_gain0=2;
			EA_gain=2;

		}

		if (Input.GetKeyDown ("5")) {
			CalibAngle1=handangles [2]+180;
			Debug.Log ("Calib Angle " + CalibAngle1);

		}

		if (Input.GetKeyDown ("]")) {
			grav_gain0 = grav_gain0 + 0.1f;

		}

		if (Input.GetKeyDown ("[")) {
			grav_gain0 = grav_gain0 - 0.1f;
		}
		grav_gain0 = Mathf.Clamp (Mathf.Round (grav_gain0 * 10) / 10, 0, 5);
		_robot.UgcGain = grav_gain0;
		//Debug.Log ();
		if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Left) {
			transform.localScale = new Vector3(-35F, 35F, 35F);
			flipangle = 180f;

		}
		if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Right) {
			transform.localScale = new Vector3(35F, 35F, 35F);
			flipangle = 0f;
		}
		//EA_gain = EA_gain0;
		updateForceText ();

	}
		
    private void FixedUpdate ()    {
        _currentPosition = GetPosition ();
		_currentVelocity = GetVelocity ();
		transform.position = _currentPosition;
		robotangles = GetJointAngles ();
		transform.eulerAngles = new Vector3(robotangles[0]*180/Mathf.PI, -robotangles[1]*180/Mathf.PI,flipangle -90-robotangles[3]*180/Mathf.PI);
		handangles = transform.eulerAngles;
		//target_pos = GameObject.FindGameObjectWithTag("TargetObject").transform.position;
		//target_angles = GameObject.FindGameObjectWithTag("TargetObject").transform.eulerAngles;

		 
    }

    /// <summary>
    /// This is the control loop that calculates the forces to be applied to the robot. It
    /// runs automatically through RobotConnection, as long as it is registered. In this
    /// example, this function is registered in the Start () function.
    /// 
    /// The RobotCommand type may include a force, a torque, both, or neither. Whichever of
    /// these are set get sent to the robot.
    /// </summary>
    public BurtSharp.CoAP.MsgTypes.RobotCommand CalcForces ()
    {
        /// Dividing the force by kPositionScale allows the gains to be independent of the value
        /// of kPositionScale. If you add forces that are not related to haptic objects and are
        /// independent of the scaling, you may need to divide only certain components of the
        /// force by kPositionScale.
//        Vector3 force = _toolForce / kPositionScale;
		Vector3 vel = _currentVelocity;
		Vector3 pos = _currentPosition;



		//Vector3 hand_to_target_vec = target_pos - pos;
		//Vector3 hand_to_target_vec_norm = hand_to_target_vec.normalized;

		Vector3 vel_norm = vel.normalized;
		float vel_mag = vel.magnitude;
		//Vector3 plane_norm = Vector3.Cross(vel_norm, hand_to_target_vec_norm);

		//Vector3 force_norm = Vector3.Cross(hand_to_target_vec_norm, plane_norm);

		//float proj_angle = Mathf.Acos (Vector3.Dot(vel_norm, hand_to_target_vec_norm));

		//float forcemag = 3.0f*vel_mag*Mathf.Sin(proj_angle);
		//Vector3 force1 = forcemag*force_norm;  




		//Vector3 force2 = -EA_gain*hand_to_target_vec;  

		//* Mathf.PI / 180.0f 

		//_teneoTorque0 = -EA_gain*0.002f * Mathf.DeltaAngle (target_angles [2], handangles [2]);
			//(0.0f*target_angles [2] + 1.0f*transform.eulerAngles[2] );

		//Vector3 force = _robot.GetToolVelocity () *0 +2.0f;  
		//_toolForce =new Vector3 (10,0,0);
		//_toolForce = force2;
		//Debug.Log (target_pos.ToString ());
		Vector3 coreTorque = Vector3.zero;



		//_teneoTorque0 = 1.0f;
		CalibTenoAngle = -Mathf.DeltaAngle ( handangles [2], CalibAngle1)+180.0f;

		if (CalibTenoAngle > 160+180) {
			//Debug.Log ("Teneo Limit high: "  );

			_teneoTorque = _teneoTorque0*Mathf.Clamp(Mathf.Cos(5.0f*(CalibTenoAngle-160 +180)*Mathf.PI/180), 0, 1);
			//_teneoTorque = 0f;
		}

		if (CalibTenoAngle < 20+180) {
			//Debug.Log ("Teneo Limit low: "  );
			//Mathf.Cos((handangles[2]-180+90))
			_teneoTorque = _teneoTorque0*Mathf.Clamp(Mathf.Cos(5.0f*(CalibTenoAngle-20 +180)*Mathf.PI/180), 0, 1);
			//_teneoTorque = 0f;
		}

		//_teneoTorque = _teneoTorque0;


		//UnityEngine.Debug.Log (_toolForce.ToString ());
		_toolForceQ=_toolForce;
		_teneoTorqueQ=_teneoTorque;


		//_robot.SetUserGravityCompGain (value, _ugcUnits)
		//_robot.SetUserGravityCompGain
		return RobotCommandExtensions.RobotCommandForceTorque (_toolForce*MasterForce, coreTorque, _teneoTorque*MasterForce);
	
    }

    /// <summary>
    /// Runs every time the object is disabled. This can happen multiple times.
    /// </summary>
    private void OnDisable () {}

    /// <summary>
    /// Raises the trigger enter event. This happens when the object first contacts
    /// another object. This will only occur once for each collision. If the object remains
    /// in contact, OnTriggerStay() will be called instead.
    /// </summary>
    private void OnTriggerEnter (Collider other) {}

    /// <summary>
    /// Raises the trigger stay event. This happens for every timestep during which
    /// the player object remains in contact with the other object.
    ///
    /// If the colliding object is tagged as a HapticObject, it calculates forces that
    /// can be applied to the robot. An object can be tagged in the Unity editor through
    /// the Tags drop-down menu.
    ///
    /// Note that this code works when the player object can only contact one object at
    /// a time, and only at a single point. If it will be possible to contact multiple
    /// objects at the same time or multiple points on the same object (the latter can
    /// often happen with very thin objects), modifications must be made to handle this.
    /// </summary>
//    private void OnTriggerStay (Collider other) {
//        if (other.gameObject.CompareTag ("HapticObject")) {
//            _toolForce = other.gameObject.GetComponent<HapticObject> ().GetForce (_playerCollider);
//        }
//    }

    /// <summary>
    /// Raises the trigger exit event. This happens at the timestep when the player object
    /// loses contact with the other object.
    ///
    /// Sets the force back to zero. Again, this assumes that the player was only in contact
    /// with one object.
    /// </summary>
    private void OnTriggerExit (Collider other) {
        _toolForce = Vector3.zero;
    }

    /// <summary>
    /// This function demonstrates how to capture key presses and write messages to the Unity
    /// console. Key presses are not used in this example.
    /// </summary>
    private void OnGUI () {
        Event e = Event.current;
        string keyPressed = e.keyCode.ToString();
        if (e.type == EventType.KeyUp) {
            Debug.Log ("Key pressed: " + keyPressed);
            keyPressed = keyPressed.ToLower ();
        }
    }

    /// <summary>
    /// Raises the application quit event. This is called when you quit the game.
    /// </summary>
    private void OnApplicationQuit() {}

    ////////////////////////////////////
    /// Custom functions begin here! ///
    ////////////////////////////////////

    /// <summary>
    /// Gets the tool velocity (scaled for the Unity workspace).
    /// </summary>
    /// <returns>The velocity.</returns>
    public Vector3 GetVelocity () {
        return kPositionScale * _robot.GetToolVelocity ();
    }
	public Vector3 GetPosition () {
		return kPositionScale * _robot.GetToolPosition ();
	}

	public Vector4 GetJointAngles () {
		return  _robot.GetJointPositions ();
	}

 

	private void updateForceText(){
		forceText.text = "EA Gain ↑↓: " + EA_gain.ToString () + "    Grav []: " + grav_gain0.ToString ();
		//forceText.text = "Teneo Torque " + CalibTenoAngle.ToString ();

	}


}
