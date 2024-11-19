
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemRegistry;
public class ItemManager : MonoBehaviour
{
  public Shop shop = new(BuyItems, UpgradeItems);
  public Inventory inventory = new(new Dictionary<BuyItemType, InventoryItem>());

  public void BuyItem(BuyItemType item)
  {
    BuyItem buyItem = shop.BuyItems[item];
    if (shop.BuyItem(buyItem))
    {
      inventory.AddItem(item);
    }
    else
    {
      Debug.Log("Not enough balance");
    };
  }
}

public static class ItemRegistry
{
  public enum BuyItemType
  {
    Rokok
  }
  public enum UpgradeItemType
  {
    Upgrade
  }
  public static Dictionary<BuyItemType, BuyItem> BuyItems = new Dictionary<BuyItemType, BuyItem>(){
    {BuyItemType.Rokok,new BuyItem("Rokok", 1000, "Rokok", null)},
  };
  public static Dictionary<UpgradeItemType, UpgradeItem> UpgradeItems = new Dictionary<UpgradeItemType, UpgradeItem>(){
    {UpgradeItemType.Upgrade,new UpgradeItem("Upgrade", 1000, "Upgrade", null)},
  };

}