using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraManager : MonoBehaviour
{
    [SerializeField] RodManager rodManager;
    [SerializeField] Transform player, mainMenuPos, fishingPos, gameManager, hidePos;
    [SerializeField] CanvasGroup mainMenuCanvas;
    [SerializeField] float transitionTime = 1f;
    [SerializeField] MouseRotateCamera mouseRotateCamera;
    void Start()
    {
        SwitchToMainMenu();
    }
    public void SwitchToMainMenu()
    {
        mouseRotateCamera.SetAble(false, mainMenuPos.eulerAngles);
        player.DOMove(mainMenuPos.position, 1f);
        player.DORotate(mainMenuPos.eulerAngles, 1f);
        mouseRotateCamera.SetAble(true, mainMenuPos.eulerAngles);
    }
    public void SwitchToFishing(float time = 1f)
    {
        mouseRotateCamera.SetAble(false, fishingPos.eulerAngles);
        player.DORotate(fishingPos.eulerAngles, time);
        mainMenuCanvas.transform.DOMove(mainMenuCanvas.transform.position + new Vector3(0, 0, -10), 1).SetEase(Ease.InOutQuart);
        mainMenuCanvas.DOFade(0, 1);
        player.DOMove(fishingPos.position, time).SetEase(Ease.InOutQuart).onComplete = () =>
        {
            Debug.Log("switch to fishing");
            gameManager.GetComponent<CentralStateManager>().SetState(CentralStateManager.PlayerState.Rod);
        };
        mouseRotateCamera.SetAble(true, fishingPos.eulerAngles);
    }
    public void SwitchToShop()
    {
        mouseRotateCamera.SetAble(false, fishingPos.eulerAngles);
        player.DORotate(fishingPos.eulerAngles, transitionTime);
        mainMenuCanvas.transform.DOMove(mainMenuCanvas.transform.position + new Vector3(0, 0, -10), 1).SetEase(Ease.InOutQuart);
        mainMenuCanvas.DOFade(0, 1);
        player.DOMove(fishingPos.position, transitionTime).SetEase(Ease.InOutQuart).onComplete = () =>
        {
            Debug.Log("switch to shop");
            gameManager.GetComponent<CentralStateManager>().SetState(CentralStateManager.PlayerState.Shop);
        };
        mouseRotateCamera.SetAble(true, fishingPos.eulerAngles);
    }
    public void SwitchToHide()
    {
        mouseRotateCamera.SetAble(false, hidePos.eulerAngles);
        player.transform.DOMove(hidePos.position, transitionTime);
        player.transform.DORotate(hidePos.rotation.eulerAngles, transitionTime).onComplete = () =>
        {
            mouseRotateCamera.SetAble(true, hidePos.eulerAngles);
        };
    }
    public void SwitchToCaught()
    {
        mouseRotateCamera.SetAble(false, hidePos.eulerAngles);
        player.transform.DOMove(fishingPos.position, 0.7f);
        player.transform.DORotate(new Vector3(-13, 231, 0), 0.7f).onComplete = () =>
        {
            gameManager.GetComponent<CentralStateManager>().FinishRun(true);
            player.DOMove(fishingPos.position, 0.5f).SetEase(Ease.InOutQuart);
            rodManager.equippedRod.SetState(RodRegistry.RodState.PreCast);
        };
    }
}
