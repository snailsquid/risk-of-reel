using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static RodRegistry;

public class Rod
{
    public RodState RodState { get; set; } = RodState.PreCast;
    public string Name
    { get; private set; }
    public RodRarity RodRarity { get; private set; }
    public HookRarity HookRarity { get; private set; }
    public RodMechanics RodMechanics { get; private set; }
    public bool IsFishBite { get; private set; } = false;
    public Fish fishAttached;
    public BaitRegistry.BaitType BaitAttached;
    public Bucket currentBucket;
    public Rod(string name, RodRarity rodRarity)
    {
        Name = name;
        RodRarity = rodRarity;
    }
    public bool CanFish = true;
    TimeManager timeManager;
    CentralStateManager centralStateManager;
    public void SetRodMechanic(RodMechanics.Props props, TimeManager timeManager, CentralStateManager centralStateManager)
    {
        RodMechanics = new RodMechanics(props);
        this.timeManager = timeManager;
        this.centralStateManager = centralStateManager;
    }
    public void SetRodRarity(RodRarity rodRarity)
    {
        RodRarity = rodRarity;
    }
    public void SetBucket(Bucket bucket)
    {
        currentBucket = bucket;
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
                    Debug.Log("Finished casting");
                    RodState = RodState.FishWaiting;
                    Debug.Log((timeManager.CurrentTime, timeManager.maxTime));
                    if (BaitAttached != BaitRegistry.BaitType.None && RodMechanics.cast.CastProperties.itemManager.inventory.Items[ItemRegistry.BaitToBuy[BaitAttached]].Quantity <= 0) return;
                    RodMechanics.fishWait.WaitBite(BaitAttached, (timeManager.CurrentTime, timeManager.maxTime));
                };
                break;
            case RodState.FishWaiting:
                if (RodMechanics.fishWait.GetTempFishBite())
                {
                    IsFishBite = true;
                    fishAttached = RodMechanics.fishWait.TempFish;
                    Battle();
                }
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
        Debug.Log("Clicked");
        if (CanFish)
        {
            Debug.Log("can fish");
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
    }
    public void EquipBait(BaitRegistry.BaitType bait)
    {
        BaitAttached = bait;
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
        if (!currentBucket.AddFish(fishAttached)) { RodMechanics.battle.props.eventLog.Log("Bucket too full"); RodMechanics.postFish.props.centralStateManager.FinishRun(false); return; };
        RodMechanics.cast.bobberClone.GetComponent<Bobber>().FishLaunch(fishAttached.fishType);
        RodMechanics.battle.PopUp(fishAttached);
    }
    public void SetState(RodState rodState)
    {
        RodState = rodState;
    }
    public void Restart()
    {
        RodState = RodState.PreCast;
    }
    public void Battle()
    {
        //RodMechanics.cast.Restart();
        RodMechanics.battle.UI(true);
        RodMechanics.cast.UI(false);
        RodState = RodState.Battling;
        Debug.Log("battling");
        RodMechanics.cast.Splashing();
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
        RodMechanics.cast.bobberClone.GetComponent<AudioSource>().Stop();
        AudioManager.Instance.StopSound();
        AudioManager.Instance.PlaySFX(AudioRegistry.Sounds[AudioManager.Sound.WaterSplashPostFish]);
        RodMechanics.cast.Restart();
        Debug.Log("Successfully Battled the god damn fish");
        RodMechanics.battle.UI(false);
        RodMechanics.battle.Restart();
        RodMechanics.battle.props.linePointAttacher.Unequip();
        RodState = RodState.PostFish;
        PostFish();
    }
    public void BattleFail()
    {
        RodMechanics.cast.bobberClone.GetComponent<AudioSource>().Stop();
        AudioManager.Instance.StopSound();
        RodMechanics.cast.Restart();
        FishUnbite();
        RodState = RodState.PreCast;
        RodMechanics.battle.UI(false);
        GameObject.Destroy(RodMechanics.cast.bobberClone.gameObject);
    }
    public void Hide()
    {
        AudioManager.Instance.StopSound();
        RodMechanics.cast.Restart();
        FishUnbite();
        RodState = RodState.PreCast;
        RodMechanics.battle.UI(false);
    }
    public void FishUnbite()
    {
        IsFishBite = false;
    }

}
public class PostFish
{
    public class Props
    {
        public CentralStateManager centralStateManager;
        public PostRunPopup postRunPopup;
        public Bucket bucket;
        public Props(CentralStateManager centralStateManager, PostRunPopup postRunPopup, Bucket bucket)
        {
            this.centralStateManager = centralStateManager;
            this.postRunPopup = postRunPopup;
            this.bucket = bucket;
        }
    }
    public Props props;
    public PostFish(Props props)
    {
        this.props = props;
    }
}


public class RodMechanics
{
    public Cast cast;
    public Battle battle;
    public FishWait fishWait;
    public PostFish postFish;
    public RodMechanics(Props props)
    {
        cast = new Cast(props.castProps);
        battle = new Battle(props.battleProps);
        fishWait = new FishWait(props.fishProps);
        postFish = new PostFish(props.postFishProps);
    }
    public class Props
    {
        public Cast.Props castProps;
        public Battle.Props battleProps;
        public FishWait.Props fishProps;
        public PostFish.Props postFishProps;
        public Props(Cast.Props castProps, Battle.Props battleProps, FishWait.Props fishProps, PostFish.Props postFishProps)
        {
            this.castProps = castProps;
            this.battleProps = battleProps;
            this.fishProps = fishProps;
            this.postFishProps = postFishProps;
        }
    }
}
public class FishWait
{
    bool tempFishBite = false;
    public Fish TempFish;
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
    public async Task WaitBite(BaitRegistry.BaitType bait, (float current, float max) time)
    {
        Debug.Log("waiting..");
        float randomTime = Random.Range(props.FishBite.MinTime, props.FishBite.MaxTime) * 1000;
        Debug.Log(randomTime);
        await Task.Delay((int)randomTime);
        SetTempFishBite(true);
        TempFish = FishGenerator.GenerateFish(bait, time);
    }
    public void SetTempFishBite(bool value)
    {
        tempFishBite = value;
    }
    public bool GetTempFishBite()
    {
        if (tempFishBite)
        {
            SetTempFishBite(false);
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
    public class Props
    {
        public Transform hookBar { get; private set; }
        public Transform successBar { get; private set; }
        public float maxFishBiteTime { get; private set; }
        public Transform popup { get; private set; }
        public EventLog eventLog { get; private set; }
        public LinePointAttacher linePointAttacher;
        public Props(Transform hookBar, Transform successBar, float maxFishBiteTime, Transform popUp, EventLog eventLog, LinePointAttacher linePointAttacher)
        {
            this.hookBar = hookBar;
            this.successBar = successBar;
            this.maxFishBiteTime = maxFishBiteTime;
            this.popup = popUp;
            this.eventLog = eventLog;
            this.linePointAttacher = linePointAttacher;
        }
        public void SetmaxFishBiteTime(float time)
        {
            maxFishBiteTime = time;
        }
    }
    public Props props { get; private set; }
    public void UI(bool show)
    {
        props.hookBar.GetComponent<FishingProgress>().Reset();
        props.hookBar.GetComponent<FadeAnim>().SetUI(show);
        props.successBar.GetComponent<FadeAnim>().SetUI(show);
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
        props.linePointAttacher.Reel(true);

        if (FishTimer > props.maxFishBiteTime)
        {
            FishTimer = 0;
            props.linePointAttacher.Reel(false);
            props.eventLog.Log("Fish got away, took too long", 2);
            return false;
        }
        return true;
    }
    public void PopUp(Fish fish)

    {
        PopUp popUp = props.popup.GetComponent<PopUp>();
        popUp.SetText(fish);
        popUp.Show();
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
    float initialWidth;
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
        initialWidth = CastProperties.horizontalBar.GetComponent<RectTransform>().rect.width;
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
        CastProperties.verticalBar.localRotation = Quaternion.Euler(0f, 0f, -verticalPercent * 90f + 45f);
    }
    void PlayHorizontal()
    {
        horizontalPercent = Mathf.PingPong(Time.time, 1f);
        CastProperties.horizontalBar.localPosition = new Vector3((horizontalPercent - 0.5f) * amplitude, 0f, 0f);
    }
    public void Restart()
    {
        castState = CastState.None;
    }
    public Transform bobberClone { get; private set; }
    bool hasBobberLaunched;
    void BobberCast()
    {
        CastProperties.linePointAttacher.Cast();
        hasBobberLaunched = false;
        BobberLaunch();
    }
    async Task BobberLaunch()
    {
        await Task.Delay(300);

        AudioManager.Instance.PlaySFX(AudioRegistry.Sounds[AudioManager.Sound.RodWoosh]);
        await Task.Delay(400);
        hasBobberLaunched = true;
        bobberClone = GameObject.Instantiate(CastProperties.bobberObject);
        Transform fishableArea = CastProperties.fishableArea;
        Vector3 areaSize = fishableArea.GetComponent<Renderer>().bounds.size;
        Vector3 areaPos = fishableArea.position;

        Vector3 bobberTarget = new Vector3(areaSize.x * verticalPercent + areaPos.x - areaSize.x / 2f, areaPos.y, areaSize.z * -horizontalPercent + areaPos.z + areaSize.z / 2f);
        CastProperties.target.position = bobberTarget;

        bobberClone.position = CastProperties.referenceObject.GetComponent<ReferenceScript>().player.position;
        Rigidbody rigidbody = bobberClone.GetComponent<Rigidbody>();
        Vector3 distance = CastProperties.target.position - bobberClone.position;

        float time = distance.magnitude / CastProperties.bobberVelocity;
        rigidbody.velocity = CastProperties.bobberVelocity * distance.normalized + new Vector3(0, time * Physics.gravity.magnitude * 0.5f);
    }
    public void Splashing()
    {
        AudioManager.Instance.PlaySound(AudioRegistry.Sounds[AudioManager.Sound.WaterSplashBobber], bobberClone.GetComponent<AudioSource>());
        AudioManager.Instance.PlaySoundMain(AudioRegistry.Sounds[AudioManager.Sound.FishingLinePull]);
        bobberClone.GetComponent<Bobber>().Splash();
    }
    bool IsBobberOnWater()
    {
        if (!hasBobberLaunched) { return false; }
        if (bobberClone.GetComponent<Bobber>().IsTouchingWater)
        {
            Debug.Log("Bobber is on water");
            castState = CastState.None;
            return true;
        }
        else return false;
    }

    public class Props
    {
        public Transform horizontalBar, verticalBar, fishableArea, target, bobberObject, referenceObject, waterObject;
        public float bobberVelocity;
        public ItemManager itemManager;
        public LinePointAttacher linePointAttacher;
        public Props(Transform horizontalBar, Transform verticalBar, Transform fishableArea, Transform target, Transform bobberObject, Transform referenceObject, Transform waterObject, float bobberVelocity, ItemManager itemManager, LinePointAttacher linePointAttacher)
        {
            this.horizontalBar = horizontalBar;
            this.verticalBar = verticalBar;
            this.fishableArea = fishableArea;
            this.target = target;
            this.bobberObject = bobberObject;
            this.referenceObject = referenceObject;
            this.bobberVelocity = bobberVelocity;
            this.waterObject = waterObject;
            this.itemManager = itemManager;
            this.linePointAttacher = linePointAttacher;
        }
    }

    public void UI(bool show)
    {
        Debug.Log(show ? "Showing" : "Hiding");
        CastProperties.horizontalBar.parent.GetComponent<FadeAnim>().SetUI(show);
        CastProperties.verticalBar.parent.GetComponent<FadeAnim>().SetUI(show); ;
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
    public enum HookRarity
    {
        Basic,
        Super,
        Ultimate
    }
}