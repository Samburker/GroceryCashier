using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShoppingItem", menuName = "McGreedy/ShoppingItem", order = 1)]
public class ShoppingItem : ScriptableObject
{
    public enum PriceType { Unit, Weight }

    public GameObject prefab;
    public bool restricted;
    public float itemPrice;
    public PriceType priceType;
    public float itemWeight;
}
