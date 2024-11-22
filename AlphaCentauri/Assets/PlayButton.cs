using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [SerializeField] Transform gameManager;
    public void OnClick()
    {
        gameManager.GetComponent<CentralStateManager>().StartGame();
    }
}
