using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PriceTag : MonoBehaviour
{
    public ShoppingItem item;
    private float _itemWeight = -1;

    internal float ItemWeight { get => GetItemWeight(); set => _itemWeight = value; }

    private float GetItemWeight()
    {
        if (_itemWeight > 0)
            return _itemWeight;
        _itemWeight = Random.Range(item.itemWeightMin, item.itemWeightMax);
        return _itemWeight;
    }
}
