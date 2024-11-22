using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject Finish;
    [SerializeField] GameObject pauseMenuUI;
    public static bool gameIsPause = false;
    public Guard.PlayerState Trigger;
    void Start()
    {
        pauseMenuUI.SetActive(false);
    }
    void Update()
    {

    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
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
