using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SaveData : MonoBehaviour
{

    /*<summary>
	 * Attach this script to the player game object 
	 * </summary> */

    //misc.
    float theTime;
    private bool firstrun;
    public InputField mainInputField;
    private PausePlay _pauseStatus;
    private bool breaktime; //is the game paused?

    //reference other scripts
    private MissedBall _floorData;
    private BallInGoal _goalData;
    private PlayerMovement _playerData;


    //strings & filepath
    public string path;
    private string subjname, subjname_last;
    private string FileStatusString;

    public Text FileStatus;


    //ball data
    private int score;
    private int missedCount;
    private Vector3 ballTouchdown;

    //goal data
    private Vector3 goalPos;



    void Awake()
    {
        breaktime = false;

        _floorData = GameObject.Find("Floor").GetComponent<MissedBall>();
        _goalData = GameObject.Find("Goal").GetComponent<BallInGoal>();

        _pauseStatus = GameObject.Find("Main Camera").GetComponent<PausePlay>();

        string m_Path = Application.dataPath;
    }

    // Use this for initialization
    void Start()
    {
        mainInputField.text = "Blank_SubjId";

        subjname = mainInputField.text;
        firstrun = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        breaktime = _pauseStatus.breaktime;

        if ((subjname == "Blank_SubjId") && (breaktime == false) && (firstrun == true))
        {
            if (subjname != " ")
            {
                mainInputField.ActivateInputField();
                SubjUpdated(subjname);
                firstrun = false;
            }

        }

        subjname = mainInputField.text;
        //Activate function when detected new filename
        mainInputField.onEndEdit.AddListener(SubjUpdated);

        //player data


        //ball data
        score = _goalData.numCaptured;
        missedCount = _floorData.missedCount;

        ballTouchdown = _floorData.fallenPos;

        //goal data
        goalPos = _goalData.thisPos;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        float q1 = transform.eulerAngles[0];
        float q2 = transform.eulerAngles[1];
        float q3 = transform.eulerAngles[2];




        if (breaktime == false)
        {
            mainInputField.DeactivateInputField();

            StreamWriter sw = File.AppendText(path);
            //sw.WriteLine (theTime + "," + x + "," + y + "," + z + "," + q1 + "," + q2 + "," + q3 + "," + EA_gain + "," + xr + "," + yr + "," + zr + "," + q1r + "," + q2r + "," + q3r + "," + xvel + "," + yvel + "," + zvel + "," + fx + "," + fy + "," + fz + "," + torque);
            //sw.WriteLine ("I'm facing " + transform.forward);

            sw.WriteLine(theTime + "," + x + "," + y + "," + z + ","
                + q1 + "," + q2 + "," + q3 + ","
                + score + "," + ballTouchdown + "," + goalPos);
            sw.Close();
            //Debug.Log ("write to file");

        }


        theTime = Time.time;

    }

    void Update()
    {
        var fileInfo = new System.IO.FileInfo(path);
        FileStatusString = fileInfo.Length.ToString();
        updateFileText();
    }





    private void updateFileText()
    {
        FileStatus.text = "File Status: " + FileStatusString;


    }


    public void SubjUpdated(string text)
    {
        FileStatus.text = "File Status: " + FileStatusString;


        //check if directory doesn't exit
        if (!Directory.Exists("CupCatch_Data"))
        {
            //if it doesn't, create it
            Directory.CreateDirectory("CupCatch_Data");

        }

        Debug.Log("New Entry Detected " + text);
        System.DateTime theTime = System.DateTime.Now;
        string datetime = theTime.ToString("yyyy_MM_dd_\\T_HHmm\\Z");
        string pname = string.Concat("CupCatch_Data/", subjname, datetime, ".csv");

        path = @pname;


        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                //sw.WriteLine ("time," + "xdata," + "ydata," + "zdata," + "q1," + "q2," + "q3," + "ea," + "xref," + "yref," + "zref," + "q1ref," + "q2ref," + "q3ref," + "xvel," + "yvel," + "zvel," + "fx," + "fy," + "fz,"+ "torque");
                sw.WriteLine("theTime" + "," + "x" + "," + "y" + "," + "z" + ","
                    + "q1" + "," + "q2" + "," + "q3" + ","
                    + "score" + "," + ballTouchdown + "," + goalPos);
                sw.Close();
            }
        }
    }
}



/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WriteToCSVFile
{
    public class SaveData
    {

        static void Main(string[] args)
        {
            addRecord(0, "test", "one", 0, 0, "csvTest.txt");



        }

        public static void addRecord(int ID, string subjName, string subjNameLast, int score, int missedCount, string filepath)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.WriteLine(ID + "," + subjName + "," + subjNameLast + "," + score + "," + missedCount);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: ", ex);
            }
        }
    }
} */
