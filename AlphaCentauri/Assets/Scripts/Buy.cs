using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Buy : MonoBehaviour
{

    [SerializeField] TMP_Text Price, QuantityText;
    [SerializeField] Image ImageObject;
    Action<ItemRegistry.BuyItemType> updateSidepanel;
    ItemRegistry.BuyItemType buyItemType;
    public void SetUI(ItemRegistry.BuyItemType buyItemType, Action<ItemRegistry.BuyItemType> updateSidepanel, int quantity)
    {
        BuyItem buyItem = ItemRegistry.BuyItems[buyItemType];
        this.buyItemType = buyItemType;
        ImageObject.GetComponent<Image>().sprite = buyItem.Image;
        Price.text = buyItem.Price.ToString();
        this.updateSidepanel = updateSidepanel;
        if (quantity > 0)
        {

            QuantityText.text = quantity.ToString();
        }
        else
        {
            QuantityText.text = " ";
        }
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

