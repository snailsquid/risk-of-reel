using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Buy : MonoBehaviour
{

    [SerializeField] TMP_Text Price;
    [SerializeField] Image ImageObject;
    Action<ItemRegistry.BuyItemType> updateSidepanel;
    ItemRegistry.BuyItemType buyItemType;
    public void SetUI(ItemRegistry.BuyItemType buyItemType, Action<ItemRegistry.BuyItemType> updateSidepanel)
    {
        BuyItem buyItem = ItemRegistry.BuyItems[buyItemType];
        this.buyItemType = buyItemType;
        ImageObject.GetComponent<Image>().sprite = buyItem.Image;
        Price.text = buyItem.Price.ToString();
        this.updateSidepanel = updateSidepanel;
    }
    void OnClick()
    {
        Debug.Log("update sidepanel");
        updateSidepanel(buyItemType);
    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
}

