using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShifuControl : MonoBehaviour
{
    private float[] jointPositions; // Array to store joint positions
    private int jointIndex; // Index of the joint controlled by this script
    private OogwayController Oogway;
    public ArticulationBody joint;
    public ArticulationDrive currentDrive;
<<<<<<< Updated upstream
=======
    public JointStateMsg JointState;
    public string[] MyJointNames;

>>>>>>> Stashed changes
    public float speed;
    public float torque;
    public float acceleration;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< Updated upstream
        Oogway = (OogwayController)this.GetComponentInParent(typeof(OogwayController));
        joint = this.GetComponent<ArticulationBody>();
=======
        Debug.Log("Shifu called");
        // Controller
        Oogway = (Wudang.Temple.Control.OogwayController)this.GetComponentInParent(typeof(Wudang.Temple.Control.OogwayController));

        //Enums
        direction = 0;

        //ROS messages
        JointState = new JointStateMsg();

        //Variables
        articulationChain = Oogway.articulationChain;
        MyJointNames = Oogway.MyJointNames;
    }

    // Continuously updating arm joint states
    private void FixedUpdate()
    {
        JointState = Oogway.JointState;
>>>>>>> Stashed changes
        speed = Oogway.speed;
        torque = Oogway.torque;
        acceleration = Oogway.acceleration;

        // Find the index of the joint in the articulation chain
        jointIndex = GetJointIndex();

        // Get the joint positions array from the OogwayController
        jointPositions = Oogway.jointPositions;
    }

    // FixedUpdate is called once per physics update
    public void FixedUpdate()
    {
        //Debug.Log("Changing the target position of joint " + jointIndex);
        currentDrive = joint.xDrive;
        // Ensure jointPositions array and jointIndex are valid
        if (jointPositions != null && jointIndex >= 0 && jointIndex < jointPositions.Length)
        {
<<<<<<< Updated upstream
            // Set the target position of the joint
            currentDrive.target = jointPositions[jointIndex];
            joint.xDrive = currentDrive;
            //Debug.Log("jointDrive changed successfully.");
        }
        else
        {
            Debug.LogWarning("Invalid jointPositions array or jointIndex.");
        }
    }

    // Find the index of the joint in the articulation chain
    private int GetJointIndex()
    {
        ArticulationBody[] articulationChain = Oogway.articulationChain;
        for (int i = 0; i < articulationChain.Length; i++)
        {
            if (articulationChain[i] == joint)
            {
                //Debug.Log("Index retreived.");
                return i;
            }
        }
        return -1;
=======
            for (int index = 0; index < articulationChain.Length; index++)
            {
                Debug.Log(MyJointNames[index]);
                Debug.Log(JointState.name[index]);
                Debug.Log("----------------------------");

                if (MyJointNames[index] == JointState.name[index])
                {
                    Debug.Log("Joint index found");
                    currentDrive = articulationChain[index].xDrive;
                    currentDrive.target = (float)JointState.position[index];
                    articulationChain[index].xDrive = currentDrive;
                }
                else if (MyJointNames[index] != JointState.name[index])
                {
                    Debug.Log("Joint index found 2");
                    int real_index = FindCorrectIndex(MyJointNames[index]);

                    currentDrive = articulationChain[real_index].xDrive;
                    currentDrive.target = (float)JointState.position[real_index];
                    articulationChain[real_index].xDrive = currentDrive;
                }
            }
        }
    }

    // Matches the correct joint with it's mapped position in the position array
    private int FindCorrectIndex(string jointName)
    {
        int real_index = 0;
        for(int seeker = 0; seeker < JointState.name.Length; seeker++)
        {
            if (jointName == JointState.name[seeker])
            {
                real_index = seeker;
                return real_index;
            }
        }
        return real_index;
>>>>>>> Stashed changes
    }
}

    //public void FixedUpdate()
    //{
    //    speed = controller.speed;
    //    torque = controller.torque;
    //    acceleration = controller.acceleration;

    //    if(joint.jointType != ArticulationJointType.FixedJoint)
    //    {
    //        ArticulationDrive currentDrive = joint.xDrive;
    //        float newTargetDelta = Time.fixedDeltaTime * speed; // calculates how much the joint should change

    //        if (joint.jointType == ArticulationJointType.RevoluteJoint)
    //        {
    //            if (joint.twistLock == ArticulationDofLock.LimitedMotion)
    //            {
    //                if (newTargetDelta + currentDrive.target > currentDrive.upperLimit)
    //                {
    //                    currentDrive.target = currentDrive.upperLimit;
    //                }
    //                else if (newTargetDelta + currentDrive.target < currentDrive.lowerLimit)
    //                {
    //                    currentDrive.target = currentDrive.lowerLimit;
    //                }
    //                else
    //                {
    //                    currentDrive.target += newTargetDelta;
    //                }
    //            }
    //            else
    //            {
    //                currentDrive.target += newTargetDelta;
    //            }
    //        }

    //        else if (joint.jointType == ArticulationJointType.PrismaticJoint)
    //        {
    //            if (joint.linearLockX == ArticulationDofLock.LimitedMotion)
    //            {
    //                if (newTargetDelta + currentDrive.target > currentDrive.upperLimit)
    //                {
    //                    currentDrive.target = currentDrive.upperLimit;
    //                }
    //                else if (newTargetDelta + currentDrive.target < currentDrive.lowerLimit)
    //                {
    //                    currentDrive.target = currentDrive.lowerLimit;
    //                }
    //                else
    //                {
    //                    currentDrive.target += newTargetDelta;
    //                }
    //            }
    //            else
    //            {
    //                currentDrive.target += newTargetDelta;

    //            }
    //        }
    //        joint.xDrive = currentDrive;
    //    }
    //}
