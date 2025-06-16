using System.Collections.Generic;
using Mechanic.Items;
using UnityEngine;

namespace Mechanic.Player.Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeItem", menuName = "RiskOfReel/UpgradeItem")]
    public class UpgradeItemData : ScriptableObject
    {
        public string itemName;
        public int maxLevel;
        public List<ItemData> items;
    }
}