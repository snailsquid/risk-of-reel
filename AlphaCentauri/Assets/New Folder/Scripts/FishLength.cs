using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishLength
{
    
    public enum FishLengthType
    {
        Long,
        Medium,
        Short
    }
    FishLengthType fishlengthType;

    public JamText.Jam currentJam;
    public static float BatasBawahPanjang;//nanti ganti
    public static float BatasAtasPanjang;//nanti ganti
    public static float PanjangIkan;

    public void start()
    {
        BatasBawahPanjang = 1;//nanti ganti
        BatasAtasPanjang = 4;//nanti ganti
        PanjangIkan = 0;
    }

    public void update()
    {
        GenerateFishLength();
    }
    public void GenerateFishLength()
    {
        float xRandom = Random.Range(1, 4);
        PanjangIkan = JamText.DeltaJam / 6 * (BatasAtasPanjang - BatasBawahPanjang) * xRandom / 3 + BatasBawahPanjang;
        if (PanjangIkan < BatasBawahPanjang + (BatasAtasPanjang - BatasBawahPanjang) / 3)
        {
            fishlengthType = FishLengthType.Short;
        }
        if (PanjangIkan > BatasBawahPanjang + (BatasAtasPanjang - BatasBawahPanjang) / 3 && PanjangIkan < (BatasBawahPanjang + (BatasAtasPanjang - BatasBawahPanjang) / 3)*2)
        {
            fishlengthType = FishLengthType.Medium;
        }
        if (PanjangIkan < BatasBawahPanjang + (BatasAtasPanjang - BatasBawahPanjang)/3 && PanjangIkan > (BatasBawahPanjang + (BatasAtasPanjang - BatasBawahPanjang) / 3)*2)
        {
            fishlengthType = FishLengthType.Long;
        }
    }
}