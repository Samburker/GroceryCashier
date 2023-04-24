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

    [Tooltip("An array of cleaning sounds that will be randomly played when cleaning the puddle.")]
    public AudioClip[] cleaningSounds;

    [Tooltip("The sound that will be played when the puddle is completely cleaned.")]
    public AudioClip cleanedSound;

    private bool isCleaning;
    private float cleaningProgress;
    private float cleaningTimer;
    private Collider puddleCollider;
    private AudioSource broomAudioSource;
    private bool canPlaySound = true;

    private void Start()
    {
        puddleCollider = GetComponent<Collider>();
        broomAudioSource = broom.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == broom)
        {
            isCleaning = true;
            PlayRandomCleaningSound();
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

            if (canPlaySound)
            {
                PlayRandomCleaningSound();
                canPlaySound = false;
            }
        }
        else
        {
            cleaningProgress = 0f;
            cleaningTimer = 0f;
            canPlaySound = true;
        }
    }

    private void Clean()
    {
        PlayCleanedSound();
        Destroy(gameObject);
        // add code to give points to the player, etc.
    }

    private void PlayRandomCleaningSound()
    {
        if (cleaningSounds.Length > 0 && broomAudioSource != null)
        {
            int randomIndex = Random.Range(0, cleaningSounds.Length);
            broomAudioSource.PlayOneShot(cleaningSounds[randomIndex]);
            Invoke("EnableSound", cleaningSounds[randomIndex].length);
        }
    }

    private void PlayCleanedSound()
    {
        if (cleanedSound != null && broomAudioSource != null)
        {
            broomAudioSource.PlayOneShot(cleanedSound);
        }
    }

    private void EnableSound()
    {
        canPlaySound = true;
    }
}
