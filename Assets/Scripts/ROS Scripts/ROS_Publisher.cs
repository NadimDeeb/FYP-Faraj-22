using System.Collections;
using UnityEngine;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;
using RosMessageTypes.Visualization;
using Microsoft.MixedReality.Toolkit.UI;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine.Rendering;

public class ROS_Publisher : MonoBehaviour
{
    // ROS variables
    private ROSConnection RosConnection;
    private string MovementTopic = "/cmd_vel";

    // Messages to be published
    private TwistMsg RobotMovTwist;
    public MarkerArrayMsg MarkerArray;
    private TwistMsg TwistFromMarker;
    private Vector3Msg positionVector;
    private Vector3Msg orientationVector;

    // External GameObjects
    public PinchSlider SliderRightLeft;
    public PinchSlider SliderForwardBack;
    public GameObject SliderParent;

    // Variables
    private float mid_value = 0.5f;
    private float max_value = 1.0f;
    private float min_value = 0.0f;
    private float RL_value;
    private float FB_value;
    private bool movement_coroutine_running = false;

    private void Start()
    {
        // Initialize ROS connection and publisher
        RosConnection = ROSConnection.GetOrCreateInstance();
        RosConnection.RegisterPublisher<TwistMsg>(MovementTopic);

        // Initialize Twist message
        RobotMovTwist = new TwistMsg();
        MarkerArray = new MarkerArrayMsg();
        TwistFromMarker = new TwistMsg();
        positionVector = new Vector3Msg();
        orientationVector = new Vector3Msg();

        // Subscribe to slider events
        SliderRightLeft.OnValueUpdated.AddListener(PublishMovementTwist);
        SliderForwardBack.OnValueUpdated.AddListener(PublishMovementTwist);
    }

    private IEnumerator PublishTwistRepeatedly()
    {
        while (true)
        {
            // Decide Twist message values
            DecideTwistMsg();

            // Assign values to Twist message
            RobotMovTwist.linear.x = FB_value;
            RobotMovTwist.angular.z = RL_value;

            // Stop the coroutine when the GameObject is disabled
            if (!SliderParent.activeInHierarchy)
            {
                StopCoroutine("PublishTwistRepeatedly");
                movement_coroutine_running = false;
                break;
            }

            // Publish Twist message
            RosConnection.Publish(MovementTopic, RobotMovTwist);
            Debug.Log("Published Twist - Linear: " + RobotMovTwist.linear.x + ", Angular: " + RobotMovTwist.angular.z);

            // Wait for 3 seconds before publishing again
            yield return new WaitForSeconds(3.0f);
        }
    }

    private void DecideTwistMsg()
    {
        if (SliderRightLeft.SliderValue == mid_value && SliderForwardBack.SliderValue == mid_value)
        {
            FB_value = 0.0f;
            RL_value = 0.0f;
        }
        else if (SliderRightLeft.SliderValue == max_value && SliderForwardBack.SliderValue == max_value)
        {
            FB_value = 0.5f;
            RL_value = 0.5f;
        }
        else if (SliderRightLeft.SliderValue == min_value && SliderForwardBack.SliderValue == min_value)
        {
            FB_value = -0.5f;
            RL_value = -0.5f;
        }
        else if (SliderRightLeft.SliderValue == mid_value && SliderForwardBack.SliderValue == max_value)
        {
            FB_value = 0.5f;
            RL_value = 0.0f;
        }
        else if (SliderRightLeft.SliderValue == mid_value && SliderForwardBack.SliderValue == min_value)
        {
            FB_value = -0.5f;
            RL_value = 0.0f;
        }
        else if (SliderRightLeft.SliderValue == max_value && SliderForwardBack.SliderValue == mid_value)
        {
            FB_value = 0.0f;
            RL_value = 0.5f;
        }
        else if (SliderRightLeft.SliderValue == min_value && SliderForwardBack.SliderValue == mid_value)
        {
            FB_value = 0.0f;
            RL_value = -0.5f;
        }
        else if (SliderRightLeft.SliderValue == min_value && SliderForwardBack.SliderValue == max_value)
        {
            FB_value = 0.5f;
            RL_value = -0.5f;
        }
        else if (SliderRightLeft.SliderValue == max_value && SliderForwardBack.SliderValue == min_value)
        {
            FB_value = -0.5f;
            RL_value = 0.5f;
        }
    }

    private void PublishMovementTwist(SliderEventData eventData)
    {
        // This method is invoked when the slider values are updated
        // Twist message publishing is handled by PublishTwistRepeatedly()
        // Also necessary for subscribing to slider events since enumerator functions cannot be subscribed to events
        // Fuck C#
        if (movement_coroutine_running == false)
        {
            movement_coroutine_running = true;
            StartCoroutine(PublishTwistRepeatedly());
        }
    }

    public void PublishFromMarkerArray()
    {
        foreach (var marker in MarkerArray.markers)
        {
            // Extracting position information
            PointMsg position = marker.pose.position;
            
            positionVector.x = position.x;
            positionVector.y = position.y;
            positionVector.z = position.z;

            // Extracting orientation information
            QuaternionMsg orientation = marker.pose.orientation;
            
            orientationVector.x = orientation.x;
            orientationVector.y = orientation.y;
            orientationVector.z = orientation.z;

            // Creating a Twist message from pose and orientation information
            TwistFromMarker.linear = positionVector;
            TwistFromMarker.angular = orientationVector;

            // Publish Twist message
            RosConnection.Publish(MovementTopic, TwistFromMarker);
            Debug.Log("Published Twist from Marker - Linear: " + TwistFromMarker.linear.x + ", Angular: " + TwistFromMarker.angular.z);
        }
    }
}