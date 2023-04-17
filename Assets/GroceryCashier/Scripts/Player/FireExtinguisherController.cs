using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    public GameObject fireExtinguisher;
    public float pickupDistance = 2.0f;
    public KeyCode pickupKey = KeyCode.F;
    public KeyCode holsterKey = KeyCode.H;

    private bool isPickedUp = false;
    private bool isEnabled = false;
    private GameObject pickupItem;

    void Start()
    {
        fireExtinguisher.SetActive(false);
    }

    void Update()
    {
        if (!isPickedUp && pickupItem != null && Vector3.Distance(transform.position, pickupItem.transform.position) <= pickupDistance && Input.GetKeyDown(pickupKey))
        {
            isPickedUp = true;
            pickupItem.SetActive(false);
            fireExtinguisher.SetActive(true);
            isEnabled = true;
        }
        else if (isPickedUp && Input.GetKeyDown(holsterKey))
        {
            isEnabled = !isEnabled;
            fireExtinguisher.SetActive(isEnabled);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPickedUp && other.gameObject.CompareTag("FireExtinguisher"))
        {
            pickupItem = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isPickedUp && other.gameObject.CompareTag("FireExtinguisher"))
        {
            pickupItem = null;
        }
    }
}
