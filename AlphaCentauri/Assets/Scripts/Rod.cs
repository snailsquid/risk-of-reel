using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static RodRegistry;

public class Rod
{
    public RodState RodState { get; set; } = RodState.PreCast;
    public string Name
    { get; private set; }
    public RodRarity RodRarity { get; private set; }
    public Bait Bait { get; set; }
    public RodMechanics RodMechanics { get; private set; }
    public Rod(string name, RodRarity rodRarity)
    {
        Name = name;
        RodRarity = rodRarity;
    }
    public void SetRodMechanic(RodMechanics.Props props)
    {
        RodMechanics = new RodMechanics(props);
    }
    public void Update()
    {
        switch (RodState)
        {
            case RodState.PreCast:
                break;
            case RodState.Casting:
                RodMechanics.CastUpdate();
                break;
            case RodState.FishWaiting:
                break;
            case RodState.Battling:
                break;
            case RodState.PostFish:
                break;
        }
    }
    public void EquipBait(Bait bait)
    {
        Bait = bait;
    }
    public void Cast()
    {
        RodState = RodState.Casting;
        RodMechanics.Cast();
    }
    public void Battle()
    {
        RodState = RodState.Battling;

    }
    public void FishWait()
    {
        RodState = RodState.FishWaiting;
    }
    public void FinishFishing()
    {
        RodState = RodState.PostFish;
    }
}

public class RodMechanics
{
    Cast cast;
    public void Cast()
    {
        cast.CastClick();
    }
    public void CastUpdate()
    {
        cast.CastUpdate();
    }
    public void FishWait()
    {

    }
    public void Battle()
    {

    }
    public RodMechanics(Props props)
    {
        cast = new Cast(props.castProps);
    }
    public class Props
    {
        public Cast.Props castProps;
        public Props(Cast.Props castProps)
        {
            this.castProps = castProps;
        }
    }
}
public class Cast
{
    public Props CastProperties { get; private set; }
    enum CastState
    {
        Horizontal,
        Vertical,
        None,
    }
    CastState castState = CastState.None;
    public void CastUpdate()
    {
        switch (castState)
        {
            case CastState.None:
                break;
            case CastState.Horizontal:
                PlayHorizontal();
                break;
            case CastState.Vertical:
                PlayVertical();
                break;
        }
    }
    public Cast(Props castProps)
    {
        CastProperties = castProps;
    }
    public void CastClick()
    {
        switch (castState)
        {
            case CastState.None:
                castState = CastState.Horizontal;
                break;
            case CastState.Horizontal:
                castState = CastState.Vertical;
                break;
            case CastState.Vertical:
                castState = CastState.None;
                break;
        }
    }
    void PlayVertical()
    {

    }
    float horizontalPercent = 0f;
    float verticalPercent = 0f;
    float amplitude;
    void PlayHorizontal()
    {
        castState = CastState.Horizontal;
        horizontalPercent = Mathf.PingPong(Time.time, 1f);
        CastProperties.horizontalBar.localPosition = new Vector3((horizontalPercent - 0.5f) * amplitude, 0f, 0f);
    }
    public class Props
    {
        public Transform horizontalBar, verticalBar, fishableArea, target, bobberObject, referenceObject;
        public Props(Transform horizontalBar, Transform verticalBar, Transform fishableArea, Transform target, Transform bobberObject, Transform referenceObject)
        {
            this.horizontalBar = horizontalBar;
            this.verticalBar = verticalBar;
            this.fishableArea = fishableArea;
            this.target = target;
            this.bobberObject = bobberObject;
            this.referenceObject = referenceObject;
        }
    }
}

public static class RodRegistry
{
    public enum RodState
    {
        PreCast,
        Casting,
        FishWaiting,
        Battling,
        PostFish
    }
    public enum RodRarity
    {
        Basic,
        Super,
        Ultimate
    }
    public enum RodType
    {
        FishingRod1,
        FishingRod2
    }
    public static Dictionary<RodType, Rod> Rods = new Dictionary<RodType, Rod>(){
        {RodType.FishingRod1, new Rod("Fishing Rod 1", RodRarity.Basic)},
        {RodType.FishingRod2, new Rod("Fishing Rod 2", RodRarity.Super)}
    };
}