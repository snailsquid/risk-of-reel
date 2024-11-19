
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static ItemRegistry;
public class ItemManager : MonoBehaviour
{
  [SerializeField] string currencyPrefix;
  [SerializeField] Transform upgradeContent, upgradeItem, buyItem, buyContent, sidePanel;
  [SerializeField] TMP_Text balanceText;
  Shop shop = new(BuyItems, UpgradeItems);
  Inventory inventory = new(new Dictionary<BuyItemType, InventoryItem>());
  void Start()
  {
    UpdateUI();
  }


  public void BuyItem(BuyItemType buyItemType)
  {
    if (shop.BuyItem(buyItemType))
    {
      inventory.AddItem(buyItemType);
      UpdateBuyUI();
    }
    else
    {
      Debug.Log("Not enough balance");
    };
  }
  void UpdateUI()
  {
    UpdateBuyUI();
    UpdateUpgradeUI();
  }
  void UpgradeItem(UpgradeItemType upgradeItemType)
  {
    if (shop.UpgradeItem(upgradeItemType))
    {
      UpdateUpgradeUI();
    }
    else
    {
      Debug.Log("Not enough balance");
    };
  }
  void UpdateUpgradeUI()
  {
    foreach (Transform child in upgradeContent)
    {
      print("destroy");
      Destroy(child.gameObject);
    }
    foreach (UpgradeItemType key in shop.UpgradeItems.Keys)
    {
      // Update UI
      Transform clone = Instantiate(upgradeItem, upgradeContent);
      clone.GetComponent<Upgrade>().SetUI(key, UpgradeItem);
    }
    UpdateBalanceUI();
  }
  void UpdateBuyUI()
  {
    foreach (Transform child in buyContent)
    {
      Destroy(child.gameObject);
    }
    foreach (BuyItemType key in shop.BuyItems.Keys)
    {
      // Update UI
      Transform clone = Instantiate(buyItem, buyContent);
      clone.GetComponent<Buy>().SetUI(key, UpdateSidepanelUI);
    }
    UpdateBalanceUI();
  }
  void UpdateSidepanelUI(BuyItemType buyItemType)
  {
    sidePanel.GetComponent<Sidepanel>().SetUI(buyItemType, BuyItem);
  }
  void UpdateBalanceUI()
  {
    balanceText.text = currencyPrefix + shop.Balance.ToString();
  }
}

public static class ItemRegistry
{
  public enum BuyItemType
  {
    None,
    Pellet,
    CacingTanah,
    Jangkrik,
    DagingCincang,
    BeefWellington,
    Mackarel,
    Crab,

  }
  public enum UpgradeItemType
  {
    Upgrade
  }
  public static Dictionary<BuyItemType, BuyItem> BuyItems = new Dictionary<BuyItemType, BuyItem>(){
    {BuyItemType.None,new BuyItem("None", 1000, "None Description", null, BaitRegistry.Baits[BaitRegistry.BaitType.None])},
  };
  public static Dictionary<UpgradeItemType, UpgradeItem> UpgradeItems = new Dictionary<UpgradeItemType, UpgradeItem>(){
    {UpgradeItemType.Upgrade,new UpgradeItem("Upgrade", 1000, "Upgrade", null)},
  };

}