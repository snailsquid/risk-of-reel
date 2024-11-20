using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JamText : MonoBehaviour
{
    public float time;
    public TMP_Text timeText;
    public static int DeltaJam;

    public void Start()
    {
        time = 0;
    }

    public void Update()
    {
        DeductTime();
        CheckTime();
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
            DeltaJam=1;
        }
        if (time > 60 && time<60*2)
        {
            DeltaJam = 2;
        }
        if (time > 60*2 && time < 60 * 3)
        {
            DeltaJam = 3;
        }
        if (time > 60*3 && time < 60 * 4)
        {
            DeltaJam = 4;
        }
        if (time > 60*4 && time < 60 * 5)
        {
            DeltaJam = 5;
        }
        if (time > 60 * 5 && time < 60 * 6)
        {
            DeltaJam = 6;
        }
        if (time > 60 * 6)
        {
            DeltaJam = 7;
            time = 6 * 60;
            //do something
        }
    }
}
