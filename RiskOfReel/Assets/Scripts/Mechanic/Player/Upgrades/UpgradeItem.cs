using System;
using UnityEngine;

namespace Mechanic.Player.Upgrades
{
    [Serializable]
    public class UpgradeItem
    {
        [SerializeField] private UpgradeItemData upgradeItemData;
        [SerializeField] private int currentLevel;
    }
}