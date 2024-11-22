using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] Transform gameManager;
    public void Continue()
    {
        gameManager.GetComponent<CentralStateManager>().SetState(CentralStateManager.PlayerState.Rod);
    }
}
