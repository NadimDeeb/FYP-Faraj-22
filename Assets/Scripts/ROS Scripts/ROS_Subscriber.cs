using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using RosMessageTypes.Nav;
using Microsoft.MixedReality.Toolkit.UI;

public class ROS_Subscriber : MonoBehaviour
{
    public OogwayController OoController;
    public JointStateMsg JointState;

    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<JointStateMsg>("/arm_joint_states", JointStateCB);
    }

    // Receive JointStates and store them in public variable to for use by the arm joints
    public void JointStateCB(JointStateMsg msg)
    {
        JointState = new JointStateMsg();
        JointState = msg;

        OoController.JointState = JointState; //Set joint states received in controller
        Debug.Log("Received: " + msg.name[0]);
        Debug.Log("Received: " + msg.position[0]);

        StartCoroutine(OoController.JointArticulator());
    }

}
