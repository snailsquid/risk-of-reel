using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
    public class CentralStateManager : MonoBehaviour
    {
        #region Singleton
        public static CentralStateManager Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
            AssignVariables();
        }
        #endregion

        [SerializeField] private PostRunPopup postRunPopup;
        TimeManager timeManager;
        RodManager rodManager;
        ItemManager itemManager;
        CameraManager cameraManager;
        EventLog eventLog;
        UIManager uiManager = UIManager.Instance;
        [SerializeField] LinePointAttacher linePointAttacher;
        [SerializeField] FishingProgress fishingProgress;
        [SerializeField] Guard guard;
        public enum GameState
        {
            None,
            StartMenu,
            Fishing,
            Shop,
        }

        public GameState CurrentGameState { get; private set; }

        private void AssignVariables()
        {
            cameraManager = GetComponent<CameraManager>();
            timeManager = GetComponent<TimeManager>();
            rodManager = GetComponent<RodManager>();
            itemManager = GetComponent<ItemManager>();
            eventLog = EventLog.Instance;
            uiManager = UIManager.Instance;
        }
        void Start()
        {
            SetState(GameState.StartMenu);
        }

        public void SetState(GameState state)
        {
            CurrentGameState = state;
            Debug.Log("setting ui to " + state);
            uiManager.UpdateUI();
            if (state == GameState.Fishing)
            {
                guard.canCatch = true;
                timeManager.StartTime();
                eventLog.Log("Click to Start", 2);
                linePointAttacher.Equip(itemManager.shop.UpgradeItems[ItemRegistry.UpgradeItemType.Rod].CurrentLevel);
                guard.SetMove(true);
            }
            timeManager.UI(state == GameState.Fishing);
            itemManager.UI(state == GameState.Shop);
        }
        public void FinishRun(bool canContinue) // can be continued
        {
            Debug.Log(rodManager.equippedBucket.Fishes.Count);
            timeManager.PauseTime();
            linePointAttacher.Unequip();
            postRunPopup.Show(canContinue);
            postRunPopup.SetFishes(BucketToList(rodManager.equippedBucket));
            int sum = rodManager.equippedBucket.EndRun();
            itemManager.shop.AddBalance(sum);
            postRunPopup.SetBalance(sum);
            if (rodManager.equippedRod.RodMechanics.cast.bobberClone != null)
            {
                rodManager.equippedRod.RodMechanics.cast.bobberClone.GetComponent<Bobber>().Finish();
            }
            rodManager.equippedRod.CanFish = false;
            rodManager.equippedRod.Restart();
            fishingProgress.successCounter = 0;
            fishingProgress.success.GetComponent<FishRainbow>().value = 0;
            Debug.Log(rodManager.equippedBucket.Fishes.Count);
        }
        public void StartGame()
        {
            Debug.Log("starting game");
            // quickSwitchContainer.GetComponent<QuickSwitch>().ResetUI();
            cameraManager.SwitchToFishing(5);
        }
        List<Fish> BucketToList(Bucket bucket)
        {
            List<Fish> fishes = new List<Fish>();
            foreach (KeyValuePair<Fish, int> pair in bucket.Fishes)
            {
                fishes.AddRange(Enumerable.Repeat(pair.Key, pair.Value));
            }
            foreach (Fish fish in fishes)
            {
                Debug.Log(fish.Name);
            }
            return fishes;
        }
        public void ContinueRun()
        {
            rodManager.equippedRod.Restart();
            guard.canCatch = true;
            linePointAttacher.Equip(itemManager.shop.UpgradeItems[ItemRegistry.UpgradeItemType.Rod].CurrentLevel);
            cameraManager.SwitchToFishing();
            timeManager.StartTime();
            guard.SetMove(true);
        }
        public void EndRun() // actual end of run
        {
            //Animation first here
            Debug.Log(rodManager.equippedBucket.Fishes.Count);
            timeManager.RestartTime();
            itemManager.UpdateBalanceUI();
            cameraManager.SwitchToShop();
            rodManager.equippedRod.RodState = RodRegistry.RodState.PreCast;
        }
    }
}
