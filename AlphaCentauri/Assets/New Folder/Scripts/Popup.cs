using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PopUp : MonoBehaviour
{
    [SerializeField] TMP_Text fishName, fishWeightAndLength;
    public static string FishType;//nanti diganti

    public void SetText(string name, float weight, float length)
    {
        fishName.text = name;
        fishWeightAndLength.text = weight + "kg\n" + length + "m";
    }
    public void Show()
    {
        transform.DOScale(new Vector2(0.5f, 0.5f), 2);
    }
}
