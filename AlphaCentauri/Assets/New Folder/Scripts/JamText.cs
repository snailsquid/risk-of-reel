using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JamText : MonoBehaviour
{
    public float time;
    public TMP_Text timeText;
    public static float DeltaJam;
    public enum Jam
    {
        jam08,
        jam09,
        jam10,
        jam11,
        jam00,
        jam01,
        jam02
    }
    private Jam currentJam;


    public void Start()
    {
        time = 0;
    }

    public void Update()
    {
        DeductTime();
        CheckTime();
        GenerateDeltaJam();
    }

    public void GenerateDeltaJam()
    {
        if (currentJam == JamText.Jam.jam08)
        {
            DeltaJam = 1;//bukan 0 krn nanti di awal masa pasti batas bawah
        }
        else if (currentJam == JamText.Jam.jam09)
        {
            DeltaJam = 2;
        }
        else if (currentJam == JamText.Jam.jam10)
        {
            DeltaJam = 3;
        }
        else if (currentJam == JamText.Jam.jam11)
        {
            DeltaJam = 4;
        }
        else if (currentJam == JamText.Jam.jam00)
        {
            DeltaJam = 5;
        }
        else if (currentJam == JamText.Jam.jam01)
        {
            DeltaJam = 6;
        }
        else
        {
            DeltaJam = 10000000;
        }
    }

    public void DeductTime()
    {
        time += Time.deltaTime / 2; //jam 8:00 itu jam 0 jam 14:00 itu jam 4
        timeText.text = string.Format("{0:00}:{1:00}", ((int)8 + time / 60), ((int)time % 60));
    }

    public void CheckTime()
    {
        //for i in range (7)
        //1 billion if statements

        if (time < 60)
        {
            currentJam = Jam.jam08;
        }
        if (time > 60 && time<60*2)
        {
            currentJam = Jam.jam09;
        }
        if (time > 60*2 && time < 60 * 3)
        {
            currentJam = Jam.jam10;
        }
        if (time > 60*3 && time < 60 * 4)
        {
            currentJam = Jam.jam11;
        }
        if (time > 60*4 && time < 60 * 5)
        {
            currentJam = Jam.jam00;
        }
        if (time > 60 * 5 && time < 60 * 6)
        {
            currentJam = Jam.jam01;
        }
        if (time > 60 * 6)
        {
            currentJam = Jam.jam02;
            time = 6 * 60;
            //do something
        }
    }
}
