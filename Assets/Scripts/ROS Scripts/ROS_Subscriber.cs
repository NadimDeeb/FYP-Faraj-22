using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using RosMessageTypes.Nav;
using RosMessageTypes.Visualization;
using Microsoft.MixedReality.Toolkit.UI;
using Wudang.Temple.Control;

public class ROS_Subscriber : MonoBehaviour
{
    public OogwayController OoController;
<<<<<<< Updated upstream
    public JointStateMsg JointState;
=======
    public ROS_Publisher RosPublisher;

    public JointStateMsg JointState;
    public string[] JointNames;
    private MarkerArrayMsg MarkerArray;
    //private AudioSource audioSource;
>>>>>>> Stashed changes

    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<JointStateMsg>("/arm_joint_states", JointStateCB);
        ROSConnection.GetOrCreateInstance().Subscribe<MarkerArrayMsg>("/tesseract/display_tool_path", MarkerArrayCB);
    }

    // Receive JointStates and store them in public variable to for use by the arm joints
    public void JointStateCB(JointStateMsg msg)
    {
        // Receive joint state message 
        JointState = new JointStateMsg();
        JointState = msg;

<<<<<<< Updated upstream
        OoController.JointState = JointState; //Set joint states received in controller
        Debug.Log("Received: " + msg.name[0]);
        Debug.Log("Received: " + msg.position[0]);
=======
        // Convert from radians to degrees
        for (int i = 0; i < JointState.position.Length; i++)
        {
            JointState.position[i] = JointState.position[i] * 180 / Mathf.PI;
        }

        // Set joint states received in controller
        OoController.JointState = JointState;

        Debug.Log("gling gling");
>>>>>>> Stashed changes

        StartCoroutine(OoController.JointArticulator());
    }

    public void MarkerArrayCB(MarkerArrayMsg msg)
    {
        MarkerArray = new MarkerArrayMsg();
        MarkerArray = msg;

        RosPublisher.MarkerArray = MarkerArray;
        Debug.Log("Miawww");
    }
}
