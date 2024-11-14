using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingProgress : MonoBehaviour
{
    public RectTransform fishTransform;
    public RectTransform hookTransform;
    public bool HookTouchFish;
    void Start()
    {
        
    }

    void Update()
    {
        if(CheckTouching(fishTransform,hookTransform))
    }
}
