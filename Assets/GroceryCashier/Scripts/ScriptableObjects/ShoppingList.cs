using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShoppingList", menuName = "McGreedy/ShoppingList", order = 0)]
public class ShoppingList : ScriptableObject
{
    public int wantedItemsMin;
    public int wantedItemsMax;
    [Space(10)]
    [Header("Acceptable items")]
    public List<ShoppingListItem> items;

    [System.Serializable]
    public class ShoppingListItem
    {
        public bool required;
        public bool steal;
        [Tooltip("Probability %")]
        [Range(0, 100f)] public float probability;
        public List<ShoppingItem> variants;
    }

    internal IEnumerable<ShoppingItem> GetItems(int itemsWanted)
    {
        int count = 0;
        while (count < itemsWanted)
        {
            foreach (ShoppingListItem item in items)
            {
                if (UnityEngine.Random.Range(0, 1f) < item.probability)
                    continue; // Skip this item

                // Return random variant
                yield return item.variants[UnityEngine.Random.Range(0, item.variants.Count)];
                count++;
            }
        }
    }
}
