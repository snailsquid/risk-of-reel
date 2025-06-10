using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static RodRegistry;

public class RodManager : MonoBehaviour
{
    public Rod equippedRod { get; private set; }
    public Bucket equippedBucket { get; private set; }
    public bool clickDebounce;
    [SerializeField] (int MinTime, int MaxTime) FishBite = (5, 10);
    [SerializeField] private Transform referenceObject, waterObject;
    [SerializeField] Transform horizontalBar, verticalBar, fishableArea, target, bobberObject, hookBar, successBar, popUp, postRunPopup, eventLogObect;
    [SerializeField] float bobberVelocity = 5f;
    [SerializeField] LinePointAttacher linePointAttacher;
    TimeManager timeManager;
    CentralStateManager centralStateManager;
    ItemManager itemManager;
    EventLog eventLog;
    void Awake()
    {
        equippedRod = new("Rod", RodRarity.Basic);
        eventLog = eventLogObect.GetComponent<EventLog>();
        itemManager = GetComponent<ItemManager>();
        centralStateManager = GetComponent<CentralStateManager>();
        timeManager = transform.GetComponent<TimeManager>();
        equippedRod.EquipBait(BaitRegistry.BaitType.None);
        EquipBucket(0);
    }

    void Start()
    {

        SetRod(RodRarity.Basic);
    }
    public void EquipBucket(int level)
    {
        equippedBucket = new(ItemRegistry.UpgradeItems[ItemRegistry.UpgradeItemType.Bucket].Values[level]);
        equippedRod.SetBucket(equippedBucket);
    }
    void EquipBait(InventoryItem baitItem)
    {

    }
    void SetRod(RodRarity rodRarity)
    {
        equippedRod.SetRodRarity(rodRarity);
        float maxFishBiteTime = ItemRegistry.UpgradeItems[ItemRegistry.UpgradeItemType.Hook].Values[itemManager.shop.UpgradeItems[ItemRegistry.UpgradeItemType.Hook].CurrentLevel];
        Cast.Props castProps = new Cast.Props(horizontalBar, verticalBar, fishableArea, target, bobberObject, referenceObject, waterObject, bobberVelocity, itemManager, linePointAttacher);
        Battle.Props battleProps = new Battle.Props(hookBar, successBar, maxFishBiteTime, popUp, eventLog, linePointAttacher);
        FishWait.Props fishWaitProps = new FishWait.Props(FishBite);
        PostFish.Props postFishProps = new PostFish.Props(centralStateManager, postRunPopup.GetComponent<PostRunPopup>(), equippedBucket);
        equippedRod.SetRodMechanic(new RodMechanics.Props(castProps, battleProps, fishWaitProps, postFishProps), timeManager, centralStateManager);
    }
    void Update()
    {
        if (centralStateManager.CurrentGameState == CentralStateManager.GameState.Fishing)
        {

            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (!clickDebounce)
                {
                    clickDebounce = true;
                    OnClick();
                }

            }

            else
            {
                if (clickDebounce)
                {
                    clickDebounce = false;
                }
            }
            equippedRod.Update();
        }
    }
    void OnClick()
    {
        if (centralStateManager.CurrentGameState == CentralStateManager.GameState.Fishing)
        {

            equippedRod.OnClick();
        }
    }
}
