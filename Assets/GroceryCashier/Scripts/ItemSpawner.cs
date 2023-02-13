using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public void SpawnItems(IEnumerable<ShoppingItem> items, Action callback)
    {
        StartCoroutine(SpawnItemsCoroutine(items, callback));
    }

    private IEnumerator SpawnItemsCoroutine(IEnumerable<ShoppingItem> items, Action callback = null)
    {
        // Iterating item list
        foreach (ShoppingItem item in items)
        {
            // Instantiating item prefabs
            GameObject go = Instantiate(item.prefab, transform.position, transform.rotation);

            // Setting pricetag
            var tag = go.AddComponent<PriceTag>();

            // TODO: Create collider and visible indigator for pricetag

            // Assigning info to pricetag
            tag.item = item;

            // Waiting 0.25 seconds to avoid spawning next one inside of the last one
            yield return new WaitForSeconds(0.25f);
        }
        callback?.Invoke();
    }
}
