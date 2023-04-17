using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CashRegister : MonoBehaviour
{
    [Header("Cash Register Components")]
    public TagReader tagReader;

    [Header("Display")]
    public GameObject itemList;
    public RegisterLine itemListLinePrefab;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip okAudio;
    public AudioClip notificationAudio;

    [Header("Gameplay")]
    public string priceUnit = "€";
    private ScrollRect scroll;
    public CheckoutCounterDesk checkoutCounter;
    public ItemSpawner itemSpawner;

    [SerializeField] private Transform[] queueSpots;
    private List<GroceryCustomer> customerPaymentQueue = new List<GroceryCustomer>();
    private GroceryCustomer currentCustomer;
    private int itemsScanned;

    // Start is called before the first frame update
    void Start()
    {
        ResetSales();
        scroll = itemList.GetComponentInParent<ScrollRect>();
        scroll.verticalNormalizedPosition = 0;
    }

    public void ResetSales()
    {
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);
        itemsScanned = 0;
        itemSpawner.DespawnAll();
    }

    private void OnEnable()
    {
        tagReader.OnTagDetected += OnTagDetected;
    }

    private void OnDisable()
    {
        tagReader.OnTagDetected -= OnTagDetected;
    }

    private void OnTagDetected(PriceTag tag)
    {
        // Beep
        if(audioSource)
            audioSource.PlayOneShot(okAudio);

        // Adding line to display
        RegisterLine line = Instantiate(itemListLinePrefab, itemList.transform);
        line.itemName.text = tag.item.name;
        switch (tag.item.priceType)
        {
            case ShoppingItem.PriceType.Unit:
                line.itemPrice.text = Mathf.Round((tag.item.itemPrice * 100)) / 100f + " " + priceUnit;
                break;
            case ShoppingItem.PriceType.Weight:
                line.itemPrice.text = Mathf.Round((tag.item.itemPrice * tag.ItemWeight * 100))/100f + " " + priceUnit;
                break;
        }

        //Scolling down
        scroll.normalizedPosition = new Vector2(0, 0);

        itemsScanned++;

        if(currentCustomer != null && itemsScanned >= currentCustomer.itemsWanted)
        {
            ResetSales();
            CustomerPaymentDone();
        }
    }

    internal Transform GetQueueSpot(int v)
    {
        return queueSpots[v];
    }


    internal int GetQueue(GroceryCustomer groceryCustomer)
    {
        if(customerPaymentQueue.Contains(groceryCustomer))
        {
            int index = customerPaymentQueue.FindIndex(c => c == groceryCustomer);
            if (index == 0 && currentCustomer == null)
            {
                customerPaymentQueue.RemoveAt(index);
                currentCustomer = groceryCustomer;
                index = -1;
            }
            return index;
        }
        return -1;
    }

    internal void Enqueue(GroceryCustomer groceryCustomer)
    {
        customerPaymentQueue.Add(groceryCustomer);
    }

    internal void PaymentStart(GroceryCustomer groceryCustomer)
    {
        currentCustomer = groceryCustomer;
        itemSpawner.SpawnItems(groceryCustomer.shoppingList.GetItems(currentCustomer.itemsWanted), () => {
            Debug.Log("Items spawned");
        });
    }

    internal void CustomerPaymentDone()
    {
        currentCustomer.paymentDone = true;
        currentCustomer = null;
    }
}
