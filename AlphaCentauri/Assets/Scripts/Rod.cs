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
                bool isFinished = RodMechanics.CastUpdate();
                if (isFinished) { RodState = RodState.FishWaiting; };
                break;
            case RodState.FishWaiting:
                break;
            case RodState.Battling:
                break;
            case RodState.PostFish:
                break;
        }
    }
    public void OnClick()
    {
        Debug.Log("click " + RodState);
        switch (RodState)
        {
            case RodState.PreCast:
                Cast();
                break;
            case RodState.Casting:
                Cast();
                break;
            case RodState.FishWaiting:
                Battle();
                break;
            case RodState.Battling:
                FinishFishing();
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
        Debug.Log("Casting");
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
    public bool CastUpdate()
    {
        return cast.CastUpdate();
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
        None,
        Horizontal,
        Vertical,
        Throwing,
    }
    CastState castState = CastState.None;
    public bool CastUpdate()
    {
        switch (castState)
        {
            case CastState.None:
                return false;
            case CastState.Horizontal:
                PlayHorizontal();
                return false;
            case CastState.Vertical:
                PlayVertical();
                return false;
            case CastState.Throwing:
                return IsBobberOnWater();
            default: return false;
        }
    }
    public Cast(Props castProps)
    {
        CastProperties = castProps;
    }
    public void CastClick()
    {
        Debug.Log("before" + castState);
        switch (castState)
        {
            case CastState.None:
                Debug.Log("Horizontal rn");
                amplitude = CastProperties.horizontalBar.parent.GetComponent<RectTransform>().rect.width - CastProperties.horizontalBar.GetComponent<RectTransform>().rect.width;
                castState = CastState.Horizontal;
                break;
            case CastState.Horizontal:
                Debug.Log("Vertical rn");
                castState = CastState.Vertical;
                break;
            case CastState.Vertical:
                Debug.Log("Throwing rn");
                castState = CastState.Throwing;
                BobberCast();
                break;
        }
    }
    float horizontalPercent = 0f;
    float verticalPercent = 0f;
    float amplitude;
    void PlayVertical()
    {
        verticalPercent = Mathf.PingPong(Time.time, 1f);
        CastProperties.verticalBar.localRotation = Quaternion.Euler(0f, 0f, -verticalPercent * 90f + 90f);
    }
    void PlayHorizontal()
    {
        horizontalPercent = Mathf.PingPong(Time.time, 1f);
        Debug.Log((horizontalPercent - 0.5f) * amplitude);
        CastProperties.horizontalBar.localPosition = new Vector3((horizontalPercent - 0.5f) * amplitude, 0f, 0f);
    }
    Transform bobberClone;
    void BobberCast()
    {
        Transform fishableArea = CastProperties.fishableArea;
        Vector3 areaSize = fishableArea.GetComponent<Renderer>().bounds.size;
        Vector3 areaPos = fishableArea.position;

        Vector3 bobberTarget = new Vector3(areaSize.x * horizontalPercent + areaPos.x - areaSize.x / 2f, areaPos.y, areaSize.z * verticalPercent + areaPos.z - areaSize.z / 2f);
        CastProperties.target.position = bobberTarget;


        bobberClone = GameObject.Instantiate(CastProperties.bobberObject);
        bobberClone.position = CastProperties.referenceObject.GetComponent<ReferenceScript>().player.position;
        Rigidbody rigidbody = bobberClone.GetComponent<Rigidbody>();
        Vector3 distance = CastProperties.target.position - bobberClone.position;

        float time = distance.magnitude / CastProperties.bobberVelocity;
        rigidbody.velocity = CastProperties.bobberVelocity * distance.normalized + new Vector3(0, time * Physics.gravity.magnitude * 0.5f);
    }
    bool IsBobberOnWater()
    {
        return bobberClone.GetComponent<Bobber>().IsTouchingWater;
    }

    public class Props
    {
        public Transform horizontalBar, verticalBar, fishableArea, target, bobberObject, referenceObject, waterObject;
        public float bobberVelocity;
        public Props(Transform horizontalBar, Transform verticalBar, Transform fishableArea, Transform target, Transform bobberObject, Transform referenceObject, Transform waterObject, float bobberVelocity)
        {
            this.horizontalBar = horizontalBar;
            this.verticalBar = verticalBar;
            this.fishableArea = fishableArea;
            this.target = target;
            this.bobberObject = bobberObject;
            this.referenceObject = referenceObject;
            this.bobberVelocity = bobberVelocity;
            this.waterObject = waterObject;
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