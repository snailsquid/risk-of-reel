using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSwitchItem : MonoBehaviour
{
    ItemRegistry.BuyItemType bait;
    ItemManager itemManager;
    [SerializeField] int index;
    [SerializeField] Transform image;
    [SerializeField] TMP_Text quantityText;
    QuickSwitch quickSwitch;
    void Start()
    {
        quickSwitch = transform.parent.GetComponent<QuickSwitch>();
        itemManager = GameObject.Find("GameManager").transform.GetComponent<ItemManager>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void SetBait(ItemRegistry.BuyItemType bait, int index)
    {
        this.bait = bait;
        this.index = index;
        image.GetComponent<Image>().sprite = ItemRegistry.BuyItems[bait].Image;
        UpdateQuantity();
    }
    public void UpdateQuantity()
    {
        if (bait == ItemRegistry.BuyItemType.None)
        {
            quantityText.text = "";
            return;
        };
        quantityText.text = itemManager.inventory.Items[bait].Quantity.ToString();
    }
    void OnClick()
    {
        if (index == quickSwitch.baitIndex)
        {
            quickSwitch.ToggleContainer();
        }
        else
        {
            quickSwitch.SwitchBait(index);
            quickSwitch.ToggleContainer();
        }
    }
}
