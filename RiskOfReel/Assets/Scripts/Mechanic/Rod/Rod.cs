using System;
using Mechanic.Items;
using Mechanic.Rod.States;
using UnityEngine;

namespace Mechanic.Rod
{
    
    public class Rod : MonoBehaviour
    {
        private IRodState _currentState;
        public RodCastState CastState;
        public RodReelState ReelState = new RodReelState();
        [SerializeField] private RodData data;
        [SerializeField] private FishingLine attachedFishingLine;
        [SerializeField] private Bait.Bait attachedBait;
        public Fish.Fish AttachedFish { get; private set; }

        public static event Action<Fish.Fish> OnFishCaught;

        public void Awake()
        {
            CastState = new RodCastState(this);
            ReelState = new RodReelState();
        }

        public void Start()
        {
            ChangeState(CastState);
        }

        private void Update()
        {
            _currentState?.UpdateState();
        }
        
        public void ChangeState(IRodState newState)
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