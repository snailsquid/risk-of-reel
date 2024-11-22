using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static ItemRegistry;

public class Shop
{
  public Dictionary<BuyItemType, BuyItem> BuyItems { get; private set; } = new();
  public Dictionary<UpgradeItemType, UpgradeItem> UpgradeItems { get; private set; } = new();
  public int Balance { get; private set; } = 1000000000;
  public Shop(Dictionary<BuyItemType, BuyItem> buyItems, Dictionary<UpgradeItemType, UpgradeItem> upgradeItems)
  {
    Debug.Log("initialize shop");
    BuyItems = buyItems;
    UpgradeItems = upgradeItems;
  }
  public void AddBalance(int amount)
  {
    Debug.Log("before " + Balance);
    Balance += amount;
    Debug.Log("after " + Balance);
  }
  public void DeductBalance(int amount)
  {
    Balance -= amount;
  }
  public bool BuyItem(BuyItemType buyItemType)
  {
    Debug.Log("Buying Item");
    Debug.Log(BuyItems[buyItemType]);
    BuyItem item = BuyItems[buyItemType];
    if (Balance >= item.Price)
    {
      DeductBalance(item.Price);
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
    if (item.CurrentLevel >= item.MaxLevel) return false;
    if (Balance < item.Prices[item.CurrentLevel]) return false;
    Balance -= item.Prices[item.CurrentLevel];
    item.Upgrade();
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
  public Sprite Image { get; private set; }
  public Bait Bait { get; }
  public BuyItem(string name, int price, string description, Sprite image, Bait bait)
  {
    Debug.Log(name);
    Name = name;
    Price = price;
    Description = description;
    Image = image;
    Bait = bait;
  }
  public void SetImage(Sprite image)
  {
    Image = image;
  }
}
public class UpgradeItem
{
  public string Name { get; private set; }
  public List<int> Prices { get; private set; }
  public List<float> Values { get; private set; }
  public string Description { get; private set; }
  public Sprite Image { get; private set; }
  public int CurrentLevel { get; private set; } = 0;
  public int MaxLevel { get; private set; }
  public UpgradeItem(string name, List<int> prices, List<float> values, string description, Sprite image)
  {
    Name = name;
    Description = description;
    Prices = prices;
    Values = values;
    Image = image;
    MaxLevel = prices.Count;
    Debug.Log(name);
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
