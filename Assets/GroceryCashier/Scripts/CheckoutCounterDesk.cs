using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutCounterDesk : MonoBehaviour
{
    public GameObject cigaretteBelt;
    public GameObject cigaretteCover;
    public GameObject securityDisplay;
    public GameObject securityCanvas;
    public GameObject separatorObject;

    public bool cigarette;
    public bool security;
    public bool separator;

    private void Update()
    {
        UpdateStatus(true);
    }

    private void UpdateStatus(bool skipAnimation = false)
    {
        cigaretteBelt.SetActive(cigarette);
        cigaretteCover.SetActive(!cigarette);
        securityDisplay.SetActive(security);
        securityCanvas.SetActive(security);
        separatorObject.transform.rotation = separator ? Quaternion.Euler(-90, 0, 12f) : Quaternion.Euler(-90, 0, -12f);
    }


}
