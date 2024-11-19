using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWeight : MonoBehaviour
{
    public enum FishWeightType
    {
        Heavy,
        Mid,
        Light
    }
    public static FishWeightType fishweightType;

    public static float BatasBawahBerat;//nanti ganti
    public static float BatasAtasBerat;//nanti ganti
    public static float BeratIkan;
    public GameObject popUp;
    private bool hasRun;
    public static float MaxBeratStorage;
    public static float IkanDiStorage;

    private void Start()
    {
        BatasBawahBerat = 1;//nanti ganti
        BatasAtasBerat = 4;//nanti ganti
        BeratIkan = 0;
        hasRun = false;
        MaxBeratStorage = 10;
        IkanDiStorage = 0;
    }
    private void Update()
    {
        if (popUp.activeSelf == true && hasRun == false)
        {
            GenerateWeight();
            Debug.Log(BeratIkan);
            Debug.Log(fishweightType);
            Storage();
            hasRun = true;
        }
        if (popUp.activeSelf == false)
        {
            hasRun = false;
        }
    }
    private void GenerateWeight()
    {
        float xRandom = Random.Range(1, 4);
        BeratIkan = JamText.DeltaJam / 6 * (BatasAtasBerat - BatasBawahBerat) * xRandom / 3 + BatasBawahBerat;

        if (BeratIkan < BatasBawahBerat + (BatasAtasBerat - BatasBawahBerat) / 3)
        {
            fishweightType = FishWeightType.Light;
        }
        if (BeratIkan > BatasBawahBerat + (BatasAtasBerat - BatasBawahBerat) / 3 && BeratIkan < (BatasBawahBerat + (BatasAtasBerat - BatasBawahBerat) / 3) * 2)
        {
            fishweightType = FishWeightType.Mid;
        }
        if (BeratIkan > (BatasBawahBerat + (BatasAtasBerat - BatasBawahBerat) / 3) * 2 && BeratIkan < BatasAtasBerat)
        {
            fishweightType = FishWeightType.Heavy;
        }

    }
    public void Storage()
    {
        IkanDiStorage += BeratIkan;
        if (IkanDiStorage > MaxBeratStorage)
        {
            EndRun();
        }
    }
    public void EndRun()
    {
        Debug.Log('k');
    }
}