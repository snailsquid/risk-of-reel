using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventLog : MonoBehaviour
{
    [SerializeField] TMP_Text textObject;
    void Start()
    {
    }
    IEnumerator LogCoroutine(string log, double duration)
    {
        textObject.text = "[" + log + "]";
        yield return new WaitForSeconds((float)duration);
        textObject.text = "";
    }
    public void Log(string log, double duration = 1f)
    {
        StartCoroutine(LogCoroutine(log, duration));
    }
}
