using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform player, mainMenuPos, fishingPos, gameManager;
    [SerializeField] CanvasGroup mainMenuCanvas;
    [SerializeField] float transitionTime = 1f;
    public void SwitchToMainMenu()
    {
        player.DOMove(mainMenuPos.position, 1f);
        player.DORotate(mainMenuPos.rotation.eulerAngles, 1f);
    }
    public void SwitchToFishing()
    {
        player.GetChild(0).GetComponent<MouseRotateCamera>().SetAble(false, fishingPos.rotation.eulerAngles);
        player.DORotate(fishingPos.rotation.eulerAngles, transitionTime);
        mainMenuCanvas.transform.DOMove(mainMenuCanvas.transform.position + new Vector3(0, 0, -10), transitionTime).SetEase(Ease.InOutSine);
        mainMenuCanvas.DOFade(0, transitionTime);
        player.DOMove(fishingPos.position, transitionTime).SetEase(Ease.OutBack).onComplete = () =>
        {
            gameManager.GetComponent<CentralStateManager>().SetState(CentralStateManager.PlayerState.Rod);
            player.GetChild(0).GetComponent<MouseRotateCamera>().SetAble(true, fishingPos.rotation.eulerAngles);
        };
    }
}
