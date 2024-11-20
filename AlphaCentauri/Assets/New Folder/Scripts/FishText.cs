using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FishText : MonoBehaviour
{
    [SerializeField] TMP_Text fishName, fishWeightAndLength;
    public TMP_Text fishText;
    public static string FishType;//nanti diganti
    public void Start()
    {
        FishType = "Salmon";
    }

    public void SetText(string name, float weight, float length)
    {
        fishName.text = name;
        fishWeightAndLength.text = weight + "kg\n" + length + "m";
    }
    public void PopUp()
    {
        transform.DOScale(new Vector2(0.5f, 0.5f), 2);
    }
}
