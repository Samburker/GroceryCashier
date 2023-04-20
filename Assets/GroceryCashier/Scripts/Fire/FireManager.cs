using UnityEngine;

public class FireManager : MonoBehaviour
{
    public GameObject firePrefab; // The prefab to instantiate
    public int numFires = 10; // The number of fires to spawn
    public GameObject minXObj; // The object representing the minimum X position for the fire
    public GameObject maxXObj; // The object representing the maximum X position for the fire
    public GameObject minZObj; // The object representing the minimum Z position for the fire
    public GameObject maxZObj; // The object representing the maximum Z position for the fire
    public float firstFireDelay = 0f; // The delay before spawning the first fire
    public float fireSpawnInterval = 1f; // The interval between spawning new fires
    public float fireSpread = 1f; // The maximum distance between adjacent fires
    public float fireSeparation = 1f; // The minimum distance between adjacent fires

    private Vector3 minBounds;
    private Vector3 maxBounds;
    private Vector3 lastFirePosition;

    private void Start()
    {
        // Calculate the minimum and maximum bounds based on the empty game objects
        minBounds = new Vector3(minXObj.transform.position.x, transform.position.y, minZObj.transform.position.z);
        maxBounds = new Vector3(maxXObj.transform.position.x, transform.position.y, maxZObj.transform.position.z);

        // Spawn the first fire after the specified delay
        Invoke("SpawnFire", firstFireDelay);
    }

    private void SpawnFire()
    {
        if (transform.childCount >= numFires)
        {
            // Stop spawning fires if we have already reached the maximum number
            return;
        }

        // Check if all the fires have been destroyed
        bool allFiresDestroyed = true;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                allFiresDestroyed = false;
                break;
            }
        }

        if (allFiresDestroyed)
        {
            return;
        }

        // Generate a random starting point within the specified boundaries if this is the first fire
        Vector3 spawnPosition = lastFirePosition;
        if (spawnPosition == Vector3.zero)
        {
            spawnPosition = new Vector3(Random.Range(minBounds.x, maxBounds.x), transform.position.y, Random.Range(minBounds.z, maxBounds.z));
        }
        else
        {
            // Offset the spawn position based on the previous fire's position and the maximum spread
            Vector3 offset = new Vector3(Random.Range(-fireSpread, fireSpread), 0f, Random.Range(-fireSpread, fireSpread));
            offset.x = Mathf.Clamp(offset.x, -fireSeparation, fireSeparation);
            offset.z = Mathf.Clamp(offset.z, -fireSeparation, fireSeparation);
            spawnPosition = lastFirePosition + offset;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, minBounds.x, maxBounds.x);
            spawnPosition.z = Mathf.Clamp(spawnPosition.z, minBounds.z, maxBounds.z);
        }

        // Spawn a fire at the calculated position
        GameObject newFire = Instantiate(firePrefab, spawnPosition, Quaternion.identity);
        newFire.transform.SetParent(transform);

        // Update the last fire position
        lastFirePosition = spawnPosition;

        // Schedule the next fire spawn
        Invoke("SpawnFire", fireSpawnInterval);
    }


}
