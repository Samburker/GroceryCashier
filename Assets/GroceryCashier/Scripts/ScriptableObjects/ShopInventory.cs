using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopInventory", menuName = "McGreedy/Shop Inventory", order = 100)]
public class ShopInventory : ScriptableObject
{
    public List<ShoppingItem> items;
}
