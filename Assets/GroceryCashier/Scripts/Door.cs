using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class Door : MonoBehaviour, IGrabbable
{
    [SerializeField] bool isLocked = false;
    private Rigidbody rb;
    private HingeJoint hj;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hj = GetComponent<HingeJoint>();
        if (isLocked)
            LockDoor();
        else
            UnlockDoor();
    }

    internal void LockDoor()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    internal void UnlockDoor()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    public void StartGrab()
    {
        if (isLocked)
            return;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void EndGrab()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
    }
}
