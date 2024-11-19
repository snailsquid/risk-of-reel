using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishText : MonoBehaviour
{
    public GameObject popUp;
    public TMP_Text fishText;
    public static string FishType;//nanti diganti
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
        fishText.text = $"{FishType}<br>{FishLength.fishlengthType}: {(FishLength.PanjangIkan)} cm <br>{FishWeight.fishweightType}:{(float)FishWeight.BeratIkan} kg";
    }
}
