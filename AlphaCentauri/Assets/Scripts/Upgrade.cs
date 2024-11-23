using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] TMP_Text Name, Price;
    [SerializeField] Transform Level;
    [SerializeField] Transform ImageObject;
    [SerializeField] Sprite upgrade1, upgrade2, upgrade3;
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
        Level.GetComponent<Image>().sprite = ItemRegistry.UpgradeItems[upgradeItemType].CurrentLevel switch
        {
            0 => upgrade1,
            1 => upgrade2,
            2 => upgrade3,
            _ => null
        };
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
