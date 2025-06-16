using System;
using Mechanic.Items;
using UnityEngine;

namespace Mechanic.Rod
{
    
    public class Rod : MonoBehaviour
    {
        private IRodState _currentState;
        [SerializeField] private RodData data;
        [SerializeField] private FishingLine attachedFishingLine;
        [SerializeField] private Bait.Bait attachedBait;
        public Fish.Fish AttachedFish { get; private set; }

        public static event Action<Fish.Fish> OnFishCaught;
        
        public void CatchFish()
        {
            OnFishCaught?.Invoke(AttachedFish);
            LetGoFish();
            DestroyBait();
        }

        public void LetGoFish()
        {
            AttachedFish = null;
        }

        public void DestroyBait()
        {
            attachedBait = null;
        }
    }
}