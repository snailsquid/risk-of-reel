using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static RodRegistry;

public class RodManager : MonoBehaviour
{
    public Rod equippedRod { get; private set; }
    public bool clickDebounce;
    [SerializeField] (int MinTime, int MaxTime) FishBite = (5, 10);
    [SerializeField] float MaxFishBiteTime = 20f;
    [SerializeField] private Transform referenceObject, waterObject;
    [SerializeField] Transform horizontalBar, verticalBar, fishableArea, target, bobberObject, hookBar, successBar, popUp;
    [SerializeField] float bobberVelocity = 5f;
    TimeManager timeManager;
    CentralStateManager centralStateManager;
    void Awake()
    {
        centralStateManager = GetComponent<CentralStateManager>();
        timeManager = transform.GetComponent<TimeManager>();
        SetRod(RodType.FishingRod1);
        equippedRod.EquipBait(BaitRegistry.Baits[BaitRegistry.BaitType.None]);
    }
    void Start()
    {
    }
    void EquipBait(InventoryItem baitItem)
    {

    }
    void SetRod(RodType rodType)
    {
        equippedRod = Rods[rodType];
        Cast.Props castProps = new Cast.Props(horizontalBar, verticalBar, fishableArea, target, bobberObject, referenceObject, waterObject, bobberVelocity);
        Battle.Props battleProps = new Battle.Props(hookBar, successBar, MaxFishBiteTime, popUp);
        FishWait.Props fishWaitProps = new FishWait.Props(FishBite);
        equippedRod.SetRodMechanic(new RodMechanics.Props(castProps, battleProps, fishWaitProps), timeManager);
    }
    void Update()
    {
        if (centralStateManager.playerState == CentralStateManager.PlayerState.Rod)
        {

            if (Input.GetMouseButton(0))
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
        if (centralStateManager.playerState == CentralStateManager.PlayerState.Rod)
        {

            equippedRod.OnClick();
        }
    }
}
