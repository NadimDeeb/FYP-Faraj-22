using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wudang.Temple.Control;
using Unity.Robotics;
using Unity.Robotics.UrdfImporter.Control;
using System.Linq;

public class ArtBodyManager : MonoBehaviour
{
    public string[] MyJointNames;
    private OogwayController Oogway;
    private Controller Controller;
    private int jointCount;

    private void Start()
    {
        MyJointNames = new string[6];
        jointCount = 0;
    }

    public void EnableArmMovement()
    {
        AttachArticulationBodiesRecursively(transform);
        Oogway = this.GetComponent<OogwayController>();
        Oogway.enabled = true;
        Controller = GetComponent<Controller>();
        Controller.enabled = true;

    }

    private void AttachArticulationBodiesRecursively(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == "base_y_base_x")
            {
                // Skip processing this child, but continue with its children
                AttachArticulationBodiesRecursively(child);
                continue;
            }

            // Check if the child is not a prefab
            else if (child.GetComponent<Flag>() == null)
            {
                // Add ArticulationBody to the current child
                ArticulationBody newArticulationBody = child.gameObject.AddComponent<ArticulationBody>();

                // Set ArticulationBody properties
                newArticulationBody.mass = 4;
                newArticulationBody.anchorRotation = Quaternion.Euler(0, 0, 270);
                newArticulationBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                newArticulationBody.jointType = ArticulationJointType.RevoluteJoint;
                newArticulationBody.useGravity = false;

                // Set motion to limited for all axes
                newArticulationBody.linearLockX = ArticulationDofLock.LimitedMotion;
                newArticulationBody.linearLockY = ArticulationDofLock.LimitedMotion;
                newArticulationBody.linearLockZ = ArticulationDofLock.LimitedMotion;

                if(child.name == "base_theta_base_y" || child.name == "base_link_base_theta")
                {
                    newArticulationBody.immovable = true;
                }
            }
            else if (child.GetComponent<Flag>().is_prefab_flag)
            {
                continue;
            }

            if(child.name != "base_link_base_theta")
            {
                // Check if there is enough space in the array
                if (jointCount < MyJointNames.Length)
                {
                    MyJointNames[jointCount] = child.name; // Add child name to the array
                    jointCount++; // Increment the joint count
                    Debug.Log(child.name + " added to the chain.");
                }
                else
                {
                    Debug.LogError("Not enough space in MyJointNames array!"); // Log an error if there's not enough space
                }
            }

            // Recursively call the function for children of the current child
            AttachArticulationBodiesRecursively(child);
        }
    }
}
