using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VufoOFF : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VuforiaBehaviour>().enabled = false;
    }

}
