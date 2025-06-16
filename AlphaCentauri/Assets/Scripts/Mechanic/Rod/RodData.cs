using Mechanic.Items;
using UnityEngine;

namespace Mechanic.Rod
{
    [CreateAssetMenu(fileName = "RodData", menuName = "RiskOfReel/RodData")]
    public class RodData : ItemData
    {
        public int luck;
        public int rigidity;
        public int flex;
    }
}