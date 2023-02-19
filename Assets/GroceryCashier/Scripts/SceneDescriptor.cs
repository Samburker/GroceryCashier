using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDescriptor : MonoBehaviour
{
    public GroceryFirstPersonController playerPrefab;
    public GameObject[] customerSpawnpoints;
    public GameObject[] playerSpawnpoints;
    public CashRegister[] cashRegisters;
    public GroceryCustomer[] customers;
    public float respawnHeight = -10f;
}
