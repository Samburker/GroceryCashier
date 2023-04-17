using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ShopInventory inventoryPreset;

    List<ShoppingItem> inventory;

    public static InventoryManager Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void OnEnable()
    {
        PopulateInventory(inventoryPreset);
    }

    private void PopulateInventory(ShopInventory inventoryPreset)
    {
        inventory = new List<ShoppingItem>(inventoryPreset.items);
    }


}
