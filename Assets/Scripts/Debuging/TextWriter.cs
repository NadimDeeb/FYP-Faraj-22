using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Unity.Robotics.ROSTCPConnector;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class TextWriter : MonoBehaviour
{
    public TMP_Text texet;
    public GameObject image;
    public GameObject pln;
    public ROSConnection ros;
    public ROS_Subscriber sub;
    public GameObject ure;
    ArticulationBody aaa;
    bool boolian;

    [NonSerialized]
    public string meshName;

    // Update is called once per frame
    void Update()
    {
        texet.text = "ArtBod:" + boolian + "\n Pos:" + ure.transform.position;
    }
}
