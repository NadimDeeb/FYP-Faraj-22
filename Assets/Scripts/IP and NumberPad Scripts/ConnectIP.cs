using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

public class ConnectIP : MonoBehaviour
{
    public ROSConnection ros;
    [SerializeField]
    string ros_IP;
    public string ROSIP { get => ros_IP; set => ros_IP = value; }

    // Start is called before the first frame update, Connect on start if IP did not change since last time
    void Start()
    {
        ros_IP = ros.RosIPAddress;

        if (ros.ConnectOnStart)
        {
            ros.Connect(ros_IP, ros.RosPort);
        }
    }

    // Connect in case of chaging IP address (called when thumbs up pressed)
    public void DynamicConnect(string inputIP)
    {
        ros_IP = inputIP;
        ros.Disconnect();
        ros.Connect(ros_IP, ros.RosPort);
    }
}
