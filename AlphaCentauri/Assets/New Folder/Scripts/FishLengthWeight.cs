using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishLengthWeight : MonoBehaviour
{
    public GameObject popUp;
    public TMP_Text fishText;
    public string FishType;
    public void Start()
    {
        FishType = "Salmon";
    }

    public void Update()
    {
        FishStatsPrint();
    }
    public void FishStatsPrint()
    {
        fishText.text = $"{FishType}<br>{FishLength.fishlengthType}: {(float)FishLength.PanjangIkan}<br>{FishWeight.fishweightType}:{(float)FishWeight.BeratIkan}";
    }
}
