using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    public float speed = 5f; // The speed of the helicopter
    public float maxAltitude = 100f; // The maximum altitude the helicopter can reach
    public float minAltitude = 50f; // The minimum altitude the helicopter can reach
    public float horizontalRange = 50f; // The maximum horizontal distance the helicopter can move from its starting position
    public float smoothTime = 0.5f; // The smoothing time for the helicopter movement

    private Vector3 startingPosition; // The starting position of the helicopter
    private Vector3 targetPosition; // The target position of the helicopter

    private Vector3 velocity = Vector3.zero; // The velocity used for smoothing
    private Quaternion targetRotation; // The target rotation of the helicopter

    void Start()
    {
        startingPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            // Set a new target position and rotation for the helicopter to fly towards
            targetPosition = GetRandomPosition();
            targetRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
        }

        // Move the helicopter smoothly towards the target position using SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Rotate the helicopter smoothly towards the target rotation using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);

    }

    private Vector3 GetRandomPosition()
    {
        // Calculate a random position for the helicopter to fly towards
        Vector3 newPosition = startingPosition + Random.insideUnitSphere * horizontalRange;
        newPosition.y = Random.Range(minAltitude, maxAltitude);
        return newPosition;
    }
}
