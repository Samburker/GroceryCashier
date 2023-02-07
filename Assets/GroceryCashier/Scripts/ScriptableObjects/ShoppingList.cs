using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShoppingList", menuName = "McGreedy/ShoppingList", order = 0)]
public class ShoppingList : ScriptableObject
{
    public int wantedItemsMin;
    public int wantedItemsMax;

    public List<ShoppingListItem> items;

    [System.Serializable]
    public class ShoppingListItem
    {
        public bool required;
        public bool steal;
        [Range(0, 1f)] public float probability;
        public List<ShoppingItem> variants;
    }
}
