using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wudang.Temple.Control;
using Unity.Robotics;
using Unity.Robotics.UrdfImporter.Control;

public class ArtBodyManager : MonoBehaviour
{
    private ArticulationBody[] ArtBods;
    private OogwayController Oogway;
    private Controller Controller;
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
            if (child.name == "base_link_base_theta")
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

                if(child.name == "base_theta_base_y" || child.name == "base_y_base_x")
                {
                    newArticulationBody.immovable = true;
                }
            }
            else if (child.GetComponent<Flag>().is_prefab_flag)
            {
                continue;
            }

            // Recursively call the function for children of the current child
            AttachArticulationBodiesRecursively(child);
        }
        Debug.Log("Art bod added");
    }
}
