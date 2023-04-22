using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFire : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
        {
        Debug.Log("COLLISION");
            if (other.CompareTag("exhaust"))
            {
                Destroy(gameObject);
            }
        }
    }
