using UnityEngine;

public class CleanVomit : MonoBehaviour
{
    [Tooltip("The GameObject for the broom that will be used to clean the puddle.")]
    public GameObject broom;

    [Tooltip("The speed at which the puddle is cleaned. Higher values make the puddle clean faster.")]
    public float cleaningSpeed = 1f;

    [Tooltip("The percentage of the puddle that must be cleaned in order to complete the task.")]
    [Range(0f, 1f)]
    public float cleaningThreshold = 0.95f;

    [Tooltip("The amount of time (in seconds) that the player must spend cleaning the puddle in order to complete the task.")]
    public float cleaningTime = 2f;

    private bool isCleaning;
    private float cleaningProgress;
    private float cleaningTimer;
    private Collider puddleCollider;

    private void Start()
    {
        puddleCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == broom)
        {
            isCleaning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == broom)
        {
            isCleaning = false;
            cleaningProgress = 0f;
            cleaningTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (isCleaning)
        {
            float mouseMovement = Input.GetAxis("Mouse X");
            cleaningProgress += Mathf.Abs(mouseMovement) * cleaningSpeed * Time.fixedDeltaTime;

            if (cleaningProgress >= cleaningThreshold)
            {
                cleaningTimer += Time.fixedDeltaTime;
                if (cleaningTimer >= cleaningTime)
                {
                    Clean();
                }
            }
            else
            {
                cleaningTimer = 0f;
            }
        }
        else
        {
            cleaningProgress = 0f;
            cleaningTimer = 0f;
        }
    }

    private void Clean()
    {
        Destroy(gameObject);
        // add code to play a cleaning sound, give points to the player, etc.
    }
}
