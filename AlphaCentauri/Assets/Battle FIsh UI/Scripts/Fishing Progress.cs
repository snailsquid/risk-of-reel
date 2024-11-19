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
    public float failrate = 10;
    float successbar = 100;
    float failbar = -100;
    float successCounter = 0;
    void Start()
    {

    }

    void Update()
    {
        if (fishTransform.Overlaps(hookTransform))
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
        if (HookTouchFish)
        {
            successCounter += successrate * Time.deltaTime;
        }
        else
        {
            successCounter -= failrate * Time.deltaTime;
        }
        successCounter = Mathf.Clamp(successCounter, failbar, successbar);
        //Progress bar
        success.value = successCounter;
        //Success or fail
        if (successCounter >= successbar)
        {
            Debug.Log("Success");
            successCounter = 0;
            success.value = 0;
        }
        else if (successCounter <= failbar)
        {
            Debug.Log("Fail");
            successCounter = 0;
            success.value = 0;
        }
    }
}
public static class RectTransformExtensions
{

    public static bool Overlaps(this RectTransform a, RectTransform b)
    {
        return a.WorldRect().Overlaps(b.WorldRect());
    }
    public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
    {
        return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
    }

    public static Rect WorldRect(this RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        Vector2 pivot = rectTransform.pivot;

        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        //With this it works even if the pivot is not at the center
        Vector3 position = rectTransform.TransformPoint(rectTransform.rect.center);
        float x = position.x - rectTransformWidth * 0.5f;
        float y = position.y - rectTransformHeight * 0.5f;

        return new Rect(x, y, rectTransformWidth, rectTransformHeight);
    }

}