using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sidepanel : MonoBehaviour
{
    [SerializeField] TMP_Text Name, Description;
    [SerializeField] Button BuyButton, EquipButton;
    [SerializeField] Transform GameManager;
    ItemManager itemManager;
    Action<ItemRegistry.BuyItemType> callback;
    ItemRegistry.BuyItemType buyItemType;
    public void SetUI(ItemRegistry.BuyItemType buyItemType, Action<ItemRegistry.BuyItemType> callback)
    {
        BuyItem item = ItemRegistry.BuyItems[buyItemType];
        this.buyItemType = buyItemType;
        this.callback = callback;
        Name.text = item.Name;
        Description.text = item.Description;
        // BuyButton.GetComponentInChildren<TMP_Text>().text = "Buy (" + item.Price + ")";
    }
    public void Equip()
    {
        itemManager.AddToLineup(buyItemType);
    }
    void Start()
    {
        itemManager = GameManager.GetComponent<ItemManager>();
        BuyButton.onClick.AddListener(() => { callback(buyItemType); });
        EquipButton.onClick.AddListener(Equip);

    }
}
