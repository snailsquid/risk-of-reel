using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ItemRegistry;
public class ItemManager : MonoBehaviour
{
  [SerializeField] string currencyPrefix;
  [SerializeField] Transform upgradeContent, upgradeItem, buyItem, buyContent, sidePanel, shopInventory, lineupContainer;
  [SerializeField] TMP_Text balanceText;
  CentralStateManager centralStateManager;
  RodManager rodManager;
  public Shop shop { get; private set; }
  Inventory inventory = new(new Dictionary<BuyItemType, InventoryItem>());
  public BuyItemType[] BaitLineup { get; private set; }
  [SerializeField] List<BuyImage> buyImages = new List<BuyImage>();
  void Awake()
  {
    Debug.Log("awake");
    centralStateManager = GetComponent<CentralStateManager>();
    rodManager = GetComponent<RodManager>();
    BaitLineup = new BuyItemType[]{
      BuyItemType.None,
      BuyItemType.None,
      BuyItemType.None,
    };
    Dictionary<BuyItemType, BuyItem> BuyItemsClone = BuyItems;
    foreach (KeyValuePair<BuyItemType, BuyItem> pair in BuyItemsClone)
    {
      BuyImage buyImage = buyImages.FirstOrDefault(item => item.buyItem == pair.Key);
      if (buyImage != null)
      {
        BuyItemsClone[pair.Key].SetImage(buyImage.image);
      }
    }
    shop = new(BuyItemsClone, UpgradeItems);
    Debug.Log("shop init");
    UpdateUI();
  }
  void Start()
  {
  }

  public void UI(bool show = true)
  {
    shopInventory.gameObject.SetActive(show);
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
  public void UpdateUI()
  {
    UpdateBuyUI();
    UpdateUpgradeUI();
    UpdateLineupUI();
  }
  void UpgradeItem(UpgradeItemType upgradeItemType)
  {

    if (shop.UpgradeItem(upgradeItemType))
    {
      if (upgradeItemType == UpgradeItemType.Bucket)
      {
        int level = shop.UpgradeItems[UpgradeItemType.Bucket].CurrentLevel;
        rodManager.EquipBucket(level);
      }
      else if (upgradeItemType == UpgradeItemType.Hook)
      {
        int level = shop.UpgradeItems[UpgradeItemType.Hook].CurrentLevel;
        rodManager.equippedRod.RodMechanics.battle.props.SetmaxFishBiteTime(shop.UpgradeItems[UpgradeItemType.Hook].Values[level]);
      }

      UpdateUpgradeUI();
    }
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
      Debug.Log(key);
      if (key != BuyItemType.None)
      {
        // Update UI
        Transform clone = Instantiate(buyItem, buyContent);
        int quantity = inventory.Items.ContainsKey(key) ? inventory.Items[key].Quantity : 0;
        clone.GetComponent<Buy>().SetUI(key, UpdateSidepanelUI, quantity);
      }
    }
    UpdateBalanceUI();
  }
  void UpdateSidepanelUI(BuyItemType buyItemType)
  {
    sidePanel.GetComponent<Sidepanel>().SetUI(buyItemType, BuyItem);
  }
  public void UpdateBalanceUI()
  {
    Debug.Log("updating balance ui");
    balanceText.text = currencyPrefix + shop.Balance.ToString();
  }
  public void DeductBalance(int amount)
  {
    shop.DeductBalance(amount);
  }
  public string GetBalance()
  {
    return currencyPrefix + shop.Balance.ToString();

  }
  void UpdateLineupUI()
  {
    for (int i = 0; i < BaitLineup.Length; i++)
    {
      Debug.Log(BaitLineup[i]);
      lineupContainer.GetChild(i).GetComponent<LineupButton>().SetButton(BaitLineup[i]);
    }
  }
  public void AddToLineup(BuyItemType buyItemType)
  {
    if (BaitLineup.Contains(buyItemType)) return;
    for (int i = 0; i < BaitLineup.Length; i++)
    {
      if (BaitLineup[i] == BuyItemType.None)
      {
        BaitLineup[i] = buyItemType;
        break;
      }
    }
    UpdateLineupUI();
  }
  public void RemoveFromLineup(int index)
  {
    BaitLineup[index] = BuyItemType.None;
    UpdateLineupUI();
  }
  public void UseLineup(int index)
  {
    inventory.RemoveItem(BaitLineup[index]);
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
    Rod, Hook, Bucket
  }
  public static Dictionary<BuyItemType, BuyItem> BuyItems = new Dictionary<BuyItemType, BuyItem>(){
    {BuyItemType.None,new BuyItem("None", 1000, "None Description", null, BaitRegistry.Baits[BaitRegistry.BaitType.None])},
    {BuyItemType.CacingTanah,new BuyItem("Cacing Tanah", 1000, "Cacing Description", null, BaitRegistry.Baits[BaitRegistry.BaitType.CacingTanah])},
    {BuyItemType.Mackarel,new BuyItem("Macakererelle", 1000, "THIS MACAKRENRKEEKRE Description", null, BaitRegistry.Baits[BaitRegistry.BaitType.Mackarel])},
  };
  public static Dictionary<UpgradeItemType, UpgradeItem> UpgradeItems = new Dictionary<UpgradeItemType, UpgradeItem>(){
    {UpgradeItemType.Rod,new UpgradeItem("Rod", new List<int>{100000,2000000},new List<float>{1, 1.2f, 1.4f} ,"[Super] +20% bar width\n[Ultimate] +40% bar width ", Resources.Load("Images/Rod")as Sprite)},
    {UpgradeItemType.Hook,new UpgradeItem("Hook", new List<int>{100000, 2000000},new List<float>{5, 10, 15}, "[Super] +20% bar width\n[Ultimate] +40% bar width ", Resources.Load("Images/Rod")as Sprite)},
    {UpgradeItemType.Bucket,new UpgradeItem("Bucket", new List<int>{100000, 2000000},new List<float>{50, 280f, 5000f}, "[Super] +20% bar width\n[Ultimate] +40% bar width ", Resources.Load("Images/Rod")as Sprite)},
  };

}
[Serializable]
public class BuyImage
{
  public BuyItemType buyItem;
  public Sprite image;
}