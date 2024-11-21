using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float CurrentTime { get; private set; }
    public float maxTime { get; private set; } = 6;
    [SerializeField] float realToGameTime = 30;
    [SerializeField] TMP_Text timeText;
    float RealTime;
    bool isTimePlaying = false;
    void StartTime()
    {
        RealTime = 0;
        isTimePlaying = true;
    }
    void PauseTime()
    {
        isTimePlaying = false;
    }
    void Start()
    {
        isTimePlaying = true;
    }
    void Update()
    {
        if (isTimePlaying)
        {
            RealTime += Time.deltaTime;
        }
        CurrentTime = RealTime * realToGameTime / 3600f;
        float roundedTime = Mathf.Floor(CurrentTime);
        float hour = roundedTime < 4 ? roundedTime + 8 : roundedTime - 2;
        float minute = (float)Math.Floor(CurrentTime % 1 * 100 * 0.6) % 60;
        timeText.text = String.Format("{0:00}:{1:00}", hour, minute);
    }
    void EndRun()
    {
        PauseTime();
    }
}
