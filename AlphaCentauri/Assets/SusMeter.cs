using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SusMeter : MonoBehaviour
{
    [SerializeField] Image left, right, exclamation;

    public void SetPercent(float percent)
    {
        left.fillAmount = percent / 2;
        right.fillAmount = percent / 2;
        Color exclamationColor = exclamation.color;
        exclamationColor.a = percent > 0.25f ? (percent - 0.5f) * 2 : 0;
        exclamation.color = exclamationColor;
    }
}
