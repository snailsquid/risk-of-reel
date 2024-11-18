using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWeight
{
    public JamText script;
    public enum FishWeightType
    {
        Heavy,
        Mid,
        Light
    }
    FishWeightType fishweightType;

    public JamText.Jam currentJam;
    public static float BatasBawahBerat;//nanti ganti
    public static float BatasAtasBerat;//nanti ganti
    public static float BeratIkan;

    public void start()
    {
        BatasBawahBerat = 1;//nanti ganti
        BatasAtasBerat = 4;//nanti ganti
        BeratIkan = 0;
    }
    public void update()
    {
        GenerateWeight();
    }
    public void GenerateWeight()
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
        if (BeratIkan < BatasBawahBerat + (BatasAtasBerat - BatasBawahBerat)/3 && BeratIkan > (BatasBawahBerat + (BatasAtasBerat - BatasBawahBerat) / 3) * 2)
        {
            fishweightType = FishWeightType.Heavy;
        }
    }
}