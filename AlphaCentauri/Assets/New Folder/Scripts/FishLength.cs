using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishLength : MonoBehaviour
{
    public enum FishLengthType
    {
        Long,
        Medium,
        Short
    }
    public static FishLengthType fishlengthType;

    public static float BatasBawahPanjang;//nanti ganti
    public static float BatasAtasPanjang;//nanti ganti
    public static float PanjangIkan;
    public GameObject popUp;
    private bool HasRun;

    public void Start()
    {
        BatasBawahPanjang = 1;//nanti ganti
        BatasAtasPanjang = 4;//nanti ganti
        PanjangIkan = 0;
        HasRun = false;
    }
    public void Update()
    {
        if (popUp.activeSelf == true && HasRun == false)
        {
            GenerateFishLength();
            HasRun = true;
            Debug.Log(PanjangIkan);
        }
        if (popUp.activeSelf == false)
        {
            HasRun = false;
        }
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
        if (PanjangIkan > (BatasBawahPanjang + (BatasAtasPanjang - BatasBawahPanjang) / 3)*2 && PanjangIkan < BatasAtasPanjang)
        {
            fishlengthType = FishLengthType.Long;
        }
    }
}