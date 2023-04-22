using UnityEngine;

public class CleanVomit : MonoBehaviour
{
    public GameObject broom;
    public float cleaningSpeed = 1f;
    public float cleaningThreshold = 0.95f;

    private bool isCleaning;
    private float cleaningProgress;
    private Collider puddleCollider;

    private void Start()
    {
        puddleCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("CLEAN CLEAN");
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
                Clean();
            }
        }
    }

    private void Clean()
    {
        Destroy(gameObject);
        // add code to play a cleaning sound, give points to the player, etc.
    }
}
