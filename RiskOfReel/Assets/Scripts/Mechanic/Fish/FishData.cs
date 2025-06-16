using UnityEngine;

namespace Mechanic.Fish
{
    public class FishData : ScriptableObject
    {
        public string fishName;
        public int power;
        public int weight;
        public int size;
        public int agility;

        public int GetSellValue()
        {
            return 0;
        }
    }
}