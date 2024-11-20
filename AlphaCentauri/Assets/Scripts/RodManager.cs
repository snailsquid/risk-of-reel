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
    [SerializeField] private Transform referenceObject, waterObject;
    [SerializeField] Transform horizontalBar, verticalBar, fishableArea, target, bobberObject, hookBar;
    [SerializeField] float bobberVelocity = 5f;
    void Start()
    {
        SetRod(RodType.FishingRod1);
    }
    void SetRod(RodType rodType)
    {
        equippedRod = Rods[rodType];
        Cast.Props castProps = new Cast.Props(horizontalBar, verticalBar, fishableArea, target, bobberObject, referenceObject, waterObject, bobberVelocity);
        Battle.Props battleProps = new Battle.Props(hookBar);
        FishWait.Props fishWaitProps = new FishWait.Props(FishBite);
        equippedRod.SetRodMechanic(new RodMechanics.Props(castProps, battleProps, fishWaitProps));
    }
    void Update()
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
    void OnClick()
    {
        equippedRod.OnClick();
    }
}
