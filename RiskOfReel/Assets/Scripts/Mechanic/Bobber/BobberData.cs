using Mechanic.Items;
using UnityEngine;

namespace Mechanic.Bobber
{
    [CreateAssetMenu(fileName = "Bobber",menuName = "RiskOfReel/BobberData")]
    public class BobberData : ItemData
    {
        [Tooltip("How many seconds until the bobber lands")]
        public float bobberLandDuration;
    }
}