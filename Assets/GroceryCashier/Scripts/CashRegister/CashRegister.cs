using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        ResetSales();
    }

    public void ResetSales()
    {
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);
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
        if(audioSource)
            audioSource.PlayOneShot(okAudio);
        RegisterLine line = Instantiate(itemListLinePrefab, itemList.transform);
        line.itemName.text = tag.itemName;
        switch (tag.priceType)
        {
            case PriceTag.PriceType.Unit:
                line.itemPrice.text = Mathf.Round((tag.itemPrice * 100)) / 100f + " " + priceUnit;
                break;
            case PriceTag.PriceType.Weight:
                line.itemPrice.text = Mathf.Round((tag.itemPrice * tag.itemWeight * 100))/100f + " " + priceUnit;
                break;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
