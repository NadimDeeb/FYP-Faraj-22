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
    public float speed;
    public float torque;
    public float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        Oogway = (OogwayController)this.GetComponentInParent(typeof(OogwayController));
        joint = this.GetComponent<ArticulationBody>();
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
