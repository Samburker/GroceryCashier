using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObject;
    private Rigidbody heldObjectRB;

    [Header("Pickup Settings")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;
    [SerializeField] private float rotationSpeed = 1f;
    private PlayerInputs _inputs;
    private bool _rotating;

    private void OnEnable()
    {
        StartCoroutine(LateEnable());
    }

    private IEnumerator LateEnable()
    {
        yield return new WaitUntil(() => PlayerInputs.Singleton != null);

        // Getting reference to input system
        _inputs = PlayerInputs.Singleton;
        _inputs.pickupButton += OnPickup;
        _inputs.rotateButton += OnRotate;
    }

    private void OnDisable()
    {
        _inputs.pickupButton -= OnPickup;
        _inputs.rotateButton -= OnRotate;
        _inputs.rotateMode = false;
    }

    private void OnPickup(bool obj)
    {
        if (!obj)
            return;
        if (heldObject == null)
            DoRaycast();
        else
            DropObject();

    }

    private void OnRotate(bool obj)
    {
        _rotating = obj;
    }

    private void DoRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
        {
            PickupObject(hit.transform.gameObject); // this found object with raycast is not input to pickObj as parameter
        }
    }

    private void Update()
    {
        if (heldObject != null)
        {
            MoveObject();
            RotateObject();
        }
        _inputs.rotateMode = heldObject != null && _rotating;
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjectRB = pickObj.GetComponent<Rigidbody>();
            heldObjectRB.useGravity = false;
            heldObjectRB.drag = 10;
            heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRB.transform.parent = holdArea;
            heldObject = pickObj;
        }
    }

    void DropObject()
    {
        heldObjectRB.useGravity = true;
        heldObjectRB.drag = 1;
        heldObjectRB.constraints = RigidbodyConstraints.None;

        heldObjectRB.transform.parent = null;
        heldObject = null;
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObject.transform.position);
            heldObjectRB.AddForce(moveDirection * pickupForce);
        }
    }

    void RotateObject()
    {
        if (_rotating)
        {
            Debug.Log("Rotating");
            float XaxsisRotation = _inputs.rotate.x * rotationSpeed;
            float YaxsisRotation = _inputs.rotate.y * rotationSpeed;

            heldObject.transform.Rotate(Vector3.down, XaxsisRotation);
            heldObject.transform.Rotate(Vector3.right, YaxsisRotation);
        }
    }

}
