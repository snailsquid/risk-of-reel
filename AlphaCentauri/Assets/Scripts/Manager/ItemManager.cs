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
  [SerializeField] Transform upgradeContent, upgradeItem, buyItem, buyContent, sidePanel, shopInventory, lineupContainer, quickSwitch;
  [SerializeField] TMP_Text balanceText, logText;
  CentralStateManager centralStateManager;
  RodManager rodManager;
  public EventLog eventLog;
  public Shop shop { get; private set; }
  public Inventory inventory { get; private set; }
  public List<BuyItemType> BaitLineup { get; private set; }
  [SerializeField] List<BuyImage> buyImages = new List<BuyImage>();
  void Awake()
  {
    Debug.Log("im still awake");
    eventLog = logText.GetComponent<EventLog>();
    centralStateManager = GetComponent<CentralStateManager>();
    rodManager = GetComponent<RodManager>();
    BaitLineup = new List<BuyItemType>{
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
    inventory = new(new Dictionary<BuyItemType, InventoryItem>(), quickSwitch.GetComponent<QuickSwitch>());

  }
  void Start()
  {
    UpdateUI();
  }

  public void UI(bool show = true)
  {
    shopInventory.GetComponent<FadeAnim>().SetUI(show);
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
    }
    ;
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
    balanceText.text = shop.Balance.ToString();
  }
  public void DeductBalance(int amount)
  {
    shop.DeductBalance(amount);
  }
  public string GetBalance()
  {
    return shop.Balance.ToString();

  }
  void UpdateLineupUI()
  {
    for (int i = 0; i < BaitLineup.Count; i++)
    {
      lineupContainer.GetChild(0).GetChild(i).GetComponent<LineupButton>().SetButton(BaitLineup[i]);
    }
  }
  public void AddToLineup(BuyItemType buyItemType)
  {
    if (BaitLineup.Contains(buyItemType)) return;
    for (int i = 0; i < BaitLineup.Count; i++)
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
  }
  public enum UpgradeItemType
  {
    Rod, Hook, Bucket
  }
  public static Dictionary<BaitRegistry.BaitType, BuyItemType> BaitToBuy = new Dictionary<BaitRegistry.BaitType, BuyItemType>(){
    {BaitRegistry.BaitType.None,BuyItemType.None},
    {BaitRegistry.BaitType.CacingTanah,BuyItemType.CacingTanah},
    {BaitRegistry.BaitType.Mackarel,BuyItemType.Mackarel},
  };
  public static Dictionary<BuyItemType, BuyItem> BuyItems = new Dictionary<BuyItemType, BuyItem>(){
    {BuyItemType.None,new BuyItem("None", 0, "Last resort as to when you absolutely have no bait left", null, BaitRegistry.Baits[BaitRegistry.BaitType.None])},
    {BuyItemType.Pellet,new BuyItem("Pellet", 200, "Common fish bait, a classic that is always reliable", null, BaitRegistry.Baits[BaitRegistry.BaitType.Pellet])},
    {BuyItemType.CacingTanah,new BuyItem("Cacing Tanah", 500, "The smell and wiggle of cacing tanah attracts common fish", null, BaitRegistry.Baits[BaitRegistry.BaitType.CacingTanah])},
    {BuyItemType.Jangkrik,new BuyItem("Jangkrik", 1000, "Farm-raised jangkrik. Born to reel", null, BaitRegistry.Baits[BaitRegistry.BaitType.Jangkrik])},
    {BuyItemType.DagingCincang,new BuyItem("Daging Cincang", 2000, "Leftover meat from kantin GKU 2 minced to bits", null, BaitRegistry.Baits[BaitRegistry.BaitType.DagingCincang])},
    {BuyItemType.BeefWellington,new BuyItem("Beef Wellington", 100000, "The ultimate bait. No fish could pass such perfection. They say if you're lucky enough, you can attract the ultimate fish", null, BaitRegistry.Baits[BaitRegistry.BaitType.BeefWellington])},
    {BuyItemType.Mackarel,new BuyItem("Mackarel", 50000, "I invented this one, this one hell of a bait is the culmination of Mackerels squished together using super glue", null, BaitRegistry.Baits[BaitRegistry.BaitType.Mackarel])},
  };
  public static Dictionary<UpgradeItemType, UpgradeItem> UpgradeItems = new Dictionary<UpgradeItemType, UpgradeItem>(){
    {UpgradeItemType.Rod,new UpgradeItem("Rod", new List<int>{100000,2000000},new List<float>{1, 1.2f, 1.4f} ,"[Super] +20% bar width\n[Ultimate] +40% bar width ", new List<Sprite>{Resources.Load<Sprite>("Images/Rod1"),Resources.Load<Sprite>("Images/Rod2"),Resources.Load<Sprite>("Images/Rod3")})},
    {UpgradeItemType.Hook,new UpgradeItem("Hook", new List<int>{100000, 2000000},new List<float>{10, 15, 17}, "[Basic] 5 seconds to fish\n[Super] 10 seconds to fish\n[Ultimate] 15 seconds to fish", new List<Sprite>{Resources.Load<Sprite>("Images/Hook1"),Resources.Load<Sprite>("Images/Hook2"),Resources.Load<Sprite>("Images/Hook3")})},
    {UpgradeItemType.Bucket,new UpgradeItem("Bucket", new List<int>{100000, 2000000},new List<float>{50, 280f, 5000f}, "[Basic] Maximum 50kg weight\n[Super] Maximum 280kg weight\n[Ultimate] Maximum 5000kg weight", new List<Sprite>{Resources.Load<Sprite>("Images/Bucket1"),Resources.Load<Sprite>("Images/Bucket2"),Resources.Load<Sprite>("Images/Bucket3")})},
  };

}
[Serializable]
public class BuyImage
{
  public BuyItemType buyItem;
  public Sprite image;
}