using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VomitSpawner : MonoBehaviour
{
    public GameObject[] prefabs; // the prefabs to spawn
    public float spawnInterval = 1f; // how often to spawn the prefab
    public int maxSpawnCount = 10; // the maximum number of prefabs to spawn
    public float initialDelay = 0f; // the delay before the first prefab spawns
    public AudioClip[] spawnSounds; // an array of spawn sounds to choose from
    public AudioClip[] secondSounds; // an array of second sounds to choose from
    public float secondSoundDelay = 3f; // the delay before the second sound plays
    public AudioSource soundSource; // the audio source to use for playing sounds

    private int spawnCount = 0;
    private int currentSpawnIndex = 0;

    void Start()
    {
        // Deactivate all child objects initially
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        InvokeRepeating("ActivateNextPrefab", initialDelay, spawnInterval);
    }

    void ActivateNextPrefab()
    {
        if (spawnCount >= maxSpawnCount)
        {
            CancelInvoke("ActivateNextPrefab");
            return;
        }

        if (currentSpawnIndex >= prefabs.Length)
        {
            currentSpawnIndex = 0;
        }

        // check if there are already 2 active prefabs
        int activeCount = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                activeCount++;
            }
        }

        if (activeCount >= 2)
        {
            return;
        }

        GameObject currentPrefab = prefabs[currentSpawnIndex];

        if (!currentPrefab.activeSelf)
        {
            currentPrefab.SetActive(true);

            Vector3 spawnPosition = currentPrefab.transform.position;
            PlayRandomSound(spawnSounds, soundSource, spawnPosition);
            StartCoroutine(PlaySecondSound(secondSounds, soundSource, spawnPosition, secondSoundDelay));

            spawnCount++;
            currentSpawnIndex++;
        }
    }


    void PlayRandomSound(AudioClip[] clips, AudioSource source, Vector3 position)
    {
        if (clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.transform.position = position;
        source.PlayOneShot(clip);
    }

    IEnumerator PlaySecondSound(AudioClip[] clips, AudioSource source, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (clips.Length == 0) yield break;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.transform.position = position;
        source.PlayOneShot(clip);
    }
}
