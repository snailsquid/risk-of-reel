using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] TMP_Text Name, Price, Level;
    [SerializeField] Transform ImageObject;
    Action<ItemRegistry.UpgradeItemType> upgradeCallback;
    ItemRegistry.UpgradeItemType upgradeItemType;
    public void SetUI(ItemRegistry.UpgradeItemType upgradeItemType, Action<ItemRegistry.UpgradeItemType> upgradeCallback)
    {
        UpgradeItem upgradeItem = ItemRegistry.UpgradeItems[upgradeItemType];
        Name.text = upgradeItem.Name;
        ImageObject.GetComponent<Image>().sprite = upgradeItem.Image;
        if (upgradeItem.CurrentLevel >= upgradeItem.Prices.Count)
            Price.text = "Max Level";
        else
            Price.text = upgradeItem.Prices[upgradeItem.CurrentLevel].ToString();
        GetComponent<TooltipItem>().description = upgradeItem.Description;
        // Level.text = upgradeItem.CurrentLevel.ToString();
        this.upgradeItemType = upgradeItemType;
        this.upgradeCallback = upgradeCallback;
    }
    public void OnClick()
    {
        upgradeCallback(upgradeItemType);
    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
}
