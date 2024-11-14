using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingProgress : MonoBehaviour
{
    public RectTransform fishTransform;
    public RectTransform hookTransform;
    public bool HookTouchFish;
    public Slider success;
    public float successrate = 15;
    public float failrate = 14;
    float successbar = 100;
    float failbar = -100;
    float successCounter = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if(CheckTouching(fishTransform,hookTransform))
        {
            HookTouchFish = true;
        }
        else
        {
            HookTouchFish = false;
        }
        Calculation();
    }
    private void Calculation()
    {
        if(HookTouchFish)
        {
            successCounter += successrate * Time.deltaTime;
        }
        else
        {
            successCounter -= failrate * Time.deltaTime;
        }
        successCounter = Mathf.Clamp(successCounter,failbar,successbar);
        //Progress bar
        success.value=successCounter;
        //Success or fail
        if (successCounter >= successbar)
        {
            Debug.Log("Success");
            successCounter = 0;
            success.value = 0;
        }
        else if(successCounter <= failbar)
        {
            Debug.Log("Fail");
            successCounter = 0;
            success.value = 0;
        }
    }
    private bool CheckTouching(RectTransform rect1, RectTransform rect2)//Checking if Hook touching Fish
    {
        Rect r1 = new Rect(rect1.position.x, rect1.position.y,rect1.rect.width,rect1.rect.height);
        Rect r2 = new Rect(rect2.position.x, rect2.position.y,rect2.rect.width,rect2.rect.height);
        return r1.Overlaps(r2);
    }
}
