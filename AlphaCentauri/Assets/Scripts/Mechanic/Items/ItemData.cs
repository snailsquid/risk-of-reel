using UnityEngine;

namespace Mechanic.Items
{
    public abstract class ItemData : ScriptableObject
    {
        public string itemName;
        public string description;
        public int price;
        public Sprite sprite;
        public Rarity rarity;
    }
}