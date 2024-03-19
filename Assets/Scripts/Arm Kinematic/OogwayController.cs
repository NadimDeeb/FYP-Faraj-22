using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.Sensor;
using Unity.Robotics.UrdfImporter.Control;
using System;

public class OogwayController : MonoBehaviour
{
    // Scripts
    public ROS_Subscriber RosSub;
    public Controller URDFController;
    public ShifuControl Shifu;

    // Variables
    public ArticulationBody[] articulationChain_1;
    public ArticulationBody[] articulationChain;
    public float[] jointPositions;
    public JointStateMsg JointState;
    public float stiffness;
    public float damping;
    public float forceLimit;
    public float speed;
    public float torque;
    public float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        articulationChain_1 = this.GetComponentsInChildren<ArticulationBody>();
        JointState = new JointStateMsg();

        // Dropping the last element from the articulationChain array
        articulationChain = new ArticulationBody[articulationChain_1.Length - 1];
        Array.Copy(articulationChain_1, articulationChain, articulationChain.Length);

        foreach (ArticulationBody joint in articulationChain)
        {
            joint.gameObject.AddComponent<ShifuControl>();
            
            joint.jointFriction = 10;
            joint.angularDamping = 10;
            ArticulationDrive currentDrive = joint.xDrive;
            currentDrive.forceLimit = 1000;
            joint.xDrive = currentDrive;
        }

        stiffness = URDFController.stiffness;
        damping = URDFController.damping;
        forceLimit = URDFController.forceLimit;
        speed = URDFController.speed;
        torque = URDFController.torque;
        acceleration = URDFController.acceleration;
    }

    public IEnumerator JointArticulator()
    {
        //Debug.Log("Absolute sex");
        while (true)
        {
            // Update joint positions based on JointState message
            UpdateJointPositions();

            // Wait for the next frame
            yield return null;
        }
    }

    private void UpdateJointPositions()
    {
        Debug.Log("UpdateJoint Called");
        // Check if JointState message is available
        if (JointState != null)
        {
            // Ensure the number of joints matches the number of elements in the articulation chain
            if (JointState.name.Length == articulationChain.Length)
            {
                // Iterate over each joint and set its position based on the JointState message
                for (int i = 0; i < articulationChain.Length; i++)
                {
                    // Save target joint position
                    // It is assumed that the joint positions go from base to end effector
                    jointPositions[i] = (float)JointState.position[i];
                    //Debug.Log("Cool it worked");
                }
            }
            else
            {
                Debug.LogWarning("Number of joints in JointState message does not match the number of elements in the articulation chain.");
            }
        }
    }
}
