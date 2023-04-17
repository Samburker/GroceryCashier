using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ShoppingItem", menuName = "McGreedy/ShoppingItem", order = 1)]
public class ShoppingItem : ScriptableObject
{
    public enum PriceType { Unit, Weight }
    public string itemName;
    public GameObject prefab;
    public bool restricted;
    public float itemPrice;
    public PriceType priceType;
    public float itemWeightMin;
    [FormerlySerializedAs("itemWeight")]
    public float itemWeightMax;

    public void Instantiate(Vector3 position, Quaternion rotation)
    {
        GameObject go = Instantiate(prefab);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.localScale = Vector3.one;

        var tag = go.AddComponent<PriceTag>();
        tag.item = this;
    }
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemName))
            itemName = name;
    }
}
