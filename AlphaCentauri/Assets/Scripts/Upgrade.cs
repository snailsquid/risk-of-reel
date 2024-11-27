using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] TMP_Text Name, Price;
    [SerializeField] Transform ImageObject;
    [SerializeField] Sprite filled, empty;
    [SerializeField] Image l1, l2, l3;
    [SerializeField] ItemManager itemManager;
    Action<ItemRegistry.UpgradeItemType> upgradeCallback;
    ItemRegistry.UpgradeItemType upgradeItemType;
    int level;
    public void Start()
    {
        itemManager = GameObject.Find("GameManager").GetComponent<ItemManager>();
        Debug.Log(itemManager);
    }
    public void SetUI(ItemRegistry.UpgradeItemType upgradeItemType, Action<ItemRegistry.UpgradeItemType> upgradeCallback)
    {
        itemManager = GameObject.Find("GameManager").GetComponent<ItemManager>();

        UpgradeItem upgradeItem = itemManager.shop.UpgradeItems[upgradeItemType];
        Name.text = upgradeItem.Name;
        ImageObject.GetComponent<Image>().sprite = upgradeItem.Image[0];
        if (upgradeItem.CurrentLevel >= upgradeItem.Prices.Count)
            Price.text = "Full Upgrade";
        else
            Price.text = upgradeItem.Prices[upgradeItem.CurrentLevel].ToString() + " J$";
        GetComponent<TooltipItem>().description = upgradeItem.Description;
        // Level.text = upgradeItem.CurrentLevel.ToString();
        this.upgradeItemType = upgradeItemType;
        this.upgradeCallback = upgradeCallback;

        l1.sprite = upgradeItem.CurrentLevel >= 0 ? filled : empty;
        l2.sprite = upgradeItem.CurrentLevel >= 1 ? filled : empty;
        l3.sprite = upgradeItem.CurrentLevel >= 2 ? filled : empty;

    }
    public void OnClick()
    {
        upgradeCallback(upgradeItemType);
        UpgradeItem upgradeItem = itemManager.shop.UpgradeItems[upgradeItemType];
        ImageObject.GetComponent<Image>().sprite = upgradeItem.Image[upgradeItem.CurrentLevel];
    }
}
