using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
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
    public bool IsFishBite { get; private set; } = false;
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
                bool isFinished = RodMechanics.cast.CastUpdate();
                if (isFinished)
                {
                    RodState = RodState.FishWaiting;
                    RodMechanics.fishWait.WaitBite();
                };
                break;
            case RodState.FishWaiting:
                if (RodMechanics.fishWait.GetTempFishBite()) { IsFishBite = true; Battle(); }
                break;
            case RodState.Battling:
                IsFishBite = RodMechanics.battle.BattleUpdate();
                if (!IsFishBite)
                { BattleFail(); }
                break;
            case RodState.PostFish:
                break;
        }
    }
    public void OnClick()
    {
        switch (RodState)
        {
            case RodState.PreCast:
                Cast();
                break;
            case RodState.Casting:
                Cast();
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
        Debug.Log("Casting");
        RodMechanics.cast.UI(true);
        RodState = RodState.Casting;
        RodMechanics.cast.CastClick();

    }
    public void PostFish()
    {
        RodMechanics.battle.UI(false);
        RodState = RodState.PreCast;
    }
    public void Battle()
    {
        RodMechanics.cast.Restart();
        RodMechanics.battle.UI(true);
        RodMechanics.cast.UI(false);
        RodState = RodState.Battling;
        Debug.Log("battling");
    }
    public void FishWait()
    {
        RodState = RodState.FishWaiting;
    }
    public void FinishFishing()
    {
        RodState = RodState.PostFish;
    }
    public void BattleSuccess()
    {
        Debug.Log("Successfully Battled the god damn fish");
        RodMechanics.battle.UI(false);
        RodState = RodState.PostFish;
    }
    public void BattleFail()
    {
        Debug.Log("You failed bruh");
        FishUnbite();
        RodState = RodState.PreCast;
        RodMechanics.battle.UI(false);
    }
    public void FishUnbite()
    {
        IsFishBite = false;
    }
}


public class RodMechanics
{
    public Cast cast;
    public Battle battle;
    public FishWait fishWait;
    public RodMechanics(Props props)
    {
        cast = new Cast(props.castProps);
        battle = new Battle(props.battleProps);
        fishWait = new FishWait(props.fishProps);
    }
    public class Props
    {
        public Cast.Props castProps;
        public Battle.Props battleProps;
        public FishWait.Props fishProps;
        public Props(Cast.Props castProps, Battle.Props battleProps, FishWait.Props fishProps)
        {
            this.castProps = castProps;
            this.battleProps = battleProps;
            this.fishProps = fishProps;
        }
    }
}
public class FishWait
{
    bool tempFishBite = false;
    public class Props
    {
        public (int MinTime, int MaxTime) FishBite;
        public Props((int MinTime, int MaxTime) fishBite)
        {
            FishBite = fishBite;
        }
    }
    Props props;
    public FishWait(Props props)
    {
        this.props = props;
    }
    public async Task WaitBite()
    {
        float time = Random.Range(props.FishBite.MinTime, props.FishBite.MaxTime) * 1000;
        Debug.Log(time);
        await Task.Delay((int)time);
        tempFishBite = true;
    }
    public bool GetTempFishBite()
    {
        if (tempFishBite)
        {
            tempFishBite = false;
            Debug.Log("Fish Bite");
            return true;
        }
        return false;
    }
}
public class Battle
{
    public float FishTimer { get; private set; } = 0f;
    public bool IsFishing { get; private set; } = false;
    public float MaxFishBite { get; private set; } = 10f;
    public class Props
    {
        public Transform hookBar { get; private set; }
        public Props(Transform hookBar)
        {
            this.hookBar = hookBar;
        }
    }
    Props props;
    public void UI(bool show)
    {
        props.hookBar.gameObject.SetActive(show);
    }
    public Battle(Props props)
    {
        this.props = props;
    }
    public void Restart()
    {
        FishTimer = 0;
    }
    public bool BattleUpdate()
    {
        FishTimer += Time.deltaTime;

        if (FishTimer > MaxFishBite)
        {
            FishTimer = 0;
            return false;
        }
        return true;
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
        CastProperties.horizontalBar.localPosition = new Vector3((horizontalPercent - 0.5f) * amplitude, 0f, 0f);
    }
    public void Restart()
    {
        GameObject.Destroy(bobberClone.gameObject);
        castState = CastState.None;
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
        if (bobberClone.GetComponent<Bobber>().IsTouchingWater)
        {
            castState = CastState.None;
            return true;
        }
        else return true;
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

    public void UI(bool show)
    {
        Debug.Log(show ? "Showing" : "Hiding");
        CastProperties.horizontalBar.parent.gameObject.SetActive(show);
        CastProperties.verticalBar.parent.gameObject.SetActive(show);
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