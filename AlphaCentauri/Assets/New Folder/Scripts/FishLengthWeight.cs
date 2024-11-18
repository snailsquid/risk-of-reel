using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishLengthWeight : MonoBehaviour
{
    public TMP_Text fishText;

    public GameObject popUp;
    public void Update()
    {
        FishStatsPrint();
    }
    public void FishStatsPrint()
    {
        fishText.text = $"{FishLength.fishlengthType}: {(float)FishLength.PanjangIkan}" +
            $"{FishWeight.fishweightType}:{(float)FishWeight.BeratIkan}";
    }
}
