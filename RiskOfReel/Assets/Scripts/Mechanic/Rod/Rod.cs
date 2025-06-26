using System;
using Mechanic.Items;
using Mechanic.Rod.States;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanic.Rod
{
    
    public class Rod : MonoBehaviour
    {
        public RodCastState CastState;
        public RodReelState ReelState;
        public RodWaitState WaitState;
        private IRodState _currentState;
        [SerializeField] private RodData data;
        public FishingLine attachedFishingLine;
        [SerializeField] private Bait.Bait attachedBait;
        public Bobber.Bobber attachedBobber;
        private Player.Player _player;
        [SerializeField] private Bobber.Bobber initialBobber;
        public Fish.Fish AttachedFish { get; private set; }

        public static event Action<Fish.Fish> OnFishCaught;

        public void Awake()
        {
            CastState = new RodCastState(this);
            ReelState = new RodReelState();
            WaitState = new RodWaitState();
        }

        public void Start()
        {
            _player = transform.parent.GetComponent<Player.Player>(); 
            ChangeState(CastState);
            ChangeBobber(initialBobber);
        }

        private void Update()
        {
            _currentState?.UpdateState();
        }
        
        public void ChangeState(IRodState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            newState.Enter();
        }

        public void ChangeBobber(Bobber.Bobber newBobber)
        {
            attachedBobber = newBobber;
            attachedBobber.Initialize(this);
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

        public Player.Player GetPlayer()
        {
            return _player;
        }
    }
}