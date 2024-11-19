using System.Collections;
using System.Collections.Generic;
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
    float timeHiding = 0, timeUnhiding = 0;
    bool canHide = true;
    Vector3 velocity;
    public float cooldownLeft
    {
        get;
        private set;
    }
    public float forcedUnhideTime { get; private set; }
    void Update()
    {
        HideCounter();
    }

    void HideCounter()
    {
        if (isHide)
        {
            timeUnhiding = 0;

            timeHiding += Time.deltaTime;
            if (timeHiding > maxHideTime)
            {
                isHide = false;
            }
            LerpHide();
        }
        else
        {
            timeHiding = 0;
            timeUnhiding += Time.deltaTime;
            if (timeUnhiding > cooldownTime)
            {
                canHide = true;
            }
            else
            {
                canHide = false;
            }
            LerpUnhide();
        }
        cooldownLeft = cooldownTime - timeUnhiding;
        forcedUnhideTime = maxHideTime - timeHiding;

    }
    public void StartHide()
    {
        print("cant hide rn" + cooldownLeft);
        if (canHide)
        {
            print("I can hide");
            isHide = true;
        }
    }
    public void StopHide()
    {
        isHide = false;
    }

    void LerpHide()
    {
        transform.position = Vector3.SmoothDamp(transform.position, bush.position, ref velocity, timeHideAnim);
    }
    void LerpUnhide()
    {
        transform.position = Vector3.SmoothDamp(transform.position, playerDefaultLocation.position, ref velocity, timeHideAnim);
    }
}
