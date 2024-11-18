using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodCasting : MonoBehaviour
{
    [SerializeField] private Transform horizontalBar, verticalBar, fishableArea, target, bobberObject, referenceObject;
    public float bobberVelocity = 5f;
    Transform bobberClone;
    Transform player;
    float horizontalPercent = 0f; //right left percent
    float verticalPercent = 0f; //up down percent
    bool clicked = false, playHorizontal = false, playVertical = false;
    float amplitude;
    bool Pulling;
    bool isThereABite;
    bool IsFishing;
    float FishHadEnough = 0f;
    /// <summary>
    /// Places bobber target 
    /// </summary>
    void CastRod()
    {
        Vector3 areaSize = fishableArea.GetComponent<Renderer>().bounds.size;
        Vector3 areaPos = fishableArea.position;
        Vector3 bobberTarget = new Vector3(areaSize.x * horizontalPercent + areaPos.x - areaSize.x / 2f, areaPos.y, areaSize.z * verticalPercent + areaPos.z - areaSize.z / 2f);
        target.position = bobberTarget;
        BobberThrow();
    }
    /// <summary>
    /// Bobber throw animation
    /// </summary>
    void BobberThrow()
    {
        bobberClone = Instantiate(bobberObject);
        bobberClone.position = player.position;
        Rigidbody rigidbody = bobberClone.GetComponent<Rigidbody>();
        Vector3 distance = target.position - bobberClone.position;
        float time = distance.magnitude / bobberVelocity;
        rigidbody.velocity = bobberVelocity * distance.normalized + new Vector3(0, time * Physics.gravity.magnitude * 0.5f);
        // bobberClone.position = target.position;
        StartFishing();
        //Trigger the fish eat the bait
    }
    /// <summary>
    /// Start the power level bar minigame
    /// </summary>
    void SetPowerLevel()
    {
        amplitude = horizontalBar.parent.GetComponent<RectTransform>().rect.width - horizontalBar.GetComponent<RectTransform>().rect.width;
        playHorizontal = true;
    }
    // Tween the bar of rightLeft or upDown
    void StartFishing()
    {
        StartCoroutine(FishingCorountine());
    }
    IEnumerator FishingCorountine()
    {
        if (IsFishing)
        {
            int wait_time = Random.Range(5, 10);//Wait random time to fish to bite the bait
            yield return new WaitForSeconds(wait_time);
        }
        if (IsFishing)
        {
            Debug.Log("Biting");
            StartCoroutine(StartFishStruggle());//Start fish struggle
        }

    }
    IEnumerator StartFishStruggle()
    {
        isThereABite = true;
        //wait until pull
        while (!Pulling)
        {
            yield return FishHadEnough += Time.deltaTime;
            if (FishHadEnough > 10)
            {
                isThereABite = false;
                Debug.Log("The fish go away");
                StartFishing();
                FishHadEnough = 0f;
                break;
            }
        }
        if (isThereABite)
        {
            Debug.Log("Start fish battle");//Start fish battle
            isThereABite = false;
            FishHadEnough = 0f;
        }
    }
    public void SetPulling()
    {
        Pulling = true;
    }
    void EndFishing()
    {
        Pulling = false;
        isThereABite = false;
        FishHadEnough = 0f;
    }

    void Start()
    {
        player = referenceObject.GetComponent<ReferenceScript>().player;
        SetPowerLevel();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
        }
        if (playHorizontal && clicked == false)  // Play Horizontal
        {
            horizontalPercent = Mathf.PingPong(Time.time, 1f);
            horizontalBar.localPosition = new Vector3((horizontalPercent - 0.5f) * amplitude, 0f, 0f);
        }
        if (playVertical && clicked == false)// Play Vertical
        {
            verticalPercent = Mathf.PingPong(Time.time, 1f);
            verticalBar.localRotation = Quaternion.Euler(0f, 0f, -verticalPercent * 90f + 90f);
        }
        if (clicked) // Debounces
        {
            if (playHorizontal) //Play Horizontal after Vertical
            {
                Debug.Log(horizontalPercent);
                playHorizontal = false;
                clicked = false;
                playVertical = true;
            }
            else if (playVertical)
            {
                Debug.Log(verticalPercent);
                playVertical = false;
                clicked = false;
                IsFishing = true;
                CastRod();
            }
            else if (isThereABite)
            {
                clicked = false;
                SetPulling();
            }
            else
            {
                clicked = false;
                IsFishing = false;
                EndFishing();
                SetPowerLevel();
            }
        }
    }
}

