using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform player, mainMenuPos, fishingPos, gameManager;
    [SerializeField] CanvasGroup mainMenuCanvas;
    [SerializeField] float transitionTime = 1f;
    void Start()
    {
        SwitchToMainMenu();
    }
    public void SwitchToMainMenu()
    {
        player.GetChild(0).GetComponent<MouseRotateCamera>().SetAble(false, mainMenuPos.eulerAngles);
        player.DOMove(mainMenuPos.position, 1f);
        player.DORotate(mainMenuPos.eulerAngles, 1f);
        player.GetChild(0).GetComponent<MouseRotateCamera>().SetAble(true, mainMenuPos.eulerAngles);
    }
    public void SwitchToFishing()
    {
        Debug.Log("hi");
        player.GetChild(0).GetComponent<MouseRotateCamera>().SetAble(false, fishingPos.eulerAngles);
        player.DORotate(fishingPos.eulerAngles, transitionTime);
        mainMenuCanvas.transform.DOMove(mainMenuCanvas.transform.position + new Vector3(0, 0, -10), transitionTime).SetEase(Ease.InOutQuart);
        mainMenuCanvas.DOFade(0, transitionTime);
        player.DOMove(fishingPos.position, transitionTime).SetEase(Ease.InOutQuart).onComplete = () =>
        {
            gameManager.GetComponent<CentralStateManager>().SetState(CentralStateManager.PlayerState.Rod);
        };
        player.GetChild(0).GetComponent<MouseRotateCamera>().SetAble(true, fishingPos.eulerAngles);
    }
}
