using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Manager;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public bool isHide
    {
        get;
        private set;
    } = false;
    [SerializeField] float maxHideTime = 8, cooldownTime = 10, timeHideAnim = 0.8f;
    [SerializeField] Transform playerDefaultLocation, bush;
    [SerializeField] RodManager rodManager;
    [SerializeField] EventLog eventLog;
    [SerializeField] LinePointAttacher linePointAttacher;
    [SerializeField] ItemManager itemManager;
    [SerializeField] FishingProgress fishingProgress;
    [SerializeField] CameraManager cameraManager;
    float timeHiding = 0, timeUnhiding = 0;
    bool canHide = true;
    Vector3 velocity;
    CentralStateManager centralStateManager;

    public float cooldownLeft
    {
        get;
        private set;
    }
    public float forcedUnhideTime { get; private set; }
    void Start()
    {
        centralStateManager = GameObject.Find("GameManager").GetComponent<CentralStateManager>();
    }
    void Update()
    {
        if (isHide)
        {
            forcedUnhideTime += Time.deltaTime;
            cooldownLeft = 0;
        }
        else
        {
            cooldownLeft -= Time.deltaTime;
        }
        if (forcedUnhideTime >= maxHideTime)
        {
            forcedUnhideTime = 0;
            StopHide();
        }
    }

    public void StartHide()
    {
        print("cant hide rn" + cooldownLeft);
        if (canHide)
        {
            isHide = true;
            print("I can hide");
            GoHide();
            rodManager.equippedRod.Hide();
            fishingProgress.successCounter = 0;
            fishingProgress.success.GetComponent<FishRainbow>().value = 0;
            rodManager.equippedRod.CanFish = false;
            if (rodManager.equippedRod.RodMechanics.cast.bobberClone != null)
            {
                rodManager.equippedRod.RodMechanics.cast.bobberClone.GetComponent<Bobber>().Finish();
            }
            rodManager.equippedRod.RodMechanics.fishWait.SetTempFishBite(false);
            linePointAttacher.Unequip();
        }
    }
    public void GoHide()
    {
        cameraManager.SwitchToHide();
    }
    public void GoUnhide()
    {
        cameraManager.SwitchToFishing();
    }
    public void StopHide()
    {
        isHide = false;
        cooldownLeft = cooldownTime;
        Debug.Log("Unhiding");
        GoUnhide();
        rodManager.equippedRod.CanFish = true;
    }

}
