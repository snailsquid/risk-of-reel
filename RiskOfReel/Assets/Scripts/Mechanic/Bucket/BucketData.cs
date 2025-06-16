using Mechanic.Items;
using UnityEngine;

namespace Mechanic.Bucket
{
    [CreateAssetMenu(fileName = "BucketData", menuName = "RiskOfReel/BucketData", order = 1)]
    public class BucketData : ItemData
    {
        public int weightCapacity;
        public int sizeCapacity;
    }
}