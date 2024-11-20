using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemRegistry;

public class Shop
{
  public Dictionary<BuyItemType, BuyItem> BuyItems { get; private set; } = new();
  public Dictionary<UpgradeItemType, UpgradeItem> UpgradeItems { get; private set; } = new();
  public int Balance { get; private set; } = 1000000000;
  public Shop(Dictionary<BuyItemType, BuyItem> buyItems, Dictionary<UpgradeItemType, UpgradeItem> upgradeItems)
  {
    BuyItems = buyItems;
    UpgradeItems = upgradeItems;
  }
  public void AddBalance(int amount)
  {
    Balance += amount;
  }
  public bool BuyItem(BuyItemType buyItemType)
  {
    BuyItem item = BuyItems[buyItemType];
    if (Balance >= item.Price)
    {
      Balance -= item.Price;
      return true;
    }
    else
    {
      return false;
    }
  }
  public bool UpgradeItem(UpgradeItemType upgradeItemType)
  {
    if (!UpgradeItems.ContainsKey(upgradeItemType)) return false;
    UpgradeItem item = UpgradeItems[upgradeItemType];
    if (Balance < item.Price) return false;
    Balance -= item.Price;
    return true;
  }
}
interface IItem
{
  string Name { get; }
  int Price { get; }
  string Description { get; }
  public Sprite Image { get; }
}
public class BuyItem : IItem
{
  public string Name { get; }
  public int Price { get; }
  public string Description { get; }
  public Sprite Image { get; }
  public Bait Bait { get; }
  public BuyItem(string name, int price, string description, Sprite image, Bait bait)
  {
    Name = name;
    Price = price;
    Description = description;
    Image = image;
    Bait = bait;
  }
}
public class UpgradeItem : IItem
{
  public string Name { get; private set; }
  public int Price { get; private set; }
  public string Description { get; private set; }
  public Sprite Image { get; private set; }
  public int CurrentLevel { get; private set; } = 0;
  public int MaxLevel { get; private set; } = 3;
  public UpgradeItem(string name, int price, string description, Sprite image)
  {
    Name = name;
    Price = price;
    Description = description;
    Image = image;
  }
  public UpgradeItem(string name, int price, string description, Sprite image, int maxLevel) : this(name, price, description, image)
  {
    MaxLevel = maxLevel;
  }
  public void Upgrade()
  {
    if (CurrentLevel >= MaxLevel)
    {
      return;
    }
    CurrentLevel++;
  }
}
