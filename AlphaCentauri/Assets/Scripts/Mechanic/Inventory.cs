using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static ItemRegistry;

public class Inventory
{
  public Dictionary<BuyItemType, InventoryItem> Items { get; private set; } = new Dictionary<BuyItemType, InventoryItem>();
  QuickSwitch quickSwitch;
  public Inventory(Dictionary<BuyItemType, InventoryItem> items, QuickSwitch quickSwitch)
  {
    Items = items;
    this.quickSwitch = quickSwitch;
  }
  public void AddItem(BuyItemType item)
  {
    if (Items.ContainsKey(item))
    {

      Items[item].AddQuantity(1);
    }
    else
    {
      Items.Add(item, new InventoryItem(ItemRegistry.BuyItems[item].Name, ItemRegistry.BuyItems[item].Price, ItemRegistry.BuyItems[item].Description, ItemRegistry.BuyItems[item].Image, 1, BuyItems[item].Bait));
    }
  }
  public void RemoveItem(BuyItemType item)
  {
    if (Items.ContainsKey(item))
    {
      Items[item].RemoveQuantity(1);
      if (Items[item].Quantity <= 0)
      {
        Items.Remove(item);
      }

    }
    else
    {
      // Debug.Log("Item not found");
    }
  }
}
public class InventoryItem : BuyItem
{
  public int Quantity { get; private set; }
  public InventoryItem(string name, int price, string description, Sprite image, int quantity, Bait bait) : base(name, price, description, image, bait)
  {
    Quantity = quantity;
  }
  public void AddQuantity(int amount)
  {
    Quantity += amount;
  }
  public void RemoveQuantity(int amount)
  {
    Quantity -= amount;
  }
}