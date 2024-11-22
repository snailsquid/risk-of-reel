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
    CentralStateManager centralStateManager;
    float RealTime;
    bool isTimePlaying = false;
    public void UI(bool show)
    {
        timeText.gameObject.SetActive(show);
    }
    void Awake()
    {
        centralStateManager = GetComponent<CentralStateManager>();
    }
    public void StartTime()
    {
        isTimePlaying = true;
    }
    public void RestartTime()
    {

        RealTime = 0;
    }
    public void PauseTime()
    {
        isTimePlaying = false;
    }
    void Start()
    {
        isTimePlaying = true;
    }
    void Update()
    {
        if (centralStateManager.playerState == CentralStateManager.PlayerState.Rod)
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
            if (CurrentTime > maxTime)
            {
                centralStateManager.FinishRun(false);
            }
        }
    }
}
