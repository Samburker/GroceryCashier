using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFire : MonoBehaviour
{
    public string fireTag = "Fire"; // the tag of the objects to be destroyed
    private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("exhaust"))
            {
                Destroy(gameObject);
            }
        }
    }
