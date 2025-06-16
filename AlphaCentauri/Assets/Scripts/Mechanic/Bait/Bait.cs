using UnityEngine;

namespace Mechanic.Bait
{
    public abstract class Bait : MonoBehaviour
    {
        public BaitData baitData;
        public abstract bool Condition();
        public abstract bool Activate();
    }
}