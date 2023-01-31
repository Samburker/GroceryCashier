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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 is left click
        {
            if(heldObject == null) // if we don't have anything currently
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickupObject(hit.transform.gameObject); // this found object with raycast is not input to pickObj as parameter
                }
            }
            else
            {
                DropObject();
            }
        }
        if (heldObject != null)
        {
            MoveObject();
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if(pickObj.GetComponent<Rigidbody>())
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
        if(Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f )
        {
            Vector3 moveDirection = (holdArea.position - heldObject.transform.position);
            heldObjectRB.AddForce(moveDirection * pickupForce);
        }
    }

    void RotateObject()
    {
        if (Input.GetMouseButtonDown(1))
        {
            float XaxsisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float YaxsisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

            heldObject.transform.Rotate(Vector3.down, XaxsisRotation);
            heldObject.transform.Rotate(Vector3.right, YaxsisRotation);
        }
    }




}
