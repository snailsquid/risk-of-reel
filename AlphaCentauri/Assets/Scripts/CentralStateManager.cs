using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CentralStateManager : MonoBehaviour
{
    [SerializeField] Transform hideButton, weightText, postRunPopupObject, quickSwitchContainer, mainMenuCanvas, eventLogObject;
    PostRunPopup postRunPopup;
    TimeManager timeManager;
    RodManager rodManager;
    ItemManager itemManager;
    CameraManager cameraManager;
    EventLog eventLog;
    [SerializeField] LinePointAttacher linePointAttacher;
    [SerializeField] FishingProgress fishingProgress;
    [SerializeField] Guard guard;
    public enum PlayerState
    {
        StartMenu,
        Rod,
        Shop,
    }
    public PlayerState playerState { get; private set; }
    void Awake()
    {
        postRunPopup = postRunPopupObject.GetComponent<PostRunPopup>();
        cameraManager = GetComponent<CameraManager>();
        timeManager = GetComponent<TimeManager>();
        rodManager = GetComponent<RodManager>();
        itemManager = GetComponent<ItemManager>();
        eventLog = eventLogObject.GetComponent<EventLog>();
    }
    void Start()
    {
        SetState(PlayerState.StartMenu);
    }

    public void SetState(PlayerState state)
    {
        if (state == PlayerState.Rod)
        {
            guard.canCatch = true;
            timeManager.StartTime();
            eventLog.Log("Click to Start", 2);
            linePointAttacher.Equip(itemManager.shop.UpgradeItems[ItemRegistry.UpgradeItemType.Rod].CurrentLevel);
            guard.SetMove(true);
        }
        timeManager.UI(state == PlayerState.Rod);
        itemManager.UI(state == PlayerState.Shop);
        weightText.GetComponent<FadeAnim>().SetUI(state == PlayerState.Rod);
        hideButton.GetComponent<FadeAnim>().SetUI(state == PlayerState.Rod);
        quickSwitchContainer.GetComponent<FadeAnim>().SetUI(state == PlayerState.Rod);
        mainMenuCanvas.GetComponent<FadeAnim>().SetUI(state == PlayerState.StartMenu);
        playerState = state;
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
        quickSwitchContainer.GetComponent<QuickSwitch>().ResetUI();
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
