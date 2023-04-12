using UnityEngine;
using System.Collections;

public class CustomerFootSteps : MonoBehaviour
{
    public AudioClip[] footstepSounds;
    public float minDelay = 0.2f;
    public float maxDelay = 0.5f;
    public float volume = 0.5f;
    private AudioSource audioSource;
    private Vector3 lastPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
        StartCoroutine(PlayFootsteps());
    }

    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            if (transform.position != lastPosition)
            {
                audioSource.volume = volume;
                audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
            }
            lastPosition = transform.position;
        }
    }
}
