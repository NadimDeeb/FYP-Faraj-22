using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class KeyFunction : MonoBehaviour
{
    public string keyName;
    public Interactable interactable;
    public NumberPadInput numberPadInput;

    void Start()
    {
        keyName = gameObject.name; 
    }

    public void OnClick()
    {
        numberPadInput.OnKeyPressedEvent(keyName);
    }

    private void OnEnable()
    {
        interactable.OnClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        interactable.OnClick.RemoveListener(OnClick);
    }
}
