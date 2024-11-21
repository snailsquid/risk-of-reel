using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject guard;
    [SerializeField] GameObject pauseMenuUI;
    public static bool gameIsPause = false;
    public Guard.PlayerState Trigger;
    void Start()
    {
        pauseMenuUI.SetActive(false);
        Trigger = guard.GetComponent<Guard>().playerState;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Trigger = Guard.PlayerState.Waiting;
        }
        Debug.Log(Trigger);
        if(Trigger == Guard.PlayerState.Waiting)
        {
            Pause();
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Trigger = Guard.PlayerState.Playing;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Restart()
    {
        Debug.Log("Go kys");
    }
}
