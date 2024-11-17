using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JamCycle : MonoBehaviour
{
    public TMP_Text timeText;
    private float time;

    private void Start()
    {
        time = 0;
    }
    private void Update()
    {
        DeductTime();
        CheckTime();
    }
    private void DeductTime()
    {
        time += Time.deltaTime/2; //jam 8:00 itu jam 0 jam 14:00 itu jam 4
        timeText.text = string.Format("{0:00}:{1:00}", ((int)8+time/60),((int)time%60)); 
    }
    private void CheckTime()
    {
        if (time > 6*60)
        {
            time = 6 * 60;
            //do something
        }
    }
}
