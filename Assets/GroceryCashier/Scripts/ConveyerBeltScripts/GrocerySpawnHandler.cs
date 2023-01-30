using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrocerySpawnHandler : MonoBehaviour
{

    public GameObject GrocerySpawner;
    public GameObject Watermelon;
    public Transform Position;

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Destroy(other.gameObject);
            Instantiate(Watermelon, GrocerySpawner.transform.position, Quaternion.identity);        
        }
    }
}
