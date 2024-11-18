using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishLengthWeight : MonoBehaviour
{
    public TMP_Text fishText;
    private FishLength.FishLengthType ClassPanjangIkan;
    private FishWeight.FishWeightType ClassBeratIkan;
    public void Update()
    {
        string ClassPanjangIka = ClassPanjangIkan.ToString();
        string ClassBeratIka = ClassBeratIkan.ToString();
        fishText.text = $"{ClassPanjangIka}: {(float)FishLength.PanjangIkan}" +
            $"{ClassBeratIka}:{(float)FishWeight.BeratIkan}";
    }
}
