using System.Collections.Generic;
using UnityEngine;

namespace Mechanic.Player.Upgrades
{
    public class Upgrades : MonoBehaviour
    {
        [SerializeField] private List<UpgradeItem> upgrades = new List<UpgradeItem>();
    }
}