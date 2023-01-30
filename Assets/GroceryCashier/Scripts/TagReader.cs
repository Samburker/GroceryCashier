using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TagReader : MonoBehaviour
{
    public enum ReaderDirections { One, Five, Nine }
    public ReaderDirections rayType;
    [Range(0f, 90f)] public float rayAngle = 25f;
    public float rayLenght = 0.3f;
    public LayerMask rayLayerMask;
    [Range(0f, 1f)] public float rayCooldown = .1f;
    private GameObject currentObject;
    public UnityEvent<GameObject> OnObjectDetection;

    private float cooldownTimer;
    private GameObject lastObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Doing raycast
        foreach (RaycastHit hit in TargetObjects())
        {
            currentObject = hit.collider.gameObject;
            cooldownTimer = rayCooldown;
        }

        // Invokeing OnDetection
        if (currentObject != null && currentObject != lastObject)
            OnObjectDetection.Invoke(currentObject);

        // Storing old object so it dosent get invoked again
        if (currentObject != null)
            lastObject = currentObject;


        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
        else
        {
            currentObject = null;
            lastObject = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (Quaternion direction in RayDirections(rayType))
        {
            Gizmos.DrawLine(transform.position, transform.position + (direction * transform.forward * rayLenght));
        }

        Gizmos.color = Color.green;
        foreach (RaycastHit hit in TargetObjects())
        {
            Gizmos.DrawLine(transform.position, (hit.point));

            //Gizmos.DrawRay(hit);
        }
    }

    private IEnumerable<RaycastHit> TargetObjects()
    {
        foreach (Quaternion direction in RayDirections(rayType))
        {
            Ray r = new Ray(transform.position, direction * transform.forward);
            if (Physics.Raycast(r, out RaycastHit ray, rayLenght, rayLayerMask.value))
            {
                yield return ray;
            }
        }
    }

    private IEnumerable<Quaternion> RayDirections(ReaderDirections rayType)
    {
        switch (rayType)
        {
            case ReaderDirections.One:
                yield return Quaternion.Euler(0, 0, 0);
                break;
            case ReaderDirections.Five:
                yield return Quaternion.Euler(0, 0, 0);
                yield return Quaternion.Euler(rayAngle, rayAngle, 0);
                yield return Quaternion.Euler(-rayAngle, rayAngle, 0);
                yield return Quaternion.Euler(rayAngle, -rayAngle, 0);
                yield return Quaternion.Euler(-rayAngle, -rayAngle, 0);
                break;
            case ReaderDirections.Nine:
                yield return Quaternion.Euler(0, 0, 0);
                yield return Quaternion.Euler(rayAngle, rayAngle, 0);
                yield return Quaternion.Euler(-rayAngle, rayAngle, 0);
                yield return Quaternion.Euler(rayAngle, -rayAngle, 0);
                yield return Quaternion.Euler(-rayAngle, -rayAngle, 0);
                yield return Quaternion.Euler(0, rayAngle, 0);
                yield return Quaternion.Euler(0, -rayAngle, 0);
                yield return Quaternion.Euler(rayAngle, 0, 0);
                yield return Quaternion.Euler(-rayAngle, 0, 0);
                break;
        }
    }
}
