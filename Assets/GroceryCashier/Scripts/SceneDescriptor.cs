using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDescriptor : MonoBehaviour
{
    public GroceryFirstPersonController playerPrefab;
    public CashRegister[] cashRegisters;
    public GameObject[] playerSpawnpoints;
    public Transform[] randomItemPositions;
    public float respawnHeight = -10f;
    private CustomerManager _customerManager;

    public CustomerManager CustomerManager { get => _customerManager ?? GetCustomerManager(); }

    private CustomerManager GetCustomerManager()
    {
        _customerManager = GetComponent<CustomerManager>();
        _customerManager.sceneDescriptor = this;
        return _customerManager;
    }
}
