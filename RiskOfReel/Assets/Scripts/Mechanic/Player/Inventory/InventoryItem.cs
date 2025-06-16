using System;
using Mechanic.Items;
using UnityEngine;

namespace Mechanic.Player
{
    [Serializable]
    public class InventoryItem 
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private int quantity;
    }
}