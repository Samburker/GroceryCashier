using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GroceryCustomer[] customerPrefabs;
    public Transform[] customerSpawnpoints;

    private List<GroceryCustomer> _customers = new List<GroceryCustomer>();

    internal void Spawn(ShoppingList shoppingList)
    {
        GroceryCustomer customer = Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Length)]);
        customer.customerManager = this;
        customer.shoppingList = shoppingList;
        customer.SetPosition(customerSpawnpoints[Random.Range(0, customerSpawnpoints.Length)]);
        _customers.Add(customer);
        customer.OnDespawn += OnDespawn;
    }

    internal SceneDescriptor sceneDescriptor;

    private void OnDespawn(GroceryCustomer obj)
    {
        _customers.Remove(obj);
        Destroy(obj.gameObject);
        Debug.Log("Customer despawned\n" + obj.ToString());
    }

    internal int CustomerCount()
    {
        return _customers.Count;
    }
}
