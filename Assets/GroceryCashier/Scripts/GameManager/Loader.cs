using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Singleton == null)
            Instantiate(gameManagerPrefab);
        Destroy(gameObject);
    }
}
