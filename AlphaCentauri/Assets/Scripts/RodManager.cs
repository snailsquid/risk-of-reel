using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static RodRegistry;

public class RodManager : MonoBehaviour
{
    public Rod equippedRod;
    public bool clickDebounce;
    [SerializeField] private Transform referenceObject, waterObject;
    [SerializeField] Transform horizontalBar, verticalBar, fishableArea, target, bobberObject;
    [SerializeField] float bobberVelocity = 5f;
    void Start()
    {
        SetRod(RodType.FishingRod1);
    }
    void SetRod(RodType rodType)
    {
        equippedRod = Rods[rodType];
        Cast.Props castProps = new Cast.Props(horizontalBar, verticalBar, fishableArea, target, bobberObject, referenceObject, waterObject, bobberVelocity);
        equippedRod.SetRodMechanic(new RodMechanics.Props(castProps));
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
