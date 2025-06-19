using Mechanic.Items;
using UnityEngine;

namespace Mechanic.Rod
{
    [CreateAssetMenu(fileName = "FishingLine", menuName = "RiskOfReel/Fishing Line")]
    public class FishingLine : ItemData
    {
        public int length;
        public int durability;
        public int resistance;
    }
}