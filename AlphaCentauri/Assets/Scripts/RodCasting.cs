using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class RodCasting : MonoBehaviour
{
    [SerializeField] private Transform horizontalBar, verticalBar;
    float horizontalPercent = 0f; //right left percent
    float verticalPercent = 0f; //up down percent
    bool clicked = false, playHorizontal = false, playVertical = false;
    float amplitude;
    void CastRod()
    {

    }
    void GetPowerLevel()
    {
        playHorizontal = true;
    }
    // Tween the bar of rightLeft or upDown
    void Start()
    {
        amplitude = horizontalBar.parent.GetComponent<RectTransform>().rect.width - horizontalBar.GetComponent<RectTransform>().rect.width;
        GetPowerLevel();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
        }
        if (playHorizontal && clicked == false)
        {
            horizontalPercent = Mathf.PingPong(Time.time, 1f);
            Debug.Log("hh");
            horizontalBar.localPosition = new Vector3((horizontalPercent - 0.5f) * amplitude, 0f, 0f);
        }
        if (playVertical && clicked == false)
        {
            verticalPercent = Mathf.PingPong(Time.time, 1f);
            Debug.Log("hi");
            verticalBar.localRotation = Quaternion.Euler(0f, 0f, verticalPercent * 90f);
        }
        if (clicked)
        {
            if (playHorizontal)
            {
                playHorizontal = false;
                clicked = false;
                playVertical = true;
            }
            else if (playVertical)
            {
                playVertical = false;
            }
        }
    }
}

