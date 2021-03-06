﻿using System.Collections;
using System.Collections.Generic;
using BurtSharp.UnityInterface;
using BurtSharp.UnityInterface.ClassExtensions;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


/*this code varies from the SimplePlayerController script in that it reworks the PSFactor into
 an error augmentation gain with a lower limit of 0 and an upper limit of 2. MasterForce is initialized to 1, and only
 becomes 0 when the game is paused*/
public class PlayerMovementWithTorque : MonoBehaviour
{

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

    public Vector4 _toolForceQ = Vector4.zero;
    public float _teneoTorqueQ = 0f;
    public float flipangle; // = 180 for RH, 0 for LH
    private float CalibAngle1 = 90, CalibTenoAngle;
    public float grav_gain0 = 0f;
    private float MasterForce = 1; //overall multiplier for CalcForces() function

    public float PSFactor = 1.0f; //pronation/supination factor; determines how much the player must pronate to flip bucket; error augmentation
    private float minPSFactor = 0.0f;
    private float maxPSFactor = 2.0f;

    public GameObject player;

    public float upperXBound = 50f;
    public float lowerXBound = -50f;
    public float upperZBound = 60f;
    public float lowerZBound = -20f;
    public float upperYBound = 3.5f;
    public float lowerYBound = 0.0f;

    public string hand;

    private void Awake()
    {
        _robot = GameObject.Find("ConnectionSetter").GetComponent<ConnectionSetter>(); //establish connection with robot
    }

    // Use this for initialization
    void Start()
    {
        if (_robot != null)
        {
            // register control function
            _robot.RegisterControlFunction("haptics", CalcForces);
            _robot.SetActiveControlFunction("haptics");

            _robot.EnableRobot();
        }
        else
        {
            BurtSharp.SystemLogger.Error("RobotConnection not found.");
        }

        if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Left)
        {
            flipangle = 0;
            hand = "LH"; //used in filename
        }

        if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Right)
        {
            flipangle = 180;
            hand = "RH"; //used in filename
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("]"))
        { //change gravity gains
            grav_gain0 = grav_gain0 + 0.1f;
        }

        if (Input.GetKeyDown("["))
        {
            grav_gain0 = grav_gain0 - 0.1f;
        }
        grav_gain0 = Mathf.Clamp(Mathf.Round(grav_gain0 * 10) / 10, -5, 5);
        _robot.UgcGain = grav_gain0;

        PSFactor = PSFactor + Input.GetAxis("Vertical") * 0.1f;
        //allows update while paused
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PSFactor -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PSFactor += 0.1f;
        }

        PSFactor = Mathf.Clamp(Mathf.Round(PSFactor * 10) / 10, minPSFactor, maxPSFactor);



        flipangle = flipangle + Input.GetAxis("Horizontal") * 5.0f; //this angle indicates when the bucket is upright

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            flipangle -= 5;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            flipangle += 5;
        }
        flipangle = Mathf.Clamp(Mathf.Round(flipangle * 10) / 10, 0, 180);


        if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Left)
        {
            transform.localScale = new Vector3(-objectScale, objectScale, objectScale);
        }
        if (_robot.Status.handedness == BurtSharp.CoAP.MsgTypes.RobotHandedness.Right)
        {
            transform.localScale = new Vector3(objectScale, objectScale, objectScale);

        }

        UpdatePSText();
        UpdateFlipAngleText();
        UpdateGravText();
    }

    private void FixedUpdate()
    {
        _currentPosition = GetPosition();
        _currentVelocity = GetVelocity();
        transform.position = _currentPosition;
        robotangles = GetJointAngles();


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

        //0 out q1 and q2 of hand angles to isolate pronation/supination
        //Results: Yes!!!
        //transform.eulerAngles = new Vector3 (0, 0, (flipangle - 90 - robotangles [3] * 180 / Mathf.PI) * 1.5f);
        transform.eulerAngles = new Vector3(0, 0, (flipangle - 90 - robotangles[3] * 180 / Mathf.PI)); //note there is no scalar multiplier applied to handangles


        handangles = transform.eulerAngles;
        OutOfBounds();
    }

    void OutOfBounds()
    {
        //
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

    public Vector3 GetVelocity()
    {
        return kPositionScale * _robot.GetToolVelocity();
    }
    public Vector3 GetPosition()
    {
        return kPositionScale * _robot.GetToolPosition();
    }

    public Vector4 GetJointAngles()
    {
        return _robot.GetJointPositions();
    }

    public BurtSharp.CoAP.MsgTypes.RobotCommand CalcForces()
    {

        Vector3 vel = _currentVelocity;
        Vector3 pos = _currentPosition;



        Vector3 vel_norm = vel.normalized;
        float vel_mag = vel.magnitude;

        Vector3 coreTorque = Vector3.zero;


        //email felix
        //this section is what we want 
        _teneoTorque0 = 1.0f;
        //CalibTenoAngle = -Mathf.DeltaAngle(handangles[2], CalibAngle1) + 180.0f;
        
        //relates torque linearly to difference in angle between player and some arbitrary constant
        CalibTenoAngle = -Mathf.DeltaAngle(handangles[3], CalibAngle1) + 180.0f; //apply calibration to pronation/supination portion of handangles

        if (CalibTenoAngle > 160 + 180)
        {
            //Debug.Log ("Teneo Limit high: "  );

            _teneoTorque = _teneoTorque0 * Mathf.Clamp(Mathf.Cos(5.0f * (CalibTenoAngle - 160 + 180) * Mathf.PI / 180), 0, 1);
            //_teneoTorque = 0f;
        }

        if (CalibTenoAngle < 20 + 180)
        {
            //Debug.Log ("Teneo Limit low: "  );
            //Mathf.Cos((handangles[2]-180+90))
            _teneoTorque = _teneoTorque0 * Mathf.Clamp(Mathf.Cos(5.0f * (CalibTenoAngle - 20 + 180) * Mathf.PI / 180), 0, 1);
            //_teneoTorque = 0f;
        }


        _toolForceQ = _toolForce;
        _teneoTorqueQ = _teneoTorque;

        //we want to apply different viscosities 
        //positive viscosity F = -bx
        //negative viscosity F = bx
        //where b is the PSFactor
        //x is the user torque/hand velocity
        //need safety checks in place as well

        _teneoTorque = _teneoTorque0

        return RobotCommandExtensions.RobotCommandForceTorque(_toolForce * MasterForce, coreTorque, _teneoTorque * MasterForce);

    }

    private void UpdatePSText()
    {
        PSText.text = "PS Factor ↑↓: " + PSFactor.ToString();
    }

    private void UpdateGravText()
    {
        GravText.text = " Grav []: " + grav_gain0.ToString();
    }

    private void UpdateFlipAngleText()
    {
        FlipAngleText.text = "Flip Angle ← →: " + flipangle.ToString();
    }
}
