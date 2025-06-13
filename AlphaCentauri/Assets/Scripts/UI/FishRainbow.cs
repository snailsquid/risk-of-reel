using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishRainbow : MonoBehaviour
{
    [SerializeField] RectMask2D mask;
    public float value = 0;
    public float maxValue = 405f;
    void Update()
    {
        float actualValue = (-(value) / 200 * maxValue) + maxValue / 2;
        mask.padding = new Vector4(0, 0, actualValue, 0);
    }
}
