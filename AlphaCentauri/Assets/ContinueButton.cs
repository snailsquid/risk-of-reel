using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] Transform gameManager, quickSwitchContainer;
    public void Continue()
    {
        quickSwitchContainer.GetComponent<QuickSwitch>().ResetUI();
        gameManager.GetComponent<CentralStateManager>().SetState(CentralStateManager.PlayerState.Rod);
        gameManager.GetComponent<RodManager>().equippedRod.CanFish = true;
    }
}
