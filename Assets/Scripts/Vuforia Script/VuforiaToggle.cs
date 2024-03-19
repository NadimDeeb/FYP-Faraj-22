using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VuforiaToggle : MonoBehaviour
{
    public void ToggleObjectDetection()
    {
        if (GetComponent<VuforiaBehaviour>().enabled == true)
        {
            GetComponent<VuforiaBehaviour>().enabled = false;
        }
        else
        {
            GetComponent<VuforiaBehaviour>().enabled = true;
        }
    }
}