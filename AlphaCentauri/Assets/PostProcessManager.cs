using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PostProcessManager : MonoBehaviour
{
    [SerializeField] TimeManager timeManager;
    [SerializeField] PostProcessVolume morning, night, evening;
    [SerializeField] (float start, float end) nightRange = (20, 3);
    float startTime;
    void Start()
    {
        startTime = timeManager.startTime;
    }
    void Update()
    {
        if (timeManager.CurrentTime + startTime < nightRange.start) // Transition to night
        {
            evening.weight = 0.5f - ((timeManager.CurrentTime) / (nightRange.start - startTime) / 2);
            night.weight = 0.5f + ((timeManager.CurrentTime) / (nightRange.start - startTime) / 2);
        }
        else if (timeManager.CurrentTime + startTime - 24 > nightRange.end) // Transition to morning
        {
            night.weight = 1 - ((timeManager.CurrentTime + startTime - 24 - nightRange.end) / (timeManager.maxTime - nightRange.end));
            Debug.Log((timeManager.CurrentTime + startTime - 24));
            morning.weight = (timeManager.CurrentTime + startTime - 24 - nightRange.end) / (timeManager.maxTime - nightRange.end);
        }
    }
}
