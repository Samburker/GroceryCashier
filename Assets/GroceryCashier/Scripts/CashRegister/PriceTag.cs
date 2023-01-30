using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceTag : MonoBehaviour
{
    public enum PriceType { Unit, Weight }

    public string itemName;
    public float itemPrice;
    public PriceType priceType;
    public float itemWeight;
}
