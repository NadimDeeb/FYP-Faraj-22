using RosMessageTypes.Sensor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShifuControl : MonoBehaviour
{
    //Scripts
    private Wudang.Temple.Control.OogwayController Oogway;

    //Enums
    public Wudang.Temple.Control.Rotation direction;

    //Arrays
    ArticulationBody[] articulationChain;

    //Variables
    public ArticulationDrive currentDrive;
    public JointStateMsg JointState;

    public float speed;
    public float torque;
    public float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Shifu called");
        // Controller
        Oogway = (Wudang.Temple.Control.OogwayController)this.GetComponentInParent(typeof(Wudang.Temple.Control.OogwayController));

        //Enums
        direction = 0;

        //ROS messages
        JointState = new JointStateMsg();

        //Variables
        articulationChain = Oogway.articulationChain;

    }

    private void FixedUpdate()
    {
        JointState = Oogway.JointState;
        speed = Oogway.speed;
        torque = Oogway.torque;
        acceleration = Oogway.acceleration;

        if (articulationChain.Length > 0 && JointState.position.Length > 0)
        {
            for (int index = 0; index < articulationChain.Length; index++)
            {
                currentDrive = articulationChain[index].xDrive;
                currentDrive.target = (float)JointState.position[index];
                articulationChain[index].xDrive = currentDrive;
            }
        }
    }
}
