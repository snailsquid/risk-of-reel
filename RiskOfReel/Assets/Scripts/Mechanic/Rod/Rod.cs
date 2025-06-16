using System;
using Mechanic.Items;
using UnityEngine;

namespace Mechanic.Rod
{
    
    public class Rod : MonoBehaviour
    {
        private RodState _currentState;
        [SerializeField] private RodData data;
        [SerializeField] private FishingLine attachedFishingLine;
        [SerializeField] private Bait.Bait attachedBait;
        public Fish.Fish AttachedFish { get; private set; }

        public static event Action<Fish.Fish> OnFishCaught;

        private void Update()
        {
            _currentState?.UpdateState();
        }
        
        public void ChangeState(RodState newState)
        {
            _currentState = newState;
            newState.Enter();
        }
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