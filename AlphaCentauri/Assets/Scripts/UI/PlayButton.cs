using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] Transform gameManager;
    public void OnClick()
    {
        Debug.Log("Play Button Clicked");
        gameManager.GetComponent<CentralStateManager>().StartGame();
    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
}
