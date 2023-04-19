using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFire : MonoBehaviour
{
    public string fireTag = "Fire"; // the tag of the objects to be destroyed

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("COLLISION DETECTED");
        if (other.CompareTag(fireTag))
        {
            Destroy(other.gameObject);
        }
    }
}
