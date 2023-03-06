using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Singleton { get; private set; }
    public GroceryCustomer[] customerPrefabs;

    internal ShoppingList shoppingList;
    private List<GroceryCustomer> _customers = new List<GroceryCustomer>();

    private void Awake()
    {
        Singleton = this;
    }

    internal void Spawn()
    {
        SceneDescriptor _sceneDescriptor = GameManager.Singleton.sceneDescriptor;
        GroceryCustomer customer = Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Length)]);
        customer.shoppingList = shoppingList;
        customer.SetPosition(_sceneDescriptor.customerSpawnpoints[Random.Range(0, _sceneDescriptor.customerSpawnpoints.Length)]);
    }

    internal void OnCustomerSpawn(GroceryCustomer c)
    {
        _customers.Add(c);
        Debug.Log("Customer Spawned\n" + c.ToString());
    }

    internal void OnCustomerDespawn(GroceryCustomer c)
    {
        _customers.Remove(c);
        Debug.Log("Customer Despawned\n" + c.ToString());
    }

    internal int CustomerCount()
    {
        return _customers.Count;
    }

}
