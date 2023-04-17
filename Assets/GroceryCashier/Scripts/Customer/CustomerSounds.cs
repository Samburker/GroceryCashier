using UnityEngine;
using System.Collections;

public class CustomerSounds : MonoBehaviour
{
    public AudioClip[] soundArray;
    public float minDelay = 5f;
    public float maxDelay = 15f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            audioSource.PlayOneShot(soundArray[Random.Range(0, soundArray.Length)]);
        }
    }
}
