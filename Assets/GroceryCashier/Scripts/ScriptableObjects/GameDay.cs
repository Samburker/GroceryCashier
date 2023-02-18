using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDay", menuName = "McGreedy/GameDay", order = 110)]
public class GameDay : ScriptableObject
{
    [Header("Relations")]
    public ShopInventory shopInventory;
    public ShoppingList shoppingList;



    [Header("Worktime")]
    [ContextMenuItem("Morning", "StartMorning", order = 0)]
    [ContextMenuItem("Midday", "StartMidday", order = 1)]
    [ContextMenuItem("Evening", "StartEvening", order = 2)]
    public float startTime;
    [ContextMenuItem("Morning", "EndMorning", order = 10)]
    [ContextMenuItem("Midday", "EndMidday", order = 11)]
    [ContextMenuItem("Evening", "EndEvening", order = 12)]
    public float endTime;

    [Header("Working")]
    [Multiline] public string messageOfTheDay;
    public bool cigarets;
    public bool security;


    private const float dayOfset = 0.25f;
    void StartMorning() => startTime = 7f - dayOfset;
    void StartMidday() => startTime = 14f - dayOfset;
    void StartEvening() => startTime = 22f - dayOfset;
    void EndMorning() => endTime = 7f + dayOfset;
    void EndMidday() => endTime = 14f + dayOfset;
    void EndEvening() => endTime = 22f + dayOfset;
}
