using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    public GameObject fireExtinguisher;
    public GameObject extinguisherParticles;
    public float pickupDistance = 2.0f;
    public KeyCode pickupKey = KeyCode.F;
    public KeyCode holsterKey = KeyCode.H;
    public KeyCode sprayKey = KeyCode.Mouse0;

    public AudioClip pickupSound;
    public AudioClip holsterSound;
    public AudioClip spraySound;

    private bool isPickedUp = false;
    private bool isEnabled = false;
    private GameObject pickupItem;
    private AudioSource audioSource;

    void Start()
    {
        fireExtinguisher.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isPickedUp && pickupItem != null && Vector3.Distance(transform.position, pickupItem.transform.position) <= pickupDistance && Input.GetKeyDown(pickupKey))
        {
            isPickedUp = true;
            pickupItem.SetActive(false);
            fireExtinguisher.SetActive(true);
            isEnabled = true;
            audioSource.PlayOneShot(pickupSound);
        }
        else if (isPickedUp && Input.GetKeyDown(holsterKey))
        {
            isEnabled = !isEnabled;
            fireExtinguisher.SetActive(isEnabled);
            audioSource.PlayOneShot(holsterSound);
        }

        if (isPickedUp && isEnabled && Input.GetKey(sprayKey))
        {
            extinguisherParticles.SetActive(true);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(spraySound);
            }
        }
        else
        {
            extinguisherParticles.SetActive(false);
            audioSource.Stop();
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
