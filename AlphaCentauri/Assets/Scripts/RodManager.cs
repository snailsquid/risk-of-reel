using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static RodRegistry;

public class RodManager : MonoBehaviour
{
    public Rod equippedRod;
    [SerializeField] private Transform player, referenceObject;
    [SerializeField] Transform horizontalBar, verticalBar, fishableArea, target, bobberObject;
    void Start()
    {
        SetRod(RodType.FishingRod1);
        player = referenceObject.GetComponent<ReferenceScript>().player;
    }
    void SetRod(RodType rodType)
    {
        equippedRod = Rods[rodType];
        Cast.Props castProps = new Cast.Props(horizontalBar, verticalBar, fishableArea, target, bobberObject, referenceObject);
        equippedRod.SetRodMechanic(new RodMechanics.Props(castProps));
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnClick();
        }
        equippedRod.Update();
    }
    void OnClick()
    {
        switch (equippedRod.RodState)
        {
            case RodState.PreCast:
                equippedRod.Cast();
                break;
            case RodState.Casting:
                equippedRod.Battle();
                break;
            case RodState.Battling:
                equippedRod.Battle();
                break;
            case RodState.FishWaiting:
                equippedRod.FinishFishing();
                break;
        }
    }
}
