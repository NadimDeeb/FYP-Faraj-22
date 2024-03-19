using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Robotics.ROSTCPConnector;

public class NumberPadInput : MonoBehaviour
{
    [SerializeField] SetIPButtonText setIPButtonText;

    public TextMeshPro inputField;
    public ROSConnection ros;
    public ConnectIP connectIP;
    private string ipAddress;
    private static NumberPadInput _instance;
    public static NumberPadInput Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inputField.text = ros.RosIPAddress; // Set IP address to the one saved in ROSConnection
        ipAddress = ros.RosIPAddress; // Set IP address to the one saved in ROSConnection 
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update IP address when function is called
    void UpdateInputField()
    {
        inputField.text = ipAddress;
    }

    public void OnKeyPressedEvent(string key)
    {
        if (key.Equals("Clear"))
        {
            Clear();
        }
        else if (key.Equals("Enter"))
        {
            Enter();
        }
        else if (key.Equals("Backspace"))
        {
            Backspace();
        }
        else if (ipAddress.Length < 20)
        {
            ipAddress += key;
            UpdateInputField();
        }
        else
        {
            Debug.LogWarning("[NumberPadInput]: Invalid name of key pressed " + key);
        }
    }

    // Clear the input
    public void Clear()
    {
        ipAddress = string.Empty;
        UpdateInputField();
    }

    // Delete single character
    public void Backspace()
    {
        if (ipAddress.Length != 0)
        {
            ipAddress = ipAddress.Remove(ipAddress.Length - 1, 1);
        }
        UpdateInputField();
    }

    // Save IP change
    public void Enter()
    {
        connectIP.DynamicConnect(ipAddress);
        setIPButtonText.CheckConnectionStatus();
    }
}
