using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.Sensor;
using System;

namespace Wudang.Temple.Control
{
    public enum Rotation { None = 0 , Positive = 1, Negative = -1 };

    public class OogwayController : MonoBehaviour
    {
        // Scripts
        public ROS_Subscriber RosSub;
        private ShifuControl current;

        // Variables
        public ArticulationBody[] articulationChain_temp;
        public ArticulationBody[] articulationChain;
        double delta;
        public double[] position_deltas; // Clark hek beddo bas hay for direction
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
            Debug.Log("Oogway called!");
            articulationChain_temp = this.GetComponentsInChildren<ArticulationBody>();
            Debug.Log("ArtChainTempLen: " + articulationChain_temp.Length);
            JointState = new JointStateMsg();

            // Dropping elements from the articulationChain array
            articulationChain = new ArticulationBody[6];
            Array.Copy(articulationChain_temp, 1, articulationChain, 0, 6);

            Debug.Log("THE COPIED ARRAY IS LEN: " + articulationChain.Length);

            foreach (ArticulationBody joint in articulationChain)
            {
                joint.gameObject.AddComponent<ShifuControl>();

                joint.jointFriction = 10;
                joint.angularDamping = 10;
                ArticulationDrive currentDrive = joint.xDrive;
                currentDrive.forceLimit = 1000;
                joint.xDrive = currentDrive;
            }
        }

        public IEnumerator JointArticulator()
        {
            Debug.Log("JointsState Len: " + JointState.position.Length);
            Debug.Log("ArticulationChain Len: " + articulationChain.Length);

            // We can do this since we assume that the index in the position array is mapped to the joint name with the same index
            for (int index = 0; index < articulationChain.Length; index++)
            {
                delta = JointState.position[index] - articulationChain[index].xDrive.target;
                position_deltas[index] = delta;
                Debug.Log("Index clark:" + index);
            }

            for(int index = 0; index < position_deltas.Length; index++)
            {
                if (position_deltas[index] > 0) 
                {
                    current = articulationChain[index].GetComponent<ShifuControl>();
                    current.direction = Rotation.Positive;
                }
                else if (position_deltas[index] < 0)
                {
                    current = articulationChain[index].GetComponent<ShifuControl>();
                    current.direction = Rotation.Negative;
                }
                else
                {
                    current = articulationChain[index].GetComponent<ShifuControl>();
                    current.direction = Rotation.None;
                }
            }

            yield return null;
        }
    }
}
