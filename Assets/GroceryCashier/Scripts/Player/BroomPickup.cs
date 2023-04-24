using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomPickup : MonoBehaviour
{
    public GameObject broom;
    public float pickupDistance = 2.0f;
    public KeyCode pickupKey = KeyCode.F;
    public KeyCode holsterKey = KeyCode.H;

    public AudioClip pickupSound;
    public AudioClip holsterSound;

    private bool isPickedUp = false;
    private bool isEnabled = false;
    private GameObject pickupItem;
    private AudioSource playerAudioSource;
    private AudioSource extinguisherAudioSource;

    void Start()
    {
        broom.SetActive(false);
        playerAudioSource = GetComponent<AudioSource>();

        // Add a new audio source component to the fireExtinguisher object
        extinguisherAudioSource = broom.AddComponent<AudioSource>();
        extinguisherAudioSource.spatialBlend = 1.0f;
        extinguisherAudioSource.maxDistance = 10.0f;
    }

    void Update()
    {
        if (!isPickedUp && pickupItem != null && Vector3.Distance(transform.position, pickupItem.transform.position) <= pickupDistance && Input.GetKeyDown(pickupKey))
        {
            isPickedUp = true;
            pickupItem.SetActive(false);
            broom.SetActive(true);
            isEnabled = true;
            playerAudioSource.PlayOneShot(pickupSound);
        }
        else if (isPickedUp && Input.GetKeyDown(holsterKey))
        {
            isEnabled = !isEnabled;
            broom.SetActive(isEnabled);
            playerAudioSource.PlayOneShot(holsterSound);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPickedUp && other.gameObject.CompareTag("broom"))
        {
            pickupItem = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isPickedUp && other.gameObject.CompareTag("broom"))
        {
            pickupItem = null;
        }
    }
}
